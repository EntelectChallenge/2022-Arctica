import logging
import uuid

from .helpers import Helpers
from .hub.auth_hub_connection import AuthHubConnection
from .hub.base_hub_connection import BaseHubConnection
from .hub.reconnection import (IntervalReconnectionHandler,
                               RawReconnectionHandler, ReconnectionType)
from .protocol.json import JsonHubProtocol
from .subject import Subject


class HubConnectionError(ValueError):
    pass


class HubConnectionBuilder(object):
    """
    Hub connection class, manages handshake and messaging

    Args:
        hub_url: SignalR core url

    Raises:
        HubConnectionError: Raises an Exception if url is empty or None
    """
    def __init__(self):
        self.hub_url = None
        self.hub = None
        self.options = {
                "access_token_factory": None
            }
        self.token = None
        self.headers = None
        self.negotiate_headers = None
        self.has_auth_configured = None
        self.use_messagepack_protocol = None
        self.protocol = None
        self.reconnection_handler = None
        self.keep_alive_interval = None
        self.verify_ssl = True
        self.skip_negotiation = False # By default do not skip negotiation

    def with_url(
            self,
            hub_url,
            options=None):
        if hub_url is None or hub_url.strip() == "":
            raise HubConnectionError("hub_url must be a valid url.")

        if options is not None and type(options) != dict:
            raise HubConnectionError(
                "options must be a dict {0}.".format(self.options))

        if options is not None \
                and "access_token_factory" in options.keys()\
                and not callable(options["access_token_factory"]):
            raise HubConnectionError(
                "access_token_factory must be a function without params")

        if options is not None:

            self.has_auth_configured = \
                "access_token_factory" in options.keys()\
                and callable(options["access_token_factory"])

            self.skip_negotiation = "skip_negotiation" in options.keys() and options["skip_negotiation"]

        self.hub_url = hub_url
        self.hub = None
        self.options = self.options if options is None else options
        return self

    def configure_logging(self, logging_level, handler=None):
        """
        Confiures signalr logging
        :param handler:  custom logging handler
        :param socket_trace: Enables socket package trace
        :param logging_level: logging.INFO | logging.DEBUG ... from python logging class
        :param log_format: python logging class format by default %(asctime)-15s %(clientip)s %(user)-8s %(message)s
        :return: instance hub with logging configured
        """
        Helpers.configure_logger(logging_level, handler)
        return self

    def build(self):
        """"
        self.token = token
        self.headers = headers
        self.negotiate_headers = negotiate_headers
        self.has_auth_configured = token is not None

        """
        if self.protocol is None:
            self.protocol = JsonHubProtocol()  

        self.headers = {}

        if "headers" in self.options.keys() and type(self.options["headers"]) is dict:
            self.headers = self.options["headers"]

        if self.has_auth_configured:
            auth_function = self.options["access_token_factory"]
            if auth_function is None or not callable(auth_function):
                raise HubConnectionError(
                    "access_token_factory is not function")
        if "verify_ssl" in self.options.keys() and type(self.options["verify_ssl"]) is bool:
            self.verify_ssl = self.options["verify_ssl"]

        self.hub = AuthHubConnection(
            self.hub_url,
            self.protocol,
            auth_function,
            keep_alive_interval=self.keep_alive_interval,
            reconnection_handler=self.reconnection_handler,
            headers=self.headers,
            verify_ssl=self.verify_ssl,
            skip_negotiation=self.skip_negotiation)\
            if self.has_auth_configured else\
            BaseHubConnection(
                self.hub_url,
                self.protocol,
                keep_alive_interval=self.keep_alive_interval,
                reconnection_handler=self.reconnection_handler,
                headers=self.headers,
                verify_ssl=self.verify_ssl,
                skip_negotiation=self.skip_negotiation)

        return self.hub

    def with_hub_protocol(self, protocol):
        self.protocol = protocol
        return self

    def with_automatic_reconnect(self, data):
        """
        https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-core-3-0-preview-4/
        :param data:
        :return:
        """
        reconnect_type = data["type"] if "type" in data.keys() else "raw"

        max_attempts = data["max_attempts"] if "max_attempts" in data.keys() else None # Infinite reconnect

        reconnect_interval = data["reconnect_interval"]\
            if "reconnect_interval" in data.keys() else 5 # 5 sec interval
        
        keep_alive_interval = data["keep_alive_interval"]\
            if "keep_alive_interval" in data.keys() else 15

        intervals = data["intervals"]\
            if "intervals" in data.keys() else []  # Reconnection intervals

        self.keep_alive_interval = keep_alive_interval

        reconnection_type = ReconnectionType[reconnect_type]

        if reconnection_type == ReconnectionType.raw:
            self.reconnection_handler = RawReconnectionHandler(
                reconnect_interval,
                max_attempts
            )
        if reconnection_type == ReconnectionType.interval:
            self.reconnection_handler = IntervalReconnectionHandler(
                intervals
            )
        return self
