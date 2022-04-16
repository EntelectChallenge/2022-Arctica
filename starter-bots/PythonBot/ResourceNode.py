from GameObject import GameObject

class ResourceNode(GameObject):
    def __init__(self, guid, game_object_type, position, resource_type, amount, max_units, current_units, reward, work_time) -> None:
        super().__init__(guid, game_object_type, position)
        self.resource_type = resource_type
        self.amount = amount
        self.max_units = max_units
        self.current_units = current_units
        self.reward = reward
        self.work_time = work_time