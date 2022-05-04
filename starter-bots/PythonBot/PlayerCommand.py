import json

class PlayerCommand:
    def __init__(self, player_id, actions) -> None:
        self.playerId = player_id
        self.actions = actions

    def toJson(self):
        com = str(json.dumps(self, default=lambda o: o.__dict__))
        return com

    def __repr__(self):
        return self.toJson()
    
    