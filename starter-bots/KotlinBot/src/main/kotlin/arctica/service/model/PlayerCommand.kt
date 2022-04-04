package arctica.service.model

import java.util.*

data class PlayerCommand(
    val playerId: UUID,
    val actions: List<CommandAction>
)
