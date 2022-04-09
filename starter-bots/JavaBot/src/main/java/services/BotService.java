package services;

import enums.ActionType;
import models.BotState;
import models.CommandAction;
import models.GameState;
import models.PlayerCommand;

import java.util.UUID;

public class BotService {

    private UUID botId;
    private BotState botState;
    private GameState gameState;
    private final PlayerCommand playerCommand = new PlayerCommand();

    public BotService() {

    }

    public void setBotId(UUID botId) {
        this.botId = botId;
        this.playerCommand.setPlayerID(botId);
    }

    public void setGameState(GameState gameState) {
        this.gameState = gameState;
        this.botState = gameState
                .getBots()
                .stream()
                .filter(bot -> bot.getId().equals(this.botId))
                .findFirst()
                .orElseThrow(() -> new IllegalArgumentException(
                        String.format("Bot is ID: %s not found in bot list", this.botId)
                ));
    }

    public void computeNextAction() {
        if (this.gameState != null) {
            if (this.gameState.getWorld().getMap().getNodes().size() > 0) {
                this.playerCommand.getActions().add(
                        new CommandAction(
                                ActionType.FARM,
                                this.botState.getAvailableUnits(),
                                this.gameState.getWorld().getMap().getNodes().get(0).getId()
                        )
                );
            } else {
                this.playerCommand.getActions().add(
                        new CommandAction(
                                ActionType.SCOUT,
                                1,
                                this.gameState.getWorld().getMap().getScoutTowers().get(0).getId()
                        )
                );
            }
        }

    }

    public PlayerCommand getPlayerCommand() {
        return playerCommand;
    }
}
