package arctica.service

import BotDto
import arctica.service.model.CommandAction
import arctica.service.model.PlayerCommand
import arctica.service.model.Position
import arctica.service.model.ScoutTower
import arctica.service.model.dto.GameStateDto
import arctica.service.model.dto.Map
import arctica.service.model.dto.ResourceNode
import arctica.service.model.enums.ActionType
import arctica.service.model.enums.ResourceType
import arctica.util.MathUtils
import java.util.*
import kotlin.random.Random

class BotService {

    private lateinit var gameState: GameStateDto
    private lateinit var botId: UUID
    private lateinit var bot: BotDto
    private lateinit var worldMap: Map
    private lateinit var scoutedTowers: ScoutTower

    private fun updateBotState(gameState: GameStateDto) {
        this.gameState = gameState
        this.botId = gameState.botId
        this.bot = gameState.bots.first { it.id == botId }
        this.worldMap = gameState.world.map
    }


    fun computeNextPlayerAction(): PlayerCommand? {
        /** Location of the nearest Scout tower */

        println("AVAILABLE UNITS ${bot.availableUnits}")
        if (bot.availableUnits < 1) return null


        val command = if (bot.map.nodes.isEmpty()) {

            scoutedTowers = findNearestScoutTower(bot.baseLocation, worldMap.scoutTowers) ?: return null
            listOf(CommandAction(ActionType.SCOUT.value, 1, scoutedTowers.id))

        } else {

            val targetNode = bot.map.nodes.random()
            val node: ResourceNode = gameState.world.map.nodes.find { it.id == targetNode } ?: return null

            val actionType = ActionType.getRandomActionType().value

            val action = when (node.type) {
                ResourceType.WOOD.value -> ActionType.LUMBER
                ResourceType.FOOD.value -> ActionType.FARM
                ResourceType.STONE.value -> ActionType.MINE
                else -> {
                    return null
                }
            }

            println("Sending: ${action.name}")

            listOf(
                CommandAction(
                    actionType,
                    Random.nextInt(
                        bot.availableUnits
                    ),
                    node.id
                )

            )

        }

        return PlayerCommand(botId, command)
    }

    private fun findNearestScoutTower(baseLocation: Position, scoutTowers: List<ScoutTower>): ScoutTower? {
        return scoutTowers.minByOrNull { MathUtils.getDistanceBetween(baseLocation, it.position) }
    }

    companion object {
        fun updateBotService(botService: BotService?, gameState: GameStateDto) {
            botService?.updateBotState(gameState)
        }
    }
}