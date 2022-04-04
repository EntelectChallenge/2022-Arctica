class CommandAction:
    def __init__(self, action_type, units, target_node) -> None:
        self.type = action_type
        self.units = units
        self.targetNode = target_node