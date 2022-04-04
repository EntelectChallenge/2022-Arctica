const {Bot} =  require("../Domain/bot");
const {getRandomInteger} =  require("../Utils");
const {actionTypes} =  require("../Domain/enums");

class BotService {
    constructor() {
        this.bot = new Bot();
    }
    
    getBot(){
        return this.bot;
    }

    addRandomAction(){
        if (this.bot.myNodes.length === 0){
            const randomScoutTowerIndex = getRandomInteger(0, this.bot.myScoutTowers.length);
            const targetScoutTower = this.bot.myScoutTowers[randomScoutTowerIndex];
            console.log('Scouting');
            return this.bot.scout(1, targetScoutTower);
        }

        const unitAmount = getRandomInteger(1, 4);
        const randomTargetNodeIndex = getRandomInteger(0, this.bot.myNodes.length);
        const targetNode = this.bot.myNodes[randomTargetNodeIndex];

        const moveType = getRandomInteger(2, 5)
        switch (moveType){
            case actionTypes.Farm:
                console.log('Farming');
                return this.bot.farm(unitAmount, targetNode);
            case actionTypes.Mine:
                console.log('Mining');
                return this.bot.mine(unitAmount, targetNode);
            case actionTypes.Lumber:
                console.log('Lumbering');
                return this.bot.lumber(unitAmount, targetNode);
            case actionTypes.StartCampfire:
                console.log('Starting Campfire');
                return this.bot.startCampfire(unitAmount);
            default:
                return null;
        }
    }
    
    computeNextPlayerCommand(botState) {
        this.bot.setState(botState);
        const numberOfActions = getRandomInteger(1, 4);
        for (let i = 0; i < numberOfActions; i++) {
            this.addRandomAction();
        }
        return this.bot.getCommand();
    }
}

module.exports = {BotService};
