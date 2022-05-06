const signalR = require("@microsoft/signalr");
const { createGuid } = require('./index')

const token = process.env["REGISTRATION_TOKEN"] ?? createGuid();

let url = process.env["RUNNER_IPV4"] ?? "http://localhost";
url = url.startsWith("http://") ? url : "http://" + url;
url += ":5000/runnerhub";

const connection = new signalR.HubConnectionBuilder().withUrl(url).configureLogging(signalR.LogLevel.Information).build();

function onDisconnect() {
    console.log("Disconnected");
    connection.stop().then();
}

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
}

function registerBot(bot) {
    connection.on("Registered", (id) => {
        console.log("Registered with the runner with Id: " + id);
        bot.setBotId(id);
    });
}

function registerPlayerCommandMethod(getPlayerCommand) {
    connection.on("ReceiveBotState", botState => {
        const playerCommand = getPlayerCommand(botState);
        connection.invoke("SendPlayerCommand", playerCommand)
    });
}

connection.on("Disconnect", (id) => onDisconnect());

connection.on("ReceiveGameComplete", (winningBot) => {
    console.log("Game complete");
    onDisconnect();
});

function retrieveEngineConfig(bot) {
    connection.on("ReceiveConfigValues", configValues => {
        console.log("Received config values from game engine");
        bot.setBotEngineConfig(configValues);
    });
}

module.exports = { start, registerBot, registerPlayerCommandMethod, retrieveEngineConfig };
