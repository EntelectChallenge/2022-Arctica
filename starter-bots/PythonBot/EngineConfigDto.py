import string
from tokenize import Double
from unicodedata import decimal
from GameObjects import GameObjects
from ResourceTypes import ResourceTypes

class EngineConfigDto:
    def __init__(self) -> None:
        self.RunnerUrl: string
        self.runner_port: string
        self.bot_count: int
        self.max_ticks: int
        self.scout_work_time: int
        self.tick_rate: int
        self.region_size = 0
        self.number_of_regions_in_map_length = 0
        self.world_length = self.region_size * self.number_of_regions_in_map_length
        self.world_area = self.world_length * 2
        self.process_tick: int
        self.base_zone_size: int
        self.resource_world_coverage: int
        self.populations_decrease_rate: float
        self.world_seed: int
        self.consumption_ratio = dict[GameObjects, decimal]
        self.score_rates = dict[GameObjects, int]
        self.resouce_score_multiplier = ResourceScoreMultiplier()
        self.unit_consumption_ratio = UnitConsumptionRatio()
        self.populations_tiers = []
        self.starting_food: int
        self.starting_units: int
        self.resource_generation_config = ResourceGenerationConfig()
        self.seeds = Seeds()
        self.minimum_population: int
        self.resource_importance = ResourceImportance()
        self.minimum_units: int

class ResourceGenerationConfig:
    def __init__(self) -> None:
        self.farm = RenewableResourceConfig()
        self.wood = RenewableResourceConfig()
        self.stone = RenewableResourceConfig()
        self.camp_fire = RenewableResourceConfig()

class ResourceConfig:
    def __init__(self) -> None:
        self.proximity_distance = 0
        self.distribution_zones = []
        self.quantity_range_per_region = []
        self.reward_range = []
        self.work_time_range = []
        self.max_units_range = []

class ConsumptionResourceConfig(ResourceConfig):
    def __init__(self) -> None:
        super().__init__()
        self.resource_consumption = dict[ResourceTypes, int]

class RenewableResourceConfig(ResourceConfig):
    def __init__(self) -> None:
        super().__init__()
        self.amount_range = []

class NonRenewableResourceConfig(ResourceConfig):
    def __init__(self) -> None:
        super().__init__()
        self.amount_range = []

class RegenerationRateRange:
    def __init__(self) -> None:
        self.tick_range = []
        self.amount_range = []

class Seeds:
    def __init__(self) -> None:
        self.player_seeds = []
        self.max_seed = 0
        self.min_seed = 0

class ResourceScoreMultiplier:
    def __init__(self) -> None:
        self.population = 0
        self.food = 0
        self.wood = 0
        self.stone = 0
        self.gold = 0

class UnitConsumptionRatio:
    def __init__(self) -> None:
        self.food = 0
        self.wood = 0
        self.stone = 0
        self.gold = 0
        self.heat = 0

class UnitActionDuration:
    def __init__(self) -> None:
        self.farm = 0
        self.scout = 0
        self.lumber = 0
        self.mine = 0

class UnitActionReward:
    def __init__(self) -> None:
        self.farm = 0
        self.lumber = 0
        self.stone = 0
        self.gold = 0

class ResourceImportance:
    def __init__(self) -> None:
        self.food = 0.0
        self.heat = 0.0