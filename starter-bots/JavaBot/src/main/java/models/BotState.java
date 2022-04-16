package models;

import java.util.UUID;

public class BotState {
    private UUID id;
    private int currentTierLevel;
    private int tick;
    private BotMapState map;
    private int population;
    private Position baseLocation;
    private int travellingUnits;
    private int miningUnits;
    private int lumberingUnits;
    private int farmingUnits;
    private int scoutingUnits;
    private int availableUnits;
    private int seeds;
    private int wood;
    private int food;
    private int stone;
    private int heat;

    public BotState() {
    }

    public BotState(UUID id) {
        this.id = id;
    }

    public BotState(UUID id, int currentTierLevel, int tick, BotMapState map, int population, Position baseLocation, int travellingUnits, int miningUnits, int lumberingUnits, int farmingUnits, int scoutingUnits, int availableUnits, int seeds, int wood, int food, int stone, int heat) {
        this.id = id;
        this.currentTierLevel = currentTierLevel;
        this.tick = tick;
        this.map = map;
        this.population = population;
        this.baseLocation = baseLocation;
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
        this.heat = heat;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
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

}
