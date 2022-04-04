import arctica.service.model.Position
import arctica.service.model.BotMapState
import java.util.*

/**
 *The player's local map
 */

data class BotDto(
    val id: UUID,
    val currentTierLevel: Int,
    val tick: Int,
    val map: BotMapState,
    val population: Int,
    val baseLocation: Position,
    val travellingUnits: Int,
    val lumberingUnits: Int,
    val miningUnits: Int,
    val farmingUnits: Int,
    val scoutingUnits: Int,
    val availableUnits: Int,
    val seed: Int,
    val wood: Int,
    val food: Int,
    val stone: Int
// public int Gold { get; set; }
)

