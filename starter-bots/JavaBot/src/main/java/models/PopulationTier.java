package models;

import java.util.List;

public class PopulationTier {
    private int level;
    private String name;
    private int maxPopulation;
    private List<Double> populationChangeFactorRange;
    private TierResourceConstraints tierResourceConstraints;

    public PopulationTier() {
    }

    public PopulationTier(int level, String name, int maxPopulation, List<Double> populationChangeFactorRange, TierResourceConstraints tierResourceConstraints) {
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
        return "PopulationTier{" +
                "level=" + level +
                ", name='" + name + '\'' +
                ", maxPopulation=" + maxPopulation +
                ", populationChangeFactorRange=" + populationChangeFactorRange +
                ", tierResourceConstraints=" + tierResourceConstraints +
                '}';
    }
}
