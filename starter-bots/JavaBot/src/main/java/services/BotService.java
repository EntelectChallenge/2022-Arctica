package services;

import com.google.gson.Gson;
import enums.ActionType;
import models.BotState;
import models.CommandAction;
import models.GameState;
import models.PlayerCommand;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.UUID;

public class BotService {

    private static final Logger logger = LoggerFactory.getLogger(BotService.class);
    private static final Gson GSON = new Gson();

    private UUID botId;
    private BotState botState;
    private GameState gameState;
    private final PlayerCommand playerCommand = new PlayerCommand();

    public BotService() {

    }

    public boolean shouldComputeNext() {
        if (this.gameState == null) {
            logger.info("Don't compute next.");
            return false;
        }
        return true;
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
        this.playerCommand.getActions().clear();

        if (this.botState.getAvailableUnits() == 0) {
            logger.info("No units available.");
            return;
        }

        if (this.gameState.getWorld().getMap().getNodes().size() > 0) {
            logger.info("Farming action.");
            this.playerCommand.getActions().add(
                    new CommandAction(
                            ActionType.FARM.getValue(),
                            this.botState.getAvailableUnits(),
                            this.gameState.getWorld().getMap().getNodes().get(0).getId()
                    )
            );
        } else {
            logger.info("scouting action");
            this.playerCommand.getActions().add(
                    new CommandAction(
                            ActionType.SCOUT.getValue(),
                            1,
                            this.gameState.getWorld().getMap().getScoutTowers().get(0).getId()
                    )
            );
        }
        logger.info("Done computing.");

    }

    public PlayerCommand getPlayerCommand() {
        logger.info("command: " + GSON.toJson(playerCommand));
        return playerCommand;
    }
}
