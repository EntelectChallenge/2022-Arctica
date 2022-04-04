package arctica.service.model

import java.util.*

data class BotMapState (
    val scoutTowers: List<UUID>,
    val nodes: List<UUID>
)
