package Models.Dtos;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import Models.PopulationTier;
import Models.World;

public class GameStateDto {
    private World world;
    List<BotDto> bots;
    UUID botID = null;
    List<PopulationTier> populationTiers;

    public GameStateDto() {
        bots = new ArrayList<BotDto>(); 
    }


    public List<PopulationTier> getPopulationTiers() {
        return populationTiers;
    }

    public void setPopulationTiers(List<PopulationTier> populationTiers) {
        this.populationTiers = populationTiers;
    }

    public GameStateDto(World world, UUID botID, List<BotDto> bots, List<PopulationTier> populationTiers) {
        this.world = world;
        this.botID = botID;
        this.bots = bots;
        this.populationTiers = populationTiers;
    }

    public GameStateDto(World world, UUID botID, List<BotDto> bots) {
        this.world = world;
        this.botID = botID;
        this.bots = bots;
    }

    public BotDto getBotByID(UUID id) {
        for (BotDto bot : bots) {
            if (bot.id.equals(id)) {
                return bot;
            }
        }
        return null;
    }

    public World getWorld() {
        return world;
    }

    public void setWorld(World world) {
        this.world = world;
    }

    public UUID getBotID() {
        return botID;
    }

    public void setBotID(UUID botID) {
        this.botID = botID;
    }

    public List<BotDto> getBots() {
        return bots;
    }

    public void setBots(List<BotDto> bots) {
        this.bots = bots;
    }

    @Override
    public String toString() {
        return "GameStateDto [botID=" + botID + ", bots=" + bots + ", populationTiers=" + populationTiers + ", world="
                + world + "]";
    }

}