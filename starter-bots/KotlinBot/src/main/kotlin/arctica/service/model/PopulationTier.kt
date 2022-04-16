package arctica.service.model

data class PopulationTier(
    val level: Int,
    val name: String,
    val maxPopulation: Int,
    val populationChangeFactorRange: List<Double>,
    val tierResourceConstraints: TierResourceConstraints
)

data class TierResourceConstraints(
    val Food: Int,
    val Wood: Int,
    val Stone: Int
)
