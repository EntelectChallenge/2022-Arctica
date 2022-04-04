from PlayerAction import PlayerAction
from Bot import Bot
from ActionTypes import ActionTypes
import random

class BotService:
    def __init__(self, bot=None, playerAction=None, gameState=None) -> None:
        self.bot = Bot()
        self.playerAction = playerAction
        self.gameState = gameState

    def add_random_action(self):
        if(len(self.bot.my_nodes) == 0):
            random_scout_tower_index = random.randint(0, len(self.bot.my_scout_towers) - 1)
            target_scout_tower = self.bot.my_scout_towers[random_scout_tower_index]
            self.bot.scout(1, target_scout_tower.id)
            return

        unit_amount = random.randint(1, 4)
        random_target_node_index = random.randint(0, len(self.bot.my_nodes))
        target_node = self.bot.my_nodes[random_target_node_index]
        move_type = random.randint(2, 5)
        switcher = {
            ActionTypes.Farm: self.bot.farm(unit_amount, target_node),
            ActionTypes.Mine: self.bot.mine(unit_amount, target_node),
            ActionTypes.Lumber: self.bot.lumber(unit_amount, target_node),
        }

        return switcher.get(move_type, None)

    def compute_next_player_command(self, bot_state):
        self.bot.set_bot_state(bot_state)
        number_of_actions = random.randint(1, 4)
        for i in range(0, number_of_actions):
            self.add_random_action()
        return self.bot.get_command()
        

    # def computeNextPlayerAction(self, id):
    #     self.playerAction = {"PlayerId": id, "Action": PlayerActions.Forward.value, "Heading": random.randint(0, 359)}
    #     return self.playerAction

    def set_bot(self, args):
        self.bot = args

    # def set_game_state(self, args):
    #     self.gameState = args
    #     print("Receive GameState")

    # def set_playerAction_id(self, id):
    #     self.playerAction.PlayerId = id
