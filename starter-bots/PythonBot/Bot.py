from Action import Action
from PlayerCommand import PlayerCommand

import json

class Bot():
    def __init__(self) -> None:
        self.my_nodes = []
        self.my_scout_towers = []
        self.actions = []
        self.state = {}
        self.bot_id = 0

    def __str__(self) -> str:
        return "Bot Id:" + self.bot_id

    def set_bot_id(self, arg_id) -> None:
        self.bot_id = arg_id

    def set_bot_state(self, bot_state) -> None:
        self.state = bot_state
        self.my_scout_towers = bot_state.world.map.scoutTowers
        self.my_nodes = bot_state.world.map.nodes

    def __add_action(self, action) -> None:
        self.actions.append(action)

    def scout(self, units, node) -> None:
        self.__add_action(Action.scout_action(units, node))

    def farm(self, units, node) -> None:
        self.__add_action(Action.farm_action(units, node))

    def mine(self, units, node) -> None:
        self.__add_action(Action.mine_action(units, node))
    
    def lumber(self, units, node) -> None:
        self.__add_action(Action.lumber_action(units, node))

    def start_campfire(self, units, node) -> None:
        self.__add_action(Action.start_campfire_action(units, node))

    def get_command(self) -> PlayerCommand:
        return PlayerCommand(self.bot_id, self.actions)

    def toJson(self) -> str:
        return json.dumps(self, default=lambda o: o.__dict__)

    def __repr__(self) -> str:
        return self.toJson()