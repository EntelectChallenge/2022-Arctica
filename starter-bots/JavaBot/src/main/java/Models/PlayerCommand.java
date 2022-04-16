package Models;

import java.util.List;
import java.util.UUID;


public class PlayerCommand {
    UUID playerID = null;
    List<CommandAction> actions = null;
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
    @Override
    public String toString() {
        return "PlayerCommand [actions=" + actions + ", playerID=" + playerID + "]";
    }
    

}
