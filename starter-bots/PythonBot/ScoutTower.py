from GameObject import GameObject

class ScoutTower(GameObject):
    def __init__(self, guid, game_object_type, position, nodes) -> None:
        super().__init__(guid, game_object_type, position)
        self.nodes = nodes