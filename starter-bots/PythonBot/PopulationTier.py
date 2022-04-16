class PopulationTier:
	def __init__(self, level, name, max_population, population_change_factor_range, tier_resource_constraints) -> None:
		self.level = level
		self.name = name
		self.maxPopulation = max_population
		self.populationChangeFactorRange = population_change_factor_range
		self.tierResourceConstraints = tier_resource_constraints