package arctica.service.model

import java.util.*

data class CommandAction(
    val type: Int,
    val units: Int,
    val id : UUID
)
