package arctica.service.model

import java.util.*

class ScoutTower(
    val nodes: List<UUID>,
    id: UUID,
    gameObjectType: Int,
    position: Position
) : GameObject(id, gameObjectType, position)
