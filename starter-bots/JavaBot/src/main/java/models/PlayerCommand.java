package models;

import java.util.List;
import java.util.UUID;

public class PlayerCommand {
    private UUID playerID;
    private List<CommandAction> actions;

    public PlayerCommand() {
    }

    public PlayerCommand(UUID playerID, List<CommandAction> actions) {
        this.playerID = playerID;
        this.actions = actions;
    }

    public UUID getPlayerID() {
        return playerID;
    }

    public void setPlayerID(UUID playerID) {
        this.playerID = playerID;
    }

    public List<CommandAction> getActions() {
        return actions;
    }

    public void setActions(List<CommandAction> actions) {
        this.actions = actions;
    }
}
