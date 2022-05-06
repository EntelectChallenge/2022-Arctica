package Models.Dtos.EngineConfigClasses;

import java.util.List;

public class ResourceConfig {
    public Integer ProximityDistance;
    public List<String> DistributionZones;
    public List<Integer> QuantityRangePerRegion;

    public List<Integer> RewardRange;

    public List<Integer> WorkTimeRange;
    public List<Integer> MaxUnitsRange;
}
