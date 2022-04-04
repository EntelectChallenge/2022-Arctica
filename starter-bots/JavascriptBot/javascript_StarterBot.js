const {start, registerBot, registerPlayerCommandMethod} = require("./Utils/signalRSetup");
const {BotService} = require("./Services/BotService");

const botService = new BotService();

registerBot(botService.getBot());
registerPlayerCommandMethod(state => botService.computeNextPlayerCommand(state));

start();
