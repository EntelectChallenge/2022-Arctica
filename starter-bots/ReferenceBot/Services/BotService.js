const {Bot} =  require("../Domain/bot");
const {resourceTypes} =  require("../Domain/enums");
const {NodeService} = require("./NodeService");
const {ActionService} = require("./ActionService");

class BotService {
    constructor() {
        this.bot = new Bot();
        this.nodeService = new NodeService(this.bot);
        this.actionService = new ActionService(this.bot);
    }

    getBot(){
        return this.bot;
    }

    computeNextPlayerCommand(state) {
        try {
            if(!state) {
                return null;
            }
            const bot = this.bot;
            // console.log("Bot ID: " + bot.id);
            bot.setState(state);
            // console.log("BotState received");
            
            if (this.bot.availableUnits <= 0)
                return null;
            
            this.foodManagement();
            this.heatManagement();
            
            if (this.actionService.shouldScout()) {
                const unitsToScout = Math.floor(this.bot.availableUnits/2);
                for (let i = 0; i < unitsToScout; i++) {
                    const nearestScoutTower = this.nodeService.getNearestUnvisitedScoutTower();
                    this.bot.scout(1, nearestScoutTower);
                }
                this.actionService.currentlyScouting = true;
            } else {
                this.calculateHarvestingActions();
            }
            
            return this.bot.getCommand();
        } catch(e) {
            console.log(e);
        }
    }

    foodManagement() {
        const foodConsumptionRate = 1;
        const minimumViableFood = this.bot.population * 2 * foodConsumptionRate;
        const foodConsumptionMargin = this.bot.population * foodConsumptionRate;
        if (this.bot.availableUnits && this.bot.food <= minimumViableFood) {
            const retrievalPerUnit = 5;
            const amountToRetrieve = minimumViableFood + foodConsumptionMargin;
            const workforce = this.actionService.calculateWorkforceRequired(retrievalPerUnit, amountToRetrieve);
            
            const foodNodes = this.nodeService.getNodesByType(resourceTypes.Food).filter(node => node.amount >= 0);
            const closestFoodNode = this.nodeService.getClosest(foodNodes);
            if (!closestFoodNode) {
                return;
            }
            // TODO: add a method that distributes workforce across nodes when the closest one runs out of resources
            this.bot.farm(workforce, closestFoodNode);
        }
    }

    heatManagement() {
        const heatConsumptionRate = 1;
        const minimumViableHeat = this.bot.population * 2 * heatConsumptionRate;
        const heatConsumptionMargin = this.bot.population * heatConsumptionRate;

        if (this.bot.wood > 9 && this.bot.heat < this.bot.population * 2) {
            const retrievalPerUnit = 3;
            const amountToRetrieve = minimumViableHeat + heatConsumptionMargin;
            const workforce = this.actionService.calculateWorkforceRequired(retrievalPerUnit, amountToRetrieve);
            this.bot.startCampfire(workforce);
        }
    }

    calculateHarvestingActions() {
        let nodesByProximity = this.nodeService.getNodesByProximity().filter(node => node.amount >= 0);
        // Get current tier and see limiting resource
        if(this.bot.currentTier < 5){
            const minResources = this.bot.getMinimumResourcesForTier();
            let priorityResource;
            if (this.bot.food < minResources.food) {
                priorityResource = resourceTypes.Food;
            } else if (this.bot.wood < minResources.wood) {
                priorityResource = resourceTypes.Wood;
            } else if (this.bot.stone < minResources.stone) {
                priorityResource = resourceTypes.Stone;
            }

            nodesByProximity = nodesByProximity.filter(node => node.type == priorityResource);
        }
        
        while (this.bot.availableUnits >= 0 && nodesByProximity.length > 0) {
            const closestResourceNode = nodesByProximity.shift();
            const unitAmount = 1 + Math.floor(this.bot.availableUnits/3);
            const action = this.actionService.getActionByResourceType(closestResourceNode.type);
            action(unitAmount, closestResourceNode);
        }
    }
}

module.exports = {BotService};