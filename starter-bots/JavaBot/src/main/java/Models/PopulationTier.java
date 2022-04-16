package Models;

import java.util.List;

public class PopulationTier {

    int level =0;
    String name = null;
    int maxPopulation = 0;
    List<Double> populationChangeFactorRange = null;
    TierResourceConstraints tierResourceConstraints = null;
    public PopulationTier(int level, String name, int maxPopulation, List<Double> populationChangeFactorRange,
                          TierResourceConstraints tierResourceConstraints) {
        this.level = level;
        this.name = name;
        this.maxPopulation = maxPopulation;
        this.populationChangeFactorRange = populationChangeFactorRange;
        this.tierResourceConstraints = tierResourceConstraints;
    }
    public int getLevel() {
        return level;
    }
    public void setLevel(int level) {
        this.level = level;
    }
    public String getName() {
        return name;
    }
    public void setName(String name) {
        this.name = name;
    }
    public int getMaxPopulation() {
        return maxPopulation;
    }
    public void setMaxPopulation(int maxPopulation) {
        this.maxPopulation = maxPopulation;
    }
    public List<Double> getPopulationChangeFactorRange() {
        return populationChangeFactorRange;
    }
    public void setPopulationChangeFactorRange(List<Double> populationChangeFactorRange) {
        this.populationChangeFactorRange = populationChangeFactorRange;
    }
    public TierResourceConstraints getTierResourceConstraints() {
        return tierResourceConstraints;
    }
    public void setTierResourceConstraints(TierResourceConstraints tierResourceConstraints) {
        this.tierResourceConstraints = tierResourceConstraints;
    }
    @Override
    public String toString() {
        return "PopulationTier [level=" + level + ", maxPopulation=" + maxPopulation + ", name=" + name
                + ", populationChangeFactorRange=" + populationChangeFactorRange + ", tierResourceConstraints="
                + tierResourceConstraints + "]";
    }
    
     
    
}

class TierResourceConstraints{
    int food;
    int wood;
    int stone;
    public TierResourceConstraints(int food, int wood, int stone) {
        this.food = food;
        this.wood = wood;
        this.stone = stone;
    }
    public int getFood() {
        return food;
    }
    public void setFood(int food) {
        this.food = food;
    }
    public int getWood() {
        return wood;
    }
    public void setWood(int wood) {
        this.wood = wood;
    }
    public int getStone() {
        return stone;
    }
    public void setStone(int stone) {
        this.stone = stone;
    }
    @Override
    public String toString() {
        return "TierResourceConstraints [food=" + food + ", stone=" + stone + ", wood=" + wood + "]";
    }
    
}
