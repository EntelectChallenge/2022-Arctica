package models;

import java.util.List;
import java.util.UUID;

public class GameState {
    private World world;
    private List<BotState> bots;
    private UUID botId;
    private List<PopulationTier> populationTiers;

    public GameState() {
    }

    public GameState(World world, List<BotState> bots, UUID botId, List<PopulationTier> populationTiers) {
        this.world = world;
        this.bots = bots;
        this.botId = botId;
        this.populationTiers = populationTiers;
    }

    public World getWorld() {
        return world;
    }

    public void setWorld(World world) {
        this.world = world;
    }

    public List<BotState> getBots() {
        return bots;
    }

    public void setBots(List<BotState> bots) {
        this.bots = bots;
    }

    public UUID getBotId() {
        return botId;
    }

    public void setBotId(UUID botId) {
        this.botId = botId;
    }

    public List<PopulationTier> getPopulationTiers() {
        return populationTiers;
    }

    public void setPopulationTiers(List<PopulationTier> populationTiers) {
        this.populationTiers = populationTiers;
    }

}
