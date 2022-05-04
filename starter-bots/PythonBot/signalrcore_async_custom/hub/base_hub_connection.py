import asyncio
import ssl
import threading
import time
import uuid

import requests
import websockets
from signalrcore_async.helpers import Helpers
from signalrcore_async.messages import (InvocationMessage, MessageType,
                                        PingMessage, StreamInvocationMessage)

from ..protocol.json import JsonHubProtocol
from .connection_state import ConnectionState
from .errors import HubError, UnAuthorizedHubError
from .reconnection import ConnectionStateChecker


class StreamHandler(object):
    def __init__(self, event, invocation_id):
        self.event = event
        self.invocation_id = invocation_id
        self.next_callback = None

    def subscribe(self, subscribe_callbacks):
        if subscribe_callbacks is None:
            raise ValueError(" subscribe object must be {0}".format({
                "next": None
                }))
        self.next_callback = subscribe_callbacks["next"]

class async_event(asyncio.Event):
    def set(self):
        self._loop.call_soon_threadsafe(super().set)

class WebSocketsConnection(object):

    last_result = None
    last_invocation_id = None
    last_error = None
    event = None
    loop = None

    def __init__(self, hubConnection):
        self._hub_connection = hubConnection

    async def run(self):
        url = self._hub_connection.url
        headers = self._hub_connection.headers
        max_size = 1_000_000_000
        
        # connect
        self._ws = await websockets.connect(url, max_size=max_size, extra_headers=headers)
        self._hub_connection.logger.debug("-- web socket open --")

        # handshake
        msg = self._hub_connection.protocol.handshake_message()
        self._hub_connection._internal_send(msg, self._hub_connection.handshake_protocol)
        response = await self._ws.recv()
        self._hub_connection.evaluate_handshake(response)

        if self._hub_connection.on_connect is not None and callable(self._hub_connection.on_connect):
            self._hub_connection.state = ConnectionState.connected
            self._hub_connection.on_connect()

        # message loop
        self.loop = asyncio.create_task(self._receive_messages())
        
    async def invoke(self, data, invocationId):

        self.event = async_event()
        self.last_invocation_id = invocationId
        
        await self._ws.send(data)
        await self.event.wait()

        if (self.last_error is not None):
            raise Exception(self.last_error)
        else:
            return self.last_result

    def send(self, data):
        asyncio.create_task(self._ws.send(data))

    def handle_completion(self, message):
        if message.invocation_id == self.last_invocation_id:
            if message.error is not None:
                self.last_result = None
                self.last_error = message.error
                self.event.set() 
            else:
                self.last_result = message.result
                self.last_error = None
                self.event.set()

        self.last_invocation_id = -1

    async def close(self):
        self._hub_connection.logger.debug("-- web socket close --")

        if self._hub_connection.on_disconnect is not None and callable(self._hub_connection.on_disconnect):
            self._hub_connection.on_disconnect()

        if (self._ws is not None):
            await self._ws.close()

        self.last_error = "The connection was closed unexpectedly."

        if (self.event is not None):
            self.event.set()

        if (self.loop is not None):
            self.loop.cancel()

    async def _receive_messages(self):
        while (True):
            raw_message = await self._ws.recv()
            self._hub_connection.on_message(raw_message)

class BaseHubConnection(object):
    def __init__(
            self,
            url,
            protocol,
            headers={},
            keep_alive_interval=15,
            reconnection_handler=None,
            verify_ssl=False,
            skip_negotiation=False):
        self.skip_negotiation = skip_negotiation
        self.logger = Helpers.get_logger()
        self.url = url
        self.protocol = protocol
        self.handshake_protocol = JsonHubProtocol()
        self.headers = headers
        self.handshake_received = False
        self.token = None # auth
        self.state = ConnectionState.disconnected
        self.connection_alive = False
        self.handlers = []
        self.stream_handlers = []
        self._thread = None
        self._ws = None
        self.verify_ssl = verify_ssl
        self.connection_checker = ConnectionStateChecker(
            lambda: self._internal_send(PingMessage()),
            keep_alive_interval
        )
        self.reconnection_handler = reconnection_handler
        self.on_connect = None
        self.on_disconnect = None

    def negotiate(self):
        negotiate_url = Helpers.get_negotiate_url(self.url)
        self.logger.debug("Negotiate url:{0}".format(negotiate_url))

        response = requests.post(negotiate_url, headers=self.headers, verify=self.verify_ssl)
        self.logger.debug("Response status code{0}".format(response.status_code))

        if response.status_code != 200:
            raise HubError(response.status_code) if response.status_code != 401 else UnAuthorizedHubError()
        data = response.json()
        if "connectionId" in data.keys():
            self.url = Helpers.encode_connection_id(self.url, data["connectionId"])

        # Azure
        if 'url' in data.keys() and 'accessToken' in data.keys():
            Helpers.get_logger().debug("Azure url, reformat headers, token and url {0}".format(data))
            self.url = data["url"] if data["url"].startswith("ws") else Helpers.http_to_websocket(data["url"])
            self.token = data["accessToken"]
            self.headers = {"Authorization": "Bearer " + self.token}

    async def start(self):
        if not self.skip_negotiation:
            self.negotiate()
        self.logger.debug("Connection started")
        if self.state == ConnectionState.connected:
            self.logger.warning("Already connected unable to start")
            return
        self.state = ConnectionState.connecting
        self.logger.debug("start url:" + self.url)

        self._ws = WebSocketsConnection(self)
        await self._ws.run()

    async def stop(self):
        self.logger.debug("Connection stop")
        if self.state == ConnectionState.connected:
            await self._ws.close()
            self.connection_checker.stop()
            self.state == ConnectionState.disconnected

    def register_handler(self, event, callback):
        self.logger.debug("Handler registered started {0}".format(event))
        self.handlers.append((event, callback))

    def evaluate_handshake(self, message):
        self.logger.debug("Evaluating handshake {0}".format(message))
        msg = self.handshake_protocol.decode_handshake(message)
        if msg.error is None or msg.error == "":
            self.handshake_received = True
            self.state = ConnectionState.connected
            if self.reconnection_handler is not None:
                self.reconnection_handler.reconnecting = False
                if not self.connection_checker.running:
                    self.connection_checker.start()
        else:
            self.logger.error(msg.error)
            raise ValueError("Handshake error {0}".format(msg.error))

    def on_message(self, raw_message):

        # self.logger.debug("Message received{0}".format(raw_message))
        self.connection_checker.last_message = time.time()
        messages = self.protocol.parse_messages(raw_message)

        for message in messages:
            if message.type == MessageType.invocation_binding_failure:
                self.logger.error(message)
                continue
            if message.type == MessageType.ping:
                continue

            if message.type == MessageType.invocation:
                fired_handlers = list(
                    filter(
                        lambda h: h[0] == message.target,
                        self.handlers))
                if len(fired_handlers) == 0:
                    self.logger.warning(
                        "event '{0}' hasn't fire any handler".format(
                            message.target))
                for _, handler in fired_handlers:
                    handler(message.arguments)

            if message.type == MessageType.close:
                self.logger.info("Close message received from server")
                asyncio.create_task(self.stop())
                return

            if message.type == MessageType.completion:
                self._ws.handle_completion(message)

            if message.type == MessageType.stream_item:
                fired_handlers = list(
                    filter(
                        lambda h: h.invocation_id == message.invocation_id,
                        self.stream_handlers))
                if len(fired_handlers) == 0:
                    self.logger.warning(
                        "id '{0}' hasn't fire any stream handler".format(
                            message.invocation_id))
                for handler in fired_handlers:
                    handler.next_callback(message.item)

            if message.type == MessageType.stream_invocation:
                pass

            if message.type == MessageType.cancel_invocation:
                pass # not implemented

    async def invoke(self, method, arguments):
        if type(arguments) is not list:
            raise HubConnectionError("Arguments of a message must be a list")

        if type(arguments) is list:
            invocation_id = str(uuid.uuid4())
            message = InvocationMessage({}, invocation_id, method, arguments)
            return await self._internal_invoke(message)


    async def _internal_invoke(self, message, protocol=None):

        self.logger.debug("Sending message.".format(message))

        try:
            protocol = self.protocol if protocol is None else protocol
            invocation_id = message.invocation_id
            result = await self._ws.invoke(protocol.encode(message), invocation_id)

            self.connection_checker.last_message = time.time()

            if self.reconnection_handler is not None:
                self.reconnection_handler.reset()

            return result
            
        except Exception as ex:
            raise ex

    def send(self, method, arguments):
        if type(arguments) is not list and type(arguments) is not Subject:
            raise HubConnectionError("Arguments of a message must be a list or subject")

        if type(arguments) is list:
            self._internal_send(InvocationMessage(
                {},
                0,
                method,
                arguments))

        if type(arguments) is Subject:
            arguments.connection = self
            arguments.target = method
            arguments.start()

    def _internal_send(self, message, protocol=None):

        self.logger.debug("Sending message {0}".format(message))

        try:
            protocol = self.protocol if protocol is None else protocol

            self._ws.send(protocol.encode(message))
            self.connection_checker.last_message = time.time()

            if self.reconnection_handler is not None:
                self.reconnection_handler.reset()

        except Exception as ex:
            raise ex

    def handle_reconnect(self):
        self.reconnection_handler.reconnecting = True
        try:
            self.stop()
            self.start()
        except Exception as ex:
            self.logger.error(ex)
            sleep_time = self.reconnection_handler.next()
            threading.Thread(
                target=self.deferred_reconnect,
                args=(sleep_time,)
            )

    def deferred_reconnect(self, sleep_time):
        time.sleep(sleep_time)
        try:
            if not self.connection_alive:
                self._send_ping()
        except Exception as ex:
            self.reconnection_handler.reconnecting = False
            self.connection_alive = False

    async def stream(self, event, event_params, on_next_item):
        invocation_id = str(uuid.uuid4())
        stream_obj = StreamHandler(event, invocation_id)
        stream_obj.subscribe({ "next": on_next_item })
        self.stream_handlers.append(stream_obj)
        await self._internal_invoke(
            StreamInvocationMessage(
                {},
                invocation_id,
                event,
                event_params))

    def on_close(self, callback):
        self.on_disconnect = callback

    def on_open(self, callback):
        self.on_connect = callback

    def on(self, event, callback_function):
        """
        Register a callback on the specified event
        :param event: Event name
        :param callback_function: callback function, arguments will be binded
        :return:
        """
        self.register_handler(event, callback_function)
