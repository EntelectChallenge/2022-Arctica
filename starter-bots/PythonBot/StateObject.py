class StateObject():
    def __init__(self, conneciton_id, client, previous_tick, current_tick) -> None:
        self.connection_id = conneciton_id
        self.client = client
        self.previous_tick = previous_tick
        self.current_tick = current_tick