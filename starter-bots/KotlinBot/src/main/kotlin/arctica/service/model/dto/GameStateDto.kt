package arctica.service.model.dto

import BotDto
import arctica.service.model.World
import java.util.*

data class GameStateDto(

    val world: World,
    val bots: List<BotDto>,
    val botId: UUID
)
