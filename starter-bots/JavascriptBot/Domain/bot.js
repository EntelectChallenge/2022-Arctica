const {scoutAction, farmAction, mineAction, lumberAction} = require('./actions');

class Bot {
    constructor() {
        this.myNodes = [];
        this.myScoutTowers = [];
        this.actions = [];
        this.state = {};
    }

    setBotId(id) {
        this.id = id;
    }

    setState(botState) {
        this.actions = [];
        this.state = botState;
        this.myScoutTowers = botState.world.map.scoutTowers;
        this.myNodes = botState.world.map.nodes;
    }

    // TODO: Check action type against resource type because you cant mine wood?
    #addAction(action) {
        this.actions.push(action);
    }
    scout(units, node) {
        this.#addAction(scoutAction(units, node));
    }
    farm(units, node) {
        this.#addAction(farmAction(units, node));
    }
    mine(units, node) {
        this.#addAction(mineAction(units, node));
    }
    lumber(units, node) {
        this.#addAction(lumberAction(units, node));
    }

    getCommand() {
        return {
            PlayerId: this.id,
            Actions: [...this.actions]
        };
    }
}

module.exports = {Bot};