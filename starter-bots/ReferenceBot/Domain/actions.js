const {actionTypes} = require('./enums');

const emptyId = '00000000-0000-0000-0000-000000000000';

const baseAction = (units, targetNode) => ({
    Units: units,
    Id: targetNode.id
})

const scoutAction = (units, targetNode) => ({
    Type: actionTypes.Scout,
    ...baseAction(units, targetNode)
});

const farmAction = (units, targetNode) => ({
    Type: actionTypes.Farm,
    ...baseAction(units, targetNode)
});

const lumberAction = (units, targetNode) => ({
    Type: actionTypes.Lumber,
    ...baseAction(units, targetNode)
});

const mineAction = (units, targetNode) => ({
    Type: actionTypes.Mine,
    ...baseAction(units, targetNode)
});

const startCampfireAction = (units) => ({
    Type: actionTypes.StartCampfire,
    ...baseAction(units, {id: emptyId})
});

module.exports = {scoutAction, farmAction, lumberAction, mineAction, startCampfireAction};