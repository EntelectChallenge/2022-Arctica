from ActionTypes import ActionTypes
from PlayerAction import PlayerAction

class Action():    
    @staticmethod
    def base_action(units, target_node, action_type) -> PlayerAction:
        return PlayerAction(target_node, units, action_type)

    @staticmethod
    def scout_action(units, target_node) -> PlayerAction:
        action_type = ActionTypes.Scout.value
        return Action.base_action(units, target_node, action_type)

    @staticmethod
    def farm_action(units, target_node) -> PlayerAction:
        action_type = ActionTypes.Farm.value
        return Action.base_action(units, target_node, action_type)

    @staticmethod
    def lumber_action(units, target_node) -> PlayerAction:
        action_type = ActionTypes.Lumber.value
        return Action.base_action(units, target_node, action_type)

    @staticmethod
    def mine_action(units, target_node) -> PlayerAction:
        action_type = ActionTypes.Mine.value
        return Action.base_action(units, target_node, action_type)

    @staticmethod
    def start_campfire_action(units, target_node) -> PlayerAction:
        action_type = ActionTypes.StarCampfire.value
        return Action.base_action(units, target_node, action_type)