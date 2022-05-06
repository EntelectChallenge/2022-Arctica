const {start, registerBot, registerPlayerCommandMethod, retrieveEngineConfig} = require("./Utils/signalRSetup");
const {BotService} = require("./Services/BotService");

const botService = new BotService();

registerBot(botService.getBot());
registerPlayerCommandMethod(state => botService.computeNextPlayerCommand(state));
retrieveEngineConfig(botService.getBot());

start();
