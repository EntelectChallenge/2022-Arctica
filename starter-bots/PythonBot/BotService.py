from Bot import Bot
from ActionTypes import ActionTypes
from EngineConfigDto import EngineConfigDto
import random

class BotService:
    def __init__(self, playerAction=None, gameState=None) -> None:
        self.bot = Bot()
        self.playerAction = playerAction
        self.gameState = gameState
        self.engine_config = EngineConfigDto()

    def add_random_action(self):
        if(len(self.bot.my_nodes) == 0):
            random_scout_tower_index = random.randint(0, len(self.bot.my_scout_towers) - 1)
            target_scout_tower = self.bot.my_scout_towers[random_scout_tower_index]
            self.bot.scout(1, target_scout_tower.id)
            return

        unit_amount = random.randint(1, 4)
        random_target_node_index = random.randint(0, len(self.bot.my_nodes) - 1)
        target_node = self.bot.my_nodes[random_target_node_index]
        move_type = ActionTypes(random.randint(2, 5))

        switcher = {
            ActionTypes.Farm: self.bot.farm,
            ActionTypes.Mine: self.bot.mine,
            ActionTypes.Lumber: self.bot.lumber,
            ActionTypes.StarCampFire: self.bot.start_campfire,
        }

        switcher.get(move_type, None)(unit_amount, target_node.id)

    def compute_next_player_command(self, bot_state):
        self.bot.actions = []
        self.bot.set_bot_state(bot_state)
        number_of_actions = random.randint(1, 4)
        for i in range(0, number_of_actions):
            self.add_random_action()
        return self.bot.get_command()

    def set_bot(self, args):
        self.bot = args

    def set_engine_config(self, configs):
        self.engine_config = configs