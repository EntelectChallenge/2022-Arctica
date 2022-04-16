package Models.Dtos;

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
    int travellingUnits = 0;
    int miningUnits = 0;
    int lumberingUnits = 0;
    int farmingUnits = 0;
    int scoutingUnits = 0;
    int availableUnits = 0;
    int seeds = 0;
    int wood = 0;
    int food = 0;
    int stone = 0;
    int heat = 0;


    public BotDto(UUID id, int currentTierLevel, int tick, BotMapState map, int population, Position basePosition,
                  int travellingUnits, int miningUnits, int lumberingUnits, int farmingUnits, int scoutingUnits,
                  int availableUnits, int seeds, int wood, int food, int stone) {
        this.id = id;
        this.currentTierLevel = currentTierLevel;
        this.tick = tick;
        this.map = map;
        this.population = population;
        this.baseLocation = basePosition;
        this.travellingUnits = travellingUnits;
        this.miningUnits = miningUnits;
        this.lumberingUnits = lumberingUnits;
        this.farmingUnits = farmingUnits;
        this.scoutingUnits = scoutingUnits;
        this.availableUnits = availableUnits;
        this.seeds = seeds;
        this.wood = wood;
        this.food = food;
        this.stone = stone;
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

    public BotDto() {

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

    public int getTravellingUnits() {
        return travellingUnits;
    }

    public void setTravellingUnits(int travellingUnits) {
        this.travellingUnits = travellingUnits;
    }

    public int getMiningUnits() {
        return miningUnits;
    }

    public void setMiningUnits(int miningUnits) {
        this.miningUnits = miningUnits;
    }

    public int getLumberingUnits() {
        return lumberingUnits;
    }

    public void setLumberingUnits(int lumberingUnits) {
        this.lumberingUnits = lumberingUnits;
    }

    public int getFarmingUnits() {
        return farmingUnits;
    }

    public void setFarmingUnits(int farmingUnits) {
        this.farmingUnits = farmingUnits;
    }

    public int getScoutingUnits() {
        return scoutingUnits;
    }

    public void setScoutingUnits(int scoutingUnits) {
        this.scoutingUnits = scoutingUnits;
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

    @Override
    public String toString() {
        return "BotDto [availableUnits=" + availableUnits + ", basePosition=" + baseLocation + ", currentTierLevel="
                + currentTierLevel + ", farmingUnits=" + farmingUnits + ", food=" + food + ", id=" + id
                + ", lumberingUnits=" + lumberingUnits + ", map=" + map + ", miningUnits=" + miningUnits
                + ", population=" + population + ", scoutingUnits=" + scoutingUnits + ", seeds=" + seeds + ", stone="
                + stone + ", tick=" + tick + ", travellingUnits=" + travellingUnits + ", wood=" + wood + "]";
    }


}
