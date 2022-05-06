package arctica.service.model.dto

import arctica.service.model.enums.ActionType
import java.util.UUID

data class PlayerActionDto(

    val targetNodeId: UUID,
    val numberOfUnits: Int,
    val tickActionCompleted: Int,
    val tickActionStart: Int,
    val tickReceived: Int,
    val actionType: ActionType
)