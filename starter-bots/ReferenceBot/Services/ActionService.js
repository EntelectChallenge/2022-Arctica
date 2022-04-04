const {getRandomInteger} = require("../Utils");
const {resourceTypes} =  require("../Domain/enums");

class ActionService {
    constructor(bot) {
        this.bot = bot;
        this.currentlyScouting = false;
    }

    addRandomAction = () => {
        if (this.bot.myNodes.length === 0){
            const randomScoutTowerIndex = getRandomInteger(0, this.bot.myScoutTowers.length);
            const targetScoutTower = this.bot.myScoutTowers[randomScoutTowerIndex];
            this.bot.availableUnits--;
            return this.bot.scout(1, targetScoutTower);
        }

        const unitAmount = getRandomInteger(1, 4);

        this.bot.availableUnits -= unitAmount;
        const resourceType = getRandomInteger(2, 5);

        const applicableResourceNodes = this.bot.myNodes.filter(node => node.type === resourceType);
        const randomTargetNodeIndex = getRandomInteger(0, applicableResourceNodes.length);
        const targetNode = this.bot.myNodes[randomTargetNodeIndex];

        const action = this.getActionByResourceType(resourceType);
        action(unitAmount, targetNode);
    }

    getActionByResourceType = (resourceType) => {
        switch (resourceType){
            case resourceTypes.Food:
                return this.bot.farm;
            case resourceTypes.Stone:
                return this.bot.mine;
            case resourceTypes.Wood:
                return this.bot.lumber;
            default:
                return () => {};
        }
    }

    shouldScout = () => {
        const noKnownResources = this.bot.myNodes.length === 0;
        return !this.currentlyScouting && noKnownResources;
    }

    calculateWorkforceRequired(retrievalPerUnit, amountToRetrieve) {
        const minimumWorkforce = Math.ceil(amountToRetrieve / retrievalPerUnit);
        const availableWorkforce = minimumWorkforce >= this.bot.availableUnits ? this.bot.availableUnits : minimumWorkforce;
        return availableWorkforce;
    }
}

module.exports = {ActionService};