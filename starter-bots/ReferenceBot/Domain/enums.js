const actionTypes = Object.freeze({
    Error: 0,
    Scout: 1,
    Mine: 2,
    Farm: 3,
    Lumber: 4,
    StartCampfire: 5
});

const resourceTypes = Object.freeze({
    Error: 0,
    Wood: 1,
    Food: 2,
    Stone: 3,
    Gold: 4
});

module.exports = {actionTypes, resourceTypes};