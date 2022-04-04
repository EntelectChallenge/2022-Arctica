package arctica.service.model

import arctica.service.model.dto.Map

data class World(
    val size: Int,
    val currentTick: Int,
    val map: Map
)