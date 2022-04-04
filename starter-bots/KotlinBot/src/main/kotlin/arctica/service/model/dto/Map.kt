package arctica.service.model.dto

import arctica.service.model.ScoutTower

data class Map(
    val scoutTowers: List<ScoutTower>,
    val nodes: List<ResourceNode>
)

