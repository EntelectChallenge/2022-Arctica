package Models.Dtos;

import java.util.List;
import java.util.UUID;

import Models.BotMapState;
import Models.Position;

public class BotDto {
    public UUID id;
    int currentTierLevel = 0;
    int tick = 0;
    BotMapState map = null;
    int population = 0;
    Position baseLocation = null;

//    int travellingUnits = 0;
//    int miningUnits = 0;
//    int lumberingUnits = 0;
//    int farmingUnits = 0;
//    int scoutingUnits = 0;
    public List<PlayerActionDto> PendingActions;
    public List<PlayerActionDto> Actions;

    int availableUnits = 0;
    int seeds = 0;
    int wood = 0;
    int food = 0;
    int stone = 0;
    int heat = 0;


    public BotDto(UUID id, int currentTierLevel, int tick, BotMapState map, int population, Position baseLocation, List<PlayerActionDto> pendingActions, List<PlayerActionDto> actions, int availableUnits, int seeds, int wood, int food, int stone, int heat) {
        this.id = id;
        this.currentTierLevel = currentTierLevel;
        this.tick = tick;
        this.map = map;
        this.population = population;
        this.baseLocation = baseLocation;
        PendingActions = pendingActions;
        Actions = actions;
        this.availableUnits = availableUnits;
        this.seeds = seeds;
        this.wood = wood;
        this.food = food;
        this.stone = stone;
        this.heat = heat;
    }
    public BotDto() {

    }
    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public BotDto(UUID id) {
        this.id = id;
    }



    public int getCurrentTierLevel() {
        return currentTierLevel;
    }

    public void setCurrentTierLevel(int currentTierLevel) {
        this.currentTierLevel = currentTierLevel;
    }

    public int getTick() {
        return tick;
    }

    public void setTick(int tick) {
        this.tick = tick;
    }

    public BotMapState getMap() {
        return map;
    }

    public void setMap(BotMapState map) {
        this.map = map;
    }

    public int getPopulation() {
        return population;
    }

    public void setPopulation(int population) {
        this.population = population;
    }

    public Position getBaseLocation() {
        return baseLocation;
    }

    public void setBaseLocation(Position baseLocation) {
        this.baseLocation = baseLocation;
    }

    public int getAvailableUnits() {
        return availableUnits;
    }

    public void setAvailableUnits(int availableUnits) {
        this.availableUnits = availableUnits;
    }

    public int getSeeds() {
        return seeds;
    }

    public void setSeeds(int seeds) {
        this.seeds = seeds;
    }

    public int getWood() {
        return wood;
    }

    public void setWood(int wood) {
        this.wood = wood;
    }

    public int getFood() {
        return food;
    }

    public void setFood(int food) {
        this.food = food;
    }

    public int getStone() {
        return stone;
    }

    public void setStone(int stone) {
        this.stone = stone;
    }

    public int getHeat() {
        return heat;
    }

    public void setHeat(int heat) {
        this.heat = heat;
    }

    public List<PlayerActionDto> getPendingActions() {
        return PendingActions;
    }

    public void setPendingActions(List<PlayerActionDto> pendingActions) {
        PendingActions = pendingActions;
    }

    public List<PlayerActionDto> getActions() {
        return Actions;
    }

    public void setActions(List<PlayerActionDto> actions) {
        Actions = actions;
    }

    @Override
    public String toString() {
        return "BotDto [availableUnits=" + availableUnits + ", basePosition=" + baseLocation + ", currentTierLevel="
                + currentTierLevel + ", food=" + food + ", id=" + id
                + ", map=" + map
                + ", population=" + population + ", seeds=" + seeds + ", stone="
                + stone + ", tick=" + tick + ", wood=" + wood + "]";
    }


}
