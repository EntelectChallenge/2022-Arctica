package arctica.service.model.dto

import arctica.service.model.GameObject
import arctica.service.model.Position
import java.util.*

class ResourceNode(
    val type: Int,
    val amount: Int,
    val maxUnits: Int,
    val currentUnits: Int,
    val reward: Int,
    val workTime: Int,
    val regenerationRate: RegenerationRate,
    id: UUID,
    gameObjectType: Int,
    position: Position

) : GameObject(id, gameObjectType, position)

data class RegenerationRate(
    val ticks: Int,
    val amount: Int
)
