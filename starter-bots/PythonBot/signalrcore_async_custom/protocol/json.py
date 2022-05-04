import json
import logging
from datetime import datetime
from enum import Enum
from json import JSONEncoder

from signalrcore_async.helpers import Helpers

from ..messages import BaseMessage, MessageType
from .base_hub_protocol import BaseHubProtocol


class MyEncoder(JSONEncoder):

    # https://github.com/PyCQA/pylint/issues/414
    def default(self, o):

        if type(o) is MessageType:
            return o.value

        # prepare arguments
        if hasattr(o, "arguments"):

            for i, argument in enumerate(o.arguments):

                # date/time
                if isinstance(argument, datetime):
                    o.arguments[i] = o.arguments[i].isoformat()

                # message type
                elif isinstance(argument, Enum):
                    o.arguments[i] = o.arguments[i].value

        # prepare message
        result = o.__dict__

        if "invocation_id" in result:
            result["invocationId"] = result["invocation_id"]
            del result["invocation_id"]

        if "stream_ids" in result:
            result["streamIds"] = result["stream_ids"]
            del result["stream_ids"]

        return result

class JsonHubProtocol(BaseHubProtocol):

    def __init__(self):
        super(JsonHubProtocol, self).__init__("json", 1, "Text", chr(0x1E))
        self.encoder = MyEncoder()

    def parse_messages(self, raw):
        Helpers.get_logger().debug("Raw message incoming")
        # Helpers.get_logger().debug(raw)

        raw_messages = [
            record.replace(self.record_separator, "")
            for record in raw.split(self.record_separator)
            if record is not None and record != "" and record is not self.record_separator
            ]

        result = []

        for raw_message in raw_messages:
            dict_message = json.loads(raw_message)
            result.append(self.get_message(dict_message))
            
        return result

    def encode(self, message):
        Helpers.get_logger().debug(self.encoder.encode(message) + self.record_separator)
        return self.encoder.encode(message) + self.record_separator
