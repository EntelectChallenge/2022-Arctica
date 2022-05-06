const {scoutAction, farmAction, mineAction, lumberAction, startCampfireAction} = require('./actions');

class Bot {
    constructor() {
        this.configValues= {}
        
        this.myNodes = [];
        this.myScoutTowers = [];
        this.actions = [];
        this.state = {};
        this.baseLocation = null;
        this.food = 0;
        this.stone = 0;
        this.wood = 0;
        this.population = 0;
        this.availableUnits = 0;
        this.travellingUnits = 0;
        this.scoutingUnits = 0;
        this.farmingUnits = 0;
        this.lumberingUnits = 0;
        this.miningUnits = 0;
    }

    setBotId = (id) => {
        this.id = id;
    }

    setState = (botState) => {
        // console.log("================================================================================================");
        // console.log(JSON.stringify(botState, null, 4));        
        // console.log("================================================================================================");

        this.actions = [];
        const botDto = botState.bots.find(bot => bot.id === this.id);
        
        this.state = botState;
        this.myScoutTowers = botState.world.map.scoutTowers.filter(scoutTower => !botDto.map.scoutTowers.includes(scoutTower.id));
        this.myNodes = botState.world.map.nodes;
        this.currentTick = botState.world.currentTick;
        this.currentTier = botDto.currentTierLevel;
        this.baseLocation = botDto.baseLocation;
        this.availableUnits = botDto.availableUnits;
        this.travellingUnits = botDto.travellingUnits;
        this.scoutingUnits = botDto.scoutingUnits;
        this.farmingUnits = botDto.farmingUnits;
        this.lumberingUnits = botDto.lumberingUnits;
        this.miningUnits = botDto.miningUnits;
        this.population = botDto.population;
        this.food = botDto.food;
        this.stone = botDto.stone;
        this.wood = botDto.wood;
        this.heat = botDto.heat;
    }
    
    setConfigValues = (configValues) => {
        this.configValues = configValues;
    }

    // TODO: Check action type against resource type because you cant mine wood?
    #addAction = (action) => {
        this.actions.push(action);
        this.availableUnits -= action.Units;
    }
    scout = (units, node) => {
        console.log('Scouting [' + this.currentTick + ']');
        this.#addAction(scoutAction(units, node));
    }
    farm = (units, node) => {
        console.log('Farming [' + this.currentTick + ']');
        this.#addAction(farmAction(units, node));
    }
    mine = (units, node) => {
        console.log('Mining [' + this.currentTick + ']');
        this.#addAction(mineAction(units, node));
    }
    lumber = (units, node) => {
        console.log('Lumbering [' + this.currentTick + ']');
        this.#addAction(lumberAction(units, node));
    }
    startCampfire = (units) => {
        console.log('Starting Campfire[' + this.currentTick + ']');
        this.#addAction(startCampfireAction(units));
    }

    getCommand = () =>{
        return {
            PlayerId: this.id,
            Actions: [...this.actions]
        };
    }

    getMinimumResourcesForTier = () => {
        if(this.currentTier == 0){
            return {
                wood: 100,
                stone: 35,
                food: 20,
            }
        } else if(this.currentTier == 1){
            return {
                wood: 500,
                stone: 2500,
                food: 200,
            }
        } else if(this.currentTier == 2){
            return {
                wood: 5000,
                stone: 25000,
                food: 20000,
            }
        } else if(this.currentTier == 3){
            return {
                wood: 50000,
                stone: 250000,
                food: 20000,
            }
        } else if(this.currentTier == 4){
            return {
                wood: 500000,
                stone: 2500000,
                food: 200000,
            }
        } else {
            return {
                wood: Infinity,
                stone: Infinity,
                food: Infinity
            }
        }
    }
}

module.exports = {Bot};