package Services;

import Enums.*;
import Models.*;
import Models.Dtos.*;
import utils.MathUtils;

import java.util.*;

public class BotService {

    BotDto bot = new BotDto();
    GameStateDto gameState = new GameStateDto();

    EngineConfigDto engineConfig = new EngineConfigDto();

    Boolean shouldQuite = false;
    Boolean receivedBotState = false;

    public BotService() {
    }

    public Boolean getReceivedBotState() {
        return receivedBotState;
    }

    public void setReceivedBotState(Boolean receivedBotState) {
        this.receivedBotState = receivedBotState;
    }

    public Boolean getShouldQuite() {
        return shouldQuite;
    }

    public void setShouldQuite(Boolean shouldQuite) {
        this.shouldQuite = shouldQuite;
    }

    public GameStateDto getGameState() {
        return gameState;
    }

    public void setGameState(GameStateDto gameState) {
        this.gameState = gameState;
    }

    public BotDto getBot() {
        return bot;
    }

    public void setBot(BotDto bot) {
        this.bot.id = bot.id;
    }

    public void updateBotState(GameStateDto gameState) {
        this.gameState = gameState;
        this.bot = gameState.getBotByID(bot.getId());
    }

    public EngineConfigDto getEngineConfig() {
        return engineConfig;
    }

    public void setEngineConfig(EngineConfigDto engineConfig) {
        this.engineConfig = engineConfig;
    }

//    private ScoutTower findNearestScoutTower(Position baseLocation,List<ScoutTower> scoutTowers ){
//        if (scoutTowers.isEmpty()) {
//            return null;
//        }
//        double  dist = MathUtils.calculateDistanceBetweenPoints(scoutTowers.get(0).getPosition(), baseLocation);
//        ScoutTower nearestTower = scoutTowers.get(0);
//        for(ScoutTower tower: scoutTowers){
//            double newDist = MathUtils.calculateDistanceBetweenPoints(tower.getPosition(), baseLocation);
//            if(dist>newDist){
//                dist = newDist;
//                nearestTower = tower;
//            }
//        }
//        return nearestTower;
//    } STOUT


    public PlayerCommand computeNextPlayerAction() {


        if (gameState.getWorld().getMap().getScoutTowers() == null) {
            return null;
        }

//        System.out.println("FirstScoutTower " + gameState.getWorld().getMap().getScoutTowers().get(0));

        System.out.println("AVAILABLE UNITS: " + this.bot.getAvailableUnits());

        if (bot.getAvailableUnits() < 1) {
            System.out.println("No available units, not sending a command");
            return null;
        }

        CommandAction command;

        if (this.gameState.getWorld().getMap().getNodes().isEmpty()) {
            ScoutTower scoutTowerToScout = this.gameState.getWorld().getMap().getScoutTowers().get(0);

            command = new CommandAction(ActionTypes.SCOUT.value, 1, scoutTowerToScout.getId());
        } else {
            ResourceNode targetNode = gameState.getWorld().getMap().getNodes().get(0);

            ActionTypes actionType;
            switch (targetNode.getType()) {
                case WOOD: {
                    actionType = ActionTypes.LUMBER;
                    break;
                }
                case FOOD: {
                    actionType = ActionTypes.FARM;
                    break;
                }
                case STONE: {
                    actionType = ActionTypes.MINE;
                    break;
                }
                case HEAT: {
                    actionType = ActionTypes.START_CAMPFIRE;
                    break;
                }
                default: {
                    actionType = ActionTypes.ERROR;
                    break;
                }
            }

            System.out.println("Sending:" + actionType.name());

            command = new CommandAction(actionType.value, MathUtils.randNumGenerator(bot.getAvailableUnits()), targetNode.getId());
        }

        List<CommandAction> commandList = new ArrayList<CommandAction>();
        commandList.add(command);


        PlayerCommand commandToSend = new PlayerCommand(bot.getId(), commandList);
        commandToSend.setPlayerID(this.bot.getId());

        return commandToSend;

    }

}
