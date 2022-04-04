import json

class PlayerCommand:
    def __init__(self, player_id, actions) -> None:
        self.playerId = player_id
        self.actions = actions

    def toJson(self) -> str:
        return json.dumps(self, default=lambda o: o.__dict__)

    def __repr__(self) -> str:
        return self.toJson()