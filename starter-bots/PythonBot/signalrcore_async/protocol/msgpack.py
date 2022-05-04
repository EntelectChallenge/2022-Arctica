from datetime import date, datetime, timezone
from enum import Enum

import msgpack
from msgpack.ext import Timestamp

from ..messages import *
from .base_hub_protocol import BaseHubProtocol


class MessagePackHubProtocol(BaseHubProtocol):

    _priority = [
        "type",
        "headers",
        "invocation_id",
        "target",
        "arguments",
        "item",
        "result_kind",
        "result",
        "stream_ids"
    ]

    def __init__(self):
        super(MessagePackHubProtocol, self).__init__(
            "messagepack", 1, "Text", chr(0x1E))

    def parse_messages(self, raw):
        offset = 0
        messages = []
        total_length = len(raw)

        while True:
            (length, length_size) = self._from_varint(raw, offset)
            offset += length_size
            sliced_data = raw[offset:offset + length]
            offset += length

            message = self._decode_message(msgpack.unpackb(sliced_data))
            messages.append(message)

            if (offset >= total_length):
                break

        return messages

    def encode(self, message):
        encoded_message = msgpack.packb(message, default=self._encode_message)
        varint_length = self._to_varint(len(encoded_message))
        return varint_length + encoded_message

    def _encode_message(self, message):

        # prepare arguments
        if hasattr(message, "arguments"):

            for i, argument in enumerate(message.arguments):

                # date
                if type(argument) is date:
                    argument = datetime(argument.year, argument.month, argument.day, tzinfo=timezone.utc)

                # date/time
                if isinstance(argument, datetime):
                    date_time = argument
                    timestamp = date_time.timestamp()
                    seconds = int(timestamp)
                    nanoseconds = int((timestamp - int(timestamp)) * 1e9)
                    message.arguments[i] = Timestamp(seconds, nanoseconds)

                # message type
                elif isinstance(argument, Enum):
                    message.arguments[i] = argument.name

        result = []

        # sort attributes
        for attribute in self._priority:
            if hasattr(message, attribute):
                if (attribute == "type"):
                    result.append(getattr(message, attribute).value)    
                else:
                    result.append(getattr(message, attribute))

        return result

    def _decode_message(self, raw):

        # [1, Headers, InvocationId, Target, [Arguments], [StreamIds]]
        # [2, Headers, InvocationId, Item]
        # [3, Headers, InvocationId, ResultKind, Result]
        # [4, Headers, InvocationId, Target, [Arguments], [StreamIds]]
        # [5, Headers, InvocationId]
        # [6]
        # [7, Error, AllowReconnect?]

        if raw[0] == 1: # InvocationMessage
            if len(raw[5]) > 0:
                return InvocationClientStreamMessage(headers=raw[1], stream_ids=raw[5], target=raw[3], arguments=raw[4])
            else:
                return InvocationMessage(headers=raw[1], invocation_id=raw[2], target=raw[3], arguments=raw[4])

        elif raw[0] == 2: # StreamItemMessage
            return StreamItemMessage(headers=raw[1], invocation_id=raw[2], item=raw[3])

        elif raw[0] == 3: # CompletionMessage
            result_kind = raw[3]

            if result_kind == 1:
                return CompletionMessage(headers=raw[1], invocation_id=raw[2], result=None, error=raw[4])

            elif result_kind == 2:
                return CompletionMessage(headers=raw[1], invocation_id=raw[2], result=None, error=None)

            elif result_kind == 3:
                return CompletionMessage(headers=raw[1], invocation_id=raw[2], result=raw[4], error=None)

            else: 
                raise Exception("Unknown result kind.")

        elif raw[0] == 4: # StreamInvocationMessage
            return StreamInvocationMessage(headers=raw[1], invocation_id=raw[2], target=raw[3], arguments=raw[4]) # stream_id missing?

        elif raw[0] == 5: # CancelInvocationMessage
            return CancelInvocationMessage(headers=raw[1], invocation_id=raw[2])

        elif raw[0] == 6: # PingMessageEncoding
            return PingMessage()

        elif raw[0] == 7: # CloseMessageEncoding
            return CloseMessage(error=raw[1]) # AllowReconnect is missing

        raise Exception("Unknown message type.")

    def _from_varint(self, buffer, offset):
        shift = 0
        value = 0
        i = offset

        while True:
            byte = buffer[i]
            value |= (byte & 0x7f) << shift
            shift += 7

            if not (byte & 0x80):
                break

            i += 1

        return (value, i + 1)

    def _to_varint(self, value):
        buffer = b''

        while True:

            byte = value & 0x7f
            value >>= 7

            if value:
                buffer += bytes((byte | 0x80, ))
            else:
                buffer += bytes((byte, ))
                break

        return buffer
