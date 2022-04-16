class GameObjectDto:
    def __init__(self, object_id, current_tier_level, tick, map_object, population, base_location, available_units, seed, wood, food, stone, heat) -> None:
        self.object_id = object_id
        self.current_tier_level = current_tier_level
        self.tick = tick
        self.map = map_object
        self.population = population
        self.base_location = base_location
        self.available_units = available_units
        self.seed = seed
        self.wood = wood
        self.food = food
        self.stone = stone
        self.heat = heat