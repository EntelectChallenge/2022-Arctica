package Models.Dtos.EngineConfigClasses;

import Enums.ResourceType;

import java.util.Dictionary;
import java.util.List;

public class ConsumptionResourceConfig extends ResourceConfig {
    public Dictionary<ResourceType, List<Integer>> ResourceConsumption;
}
