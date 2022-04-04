from ResourceNode import ResourceNode

class RenewableResourceNode(ResourceNode):
    def __init__(self, guid, game_object_type, position, resource_type, amount, max_units, current_units, reward, work_time, regeneration_rate) -> None:
        super().__init__(guid, game_object_type, position, resource_type, amount, max_units, current_units, reward, work_time)
        self.regeneration_rate = regeneration_rate


class RegenerationRate():
    def __init__(self, ticks, amount) -> None:
        self.ticks = ticks
        self.amount = amount