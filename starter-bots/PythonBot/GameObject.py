class GameObject:
    def __init__(self, guid, game_object_type, position) -> None:
        self.guid = guid
        self.game_object_type = game_object_type
        self.position = position