from enum import Enum


class MessageType(Enum):
    invocation = 1
    stream_item = 2
    completion = 3
    stream_invocation = 4
    cancel_invocation = 5
    ping = 6
    close = 7
    invocation_binding_failure = -1

class BaseMessage(object):
    def __init__(self, message_type):
        self.type = MessageType(message_type)

class PingMessage(BaseMessage):
    def __init__(
            self):
        super(PingMessage, self).__init__(6)

class CloseMessage(BaseMessage):
    def __init__(
            self,
            error):
        super(CloseMessage, self).__init__(7)
        self.error = error

class BaseHeadersMessage(BaseMessage):
    def __init__(self, message_type, headers):
        super(BaseHeadersMessage, self).__init__(message_type)
        self.headers = headers

class HandshakeRequestMessage(object):

    def __init__(self, protocol, version):
        self.protocol = protocol
        self.version = version

class HandshakeResponseMessage(object):

    def __init__(self, error):
        self.error = error

class CancelInvocationMessage(BaseHeadersMessage):
    def __init__(
            self,
            headers,
            invocation_id):
        super(CancelInvocationMessage, self).__init__(5, headers)
        self.invocation_id = invocation_id

class CompletionClientStreamMessage(BaseHeadersMessage):
    def __init__(
            self,
            headers,
            invocation_id):
        super(CompletionClientStreamMessage, self).__init__(3, headers)
        self.invocation_id = invocation_id


class CompletionMessage(BaseHeadersMessage):
    def __init__(
            self,
            headers,
            invocation_id,
            result,
            error):
        super(CompletionMessage, self).__init__(3, headers)
        self.invocation_id = invocation_id
        self.result = result
        self.error = error

class InvocationMessage(BaseHeadersMessage):
    def __init__(
            self,
            headers,
            invocation_id,
            target,
            arguments):
        super(InvocationMessage, self).__init__(1, headers)
        self.invocation_id = invocation_id
        self.target = target
        self.arguments = arguments

    def __repr__(self):
        if (self.invocation_id == 0):
            return "InvocationMessage: target {1}, arguments {2}".format(self.invocation_id, self.target, self.arguments)
        else:
            return "InvocationMessage: invocation_id {0}, target {1}, arguments {2}".format(self.invocation_id, self.target, self.arguments)

class InvocationClientStreamMessage(BaseHeadersMessage):
    def __init__(
            self,
            headers,
            stream_ids,
            target,
            arguments):
        super(InvocationClientStreamMessage, self).__init__(1, headers)
        self.target = target
        self.arguments = arguments
        self.stream_ids = stream_ids

    def __repr__(self):
        return "InvocationMessage: stream_ids {0}, target {1}, arguments {2}".format(
            self.stream_ids, self.target, self.arguments)

class StreamInvocationMessage(BaseHeadersMessage):
    def __init__(
            self,
            headers,
            invocation_id,
            target,
            arguments):
        super(StreamInvocationMessage, self).__init__(4, headers)
        self.invocation_id = invocation_id
        self.target = target
        self.arguments = arguments
        self.stream_ids = []

class StreamItemMessage(BaseHeadersMessage):
    def __init__(
            self,
            headers,
            invocation_id,
            item):
        super(StreamItemMessage, self).__init__(2, headers)
        self.invocation_id = invocation_id
        self.item = item
