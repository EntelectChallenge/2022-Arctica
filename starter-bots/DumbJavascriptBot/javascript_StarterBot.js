/*
public enum ActionType
    {
        Error = 0,
        Scout = 1,
        Mine = 2,
        Farm = 3,
        Lumber = 4
    }
*/

// TODO: Check action type against resource type because you cant mine wood?

/*
{
   "id":"f9d494b0-2479-4179-9402-f3d336c5c51a",
   "map":{
      "scoutTowerLocations":[
         
      ],
      "playerBases":[
         
      ],
      "nodes":[
         {
            "type":"Wood",
            "amount":10,
            "id":"00000000-0000-0000-0000-000000000000",
            "gameObjectType":"ResourceNode",
            "position":{
               "x":13,
               "y":21
            }
         },
         {
            "type":"Wood",
            "amount":9,
            "id":"00000000-0000-0000-0000-000000000000",
            "gameObjectType":"ResourceNode",
            "position":{
               "x":15,
               "y":2
            }
         },
         {
            "type":"Wood",
            "amount":8,
            "id":"00000000-0000-0000-0000-000000000000",
            "gameObjectType":"ResourceNode",
            "position":{
               "x":8,
               "y":11
            }
         }
      ]
   },
   "availableUnits":1
}
*/

const signalR = require("@microsoft/signalr");
const token = process.env["REGISTRATION_TOKEN"] ?? createGuid();
let url = process.env["RUNNER_IPV4"] ?? "http://localhost";
url = url.startsWith("http://") ? url : "http://" + url;
url += ":5000/runnerhub";
let _bot = null;
let _playerCommand = null;
let _gameState = null;
let _playerKeys = null;
let _gameObjectKeys = null;

let myScoutTowers = null; // Initially null so first scout tower payload isn't duped later
let myNodes = [];

let connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.assert(connection.state === signalR.HubConnectionState.Connected);
        console.log("Connected to Runner");
        await connection.invoke("Register", token, "JSNickName");
    } catch (err) {
        console.assert(connection.state === signalR.HubConnectionState.Disconnected);
        onDisconnect();
    }
};

connection.on("Disconnect", (id) => onDisconnect());

connection.on("Registered", (id) => {
    console.log("Registered with the runner");
    _bot = { id: id, data: [] };
});

connection.on("ReceiveBotState", gameStateDto => {
    _gameState = gameStateDto;
    console.log("BotState received", JSON.stringify(gameStateDto));

    //Get the ID's of all players
    // _playerKeys = Object.keys(_gameState.playerObjects);

    /* if (!_playerKeys.includes(_bot.id)) {
        console.warn("I am no longer in the game state, and have been consumed");
        onDisconnect();
        return;
    } */

    /* _bot = { id: _bot.id, data: _gameState.playerObjects[_bot.id] };
    _gameObjectKeys = Object.keys(_gameState.gameObjects); */

    const scoutTowerLocations = gameStateDto?.map?.scoutTowerLocations
    console.log("Scout Towers: ", JSON.stringify(scoutTowerLocations));

    if (myScoutTowers === null && scoutTowerLocations != null && scoutTowerLocations.length > 0) {
        // Get initial scout tower locations (all should be sent at the beginning to the bot - we don't find more scout towers)
        myScoutTowers = [];
        myScoutTowers = scoutTowerLocations;
        console.log("My Scout Towers: ", JSON.stringify(myScoutTowers));
    }

    _playerCommand = {
        PlayerId: _bot.id,
        Actions: []
    };

    const resourceNodes = gameStateDto?.map?.nodes;
    console.log("Resource Nodes: ", JSON.stringify(resourceNodes));

    // If nodes are found after scouting, add them and mark them so they aren't duped later
    if (resourceNodes != null && resourceNodes.length > 0) {
        if (myNodes.length === 0) {
            myNodes = resourceNodes.map(node => {
                return {
                    node: node,
                    hasBeenHarvested: false
                }
            })
        } else {
            resourceNodes.map(node => {
                // Check for additional nodes and avoid adding dupes
                if (myNodes.findIndex(myNode => JSON.stringify(myNode.node) === JSON.stringify(node)) < 0) {
                    myNodes.push({
                        node: node,
                        hasBeenHarvested: false
                    })
                }
            })
        }
    }

    console.log("My Resource Nodes: ", JSON.stringify(myNodes));
    const nonHarvestedNodes = myNodes != null && myNodes.length > 0 && myNodes.filter(node => !node.hasBeenHarvested).length > 0;

    // Player manages scouted towers on their side
    // if (myScoutTowers != null && myScoutTowers.length > 0){//} && !nonHarvestedNodes) {
    //     // Then scout
    //     _playerCommand.Actions.push({
    //         Type: 1,
    //         Units: 1,
    //         TargetNode: myScoutTowers.shift() // Remove player scout tower position after sending command
    //     });
    // }
    // else if (nonHarvestedNodes) // If there are resource nodes that haven't been harvested
    // {
    //     const nodesToHarvest = myNodes.filter(node => !node.hasBeenHarvested);
    //     console.log("Nodes to harvest: ", JSON.stringify(nodesToHarvest))
    //     const nodeToHarvest = nodesToHarvest[0].node; // Get first node
    //     console.log("Node to harvest: ", JSON.stringify(nodeToHarvest))
    //     const index = myNodes.findIndex(myNode => myNode.node === nodeToHarvest);

    //     if (index >= 0) {
    //         myNodes[index].hasBeenHarvested = true;
    //     }
        
    //     console.log("Index was: ", index)

    //     _playerCommand.Actions.push({
    //         Type: 4,
    //         Units: 1,
    //         TargetNode: nodeToHarvest.position
    //     });
    // }
    // else {
        // If there are no nodes available and you've scouted, then farm
        _playerCommand.Actions.push({
            Type: 3,
            Units: 1,
            TargetNode: { "x": 0, "y": 0 }
        });
   // }

    console.log("Send Command", JSON.stringify(_playerCommand));
    connection.invoke("SendPlayerCommand", _playerCommand);
});

connection.on("ReceivePlayerConsumed", () => {
    console.log("You died");
});

connection.on("ReceiveGameComplete", (winningBot) => {
    console.log("Game complete");
    onDisconnect();
});

// Start the connection.
start();

function createGuid() {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === "x" ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function onDisconnect() {
    console.log("Disconnected");
    connection.stop().then();
}
