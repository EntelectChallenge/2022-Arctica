package arctica.service.model

data class EngineConfigDto
    (
    val runnerUrl: String,
    val runnerPort: String,
    val botCount: Int,
    val maxTicks: Int,
    val scoutWorkTime: Int,
    val tickRate: Int,
    val worldLength: Int, // => RegionSize * NumberOfRegionsInMapLength
    val worldArea: Int, // => WorldLength ^ 2
    val regionSize: Int,
    val processTick: Int,

    val baseZoneSize: Int,
    val numberOfRegionsInMapLength: Int,
    val resourceWorldCoverage: Int,
    val populationDecreaseRatio: Double,
    val worldSeed: Int,
    val consumptionRatio: HashMap<String, Float>,
    val scoreRates: HashMap<String, Int>,
    val resourceScoreMultiplier: ResourceScoreMultiplier,
    val unitConsumptionRatio: UnitConsumptionRatio,
    val populationTiers: List<PopulationTier>,
    val startingFood: Int,

    val startingUnits: Int,

    val resourceGenerationConfig: ResourceGenerationConfig,

    val seeds: Seeds,
    val minimumPopulation: Int,

    val resourceImportance: ResourceImportance,
    val minimumUnits: Int
)

data class ResourceGenerationConfig
    (
    val farm: RenewableResourceConfig,
    val wood: NonRenewableResourceConfig,
    val stone: NonRenewableResourceConfig,

    val campfire: ConsumptionResourceConfig
)

open class ResourceConfig
    (
    val ProximityDistance: Int,
    val DistributionZones: List<String>,
    val QuantityRangePerRegion: List<Int>,

    val RewardRange: List<Int>,

    val workTimeRange: List<Int>,
    val maxUnitsRange: List<Int>
)

class ConsumptionResourceConfig
    (
    val resourceConsumption: HashMap<String, List<Int>>,
    ProximityDistance: Int,
    DistributionZones: List<String>,
    QuantityRangePerRegion: List<Int>,
    RewardRange: List<Int>,
    workTimeRange: List<Int>,
    maxUnitsRange: List<Int>
) : ResourceConfig(
    ProximityDistance, DistributionZones, QuantityRangePerRegion, RewardRange, workTimeRange,
    maxUnitsRange
)


class RenewableResourceConfig
    (
    val regenerationRateRange: RegenerationRateRange,
    val AmountRange: List<Int>,
    ProximityDistance: Int,
    DistributionZones: List<String>,
    QuantityRangePerRegion: List<Int>,
    RewardRange: List<Int>,
    workTimeRange: List<Int>,
    maxUnitsRange: List<Int>
) : ResourceConfig(
    ProximityDistance, DistributionZones, QuantityRangePerRegion, RewardRange, workTimeRange,
    maxUnitsRange
)

class NonRenewableResourceConfig
    (
    val amountRange: List<Int>,
    ProximityDistance: Int,
    DistributionZones: List<String>,
    QuantityRangePerRegion: List<Int>,
    RewardRange: List<Int>,
    workTimeRange: List<Int>,
    maxUnitsRange: List<Int>
) : ResourceConfig(
    ProximityDistance, DistributionZones, QuantityRangePerRegion, RewardRange, workTimeRange,
    maxUnitsRange
)

data class RegenerationRateRange
    (
    val tickRange: List<Int>,
    val amountRange: List<Int>
)

data class Seeds
    (
    val PlayerSeeds: List<Int>,
    val MaxSeed: Int,
    val MinSeed: Int
)

data class ResourceScoreMultiplier
    (
    val population: Int,
    val food: Int,
    val wood: Int,
    val stone: Int,
    val gold: Int
)

data class UnitConsumptionRatio
    (
    val food: Double,
    val wood: Double,
    val stone: Double,
    val gold: Double,
    val heat: Double
)

data class UnitActionDuration
    (
    val Farm: Int,
    val Scout: Int,
    val Lumber: Int,
    val Mine: Int
)

data class UnitActionReward
    (
    val farm: Int,
    val lumber: Int,
    val stone: Int,
    val gold: Int
)

data class ResourceImportance
    (
    val food: Double,
    val heat: Double
)
