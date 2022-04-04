import json

class PlayerAction:
    def __init__(self, target_node, number_of_units, action_type) -> None:
        self.Id = target_node
        self.units = number_of_units
        self.type = action_type

    def toJson(self) -> str:
        return json.dumps(self, default=lambda o: o.__dict__)

    def __repr__(self) -> str:
        return self.toJson()