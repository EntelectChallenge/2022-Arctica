package Models;

import java.util.List;

public class GameComplete {

    int totalTicks = 0;
    List<PlayerResult> players = null;
    List<Integer> worldSeeds = null;
    GameObject winningBot = null;

    public GameComplete(int totalTicks, List<PlayerResult> players, List<Integer> worldSeeds, GameObject winningBot) {
        this.totalTicks = totalTicks;
        this.players = players;
        this.worldSeeds = worldSeeds;
        this.winningBot = winningBot;
    }

    public int getTotalTicks() {
        return totalTicks;
    }

    public void setTotalTicks(int totalTicks) {
        this.totalTicks = totalTicks;
    }

    public List<PlayerResult> getPlayers() {
        return players;
    }

    public void setPlayers(List<PlayerResult> players) {
        this.players = players;
    }

    public List<Integer> getWorldSeeds() {
        return worldSeeds;
    }

    public void setWorldSeeds(List<Integer> worldSeeds) {
        this.worldSeeds = worldSeeds;
    }

    public GameObject getWinningBot() {
        return winningBot;
    }

    public void setWinningBot(GameObject winningBot) {
        this.winningBot = winningBot;
    }

    @Override
    public String toString() {
        return "GameComplete [players=" + players + ", totalTicks=" + totalTicks + ", winningBot=" + winningBot
                + ", worldSeeds=" + worldSeeds + "]";
    }

}
