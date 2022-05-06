package Models.Dtos;

import Enums.GameObjectType;
import Models.PopulationTier;

import java.math.BigDecimal;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class EngineConfigDto {
    public String runnerUrl;
    public String runnerPort;
    public Integer botCount;
    public Integer maxTicks;
    public Integer scoutWorkTime;
    public Integer tickRate;
    public Integer regionSize = 0;
    public Integer processTick;

    public Integer baseZoneSize;
    public Integer numberOfRegionsInMapLength = 0;
    public Integer worldLength = regionSize * numberOfRegionsInMapLength;
    public Integer worldArea = worldLength ^ 2;
    public Integer resourceWorldCoverage;
    public double populationDecreaseRatio;
    public Integer worldSeed;
    public HashMap<String, BigDecimal> consumptionRatio;//null
    public HashMap<String, Integer> scoreRates;//null
    public Models.Dtos.EngineConfigClasses.ResourceScoreMultiplier resourceScoreMultiplier;
    public Models.Dtos.EngineConfigClasses.UnitConsumptionRatio unitConsumptionRatio;
    public List<PopulationTier> populationTiers;
    public Integer startingFood;

    public Integer startingUnits;

    public Models.Dtos.EngineConfigClasses.ResourceGenerationConfig resourceGenerationConfig;

    public Models.Dtos.EngineConfigClasses.Seeds seeds;
    public Integer minimumPopulation;

    public Models.Dtos.EngineConfigClasses.ResourceImportance resourceImportance;
    public Integer minimumUnits;
}
