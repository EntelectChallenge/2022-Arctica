package arctica

import arctica.service.BotService
import arctica.service.model.*
import arctica.service.model.dto.GameStateDto
import com.fasterxml.jackson.databind.ObjectMapper
import com.microsoft.signalr.HubConnectionBuilder
import com.microsoft.signalr.HubConnectionState
import java.util.*
import java.util.concurrent.TimeUnit

object Main {
    @JvmStatic
    fun main(args: Array<String>) {
        val token = System.getenv("REGISTRATION_TOKEN") ?: UUID.randomUUID().toString()
        var moveComputed = true

        HubConnectionBuilder.create(Config.url).build().use { hubConnection ->
            var service: BotService? = null
            var shouldQuit = false

            hubConnection.on("Disconnect", {
                println("Disconnected.")
                shouldQuit = true
            }, UUID::class.java)

            var receiveUpdatedGameState = false

            hubConnection.on("Registered", { id: UUID ->
                service = BotService()
                println("Registered with the runner $id.")
            }, UUID::class.java)

            /**
            === Receive any config value ===
             */

            var worldArea: Int
            var seed: Seeds
            var resourceImportance: ResourceImportance

            hubConnection.on("ReceiveConfigValues", {

                println(it)

                worldArea = it.worldArea
                seed = it.seeds
                resourceImportance = it.resourceImportance

            }, EngineConfigDto::class.java)


            /**
            === Receive the state of the bot ===
             */

            hubConnection.on("ReceiveBotState", {
                BotService.updateBotService(service, it)

                receiveUpdatedGameState = true

            }, GameStateDto::class.java)

            hubConnection.on("ReceiveGameComplete", {
                println("Game complete: $it")
            }, GameComplete::class.java)

            hubConnection.start().blockingAwait()
            println("Connection established with runner.")

            Thread.sleep(1000)
            hubConnection.send("Register", token, Config.BOT_NICKNAME)

            while (!shouldQuit) {
                Thread.sleep(20)
                if (receiveUpdatedGameState) {

                    val action: PlayerCommand? = service?.computeNextPlayerAction()

                    if (hubConnection.connectionState == HubConnectionState.CONNECTED && action != null) {

                        hubConnection.send("SendPlayerCommand", action)
                    }

                    receiveUpdatedGameState = false
                }
            }
            hubConnection.stop().blockingAwait(10, TimeUnit.SECONDS)
            println("Connection closed: ${hubConnection.connectionState}")
        }
    }
}

object Config {

    /** TODO: Change the nickname of your bot here. */
    const val BOT_NICKNAME = "KotlinBot"
    private const val ENVIRONMENT = "RUNNER_IPV4"
    private const val HOSTNAME = "localhost"
    val url = getRunnerUrl()
    private val mapper = ObjectMapper()

    private fun getRunnerUrl(): String {
        var ip = System.getenv(ENVIRONMENT)
        if (ip == null || ip.isBlank()) {
            ip = HOSTNAME
        }
        if (!ip.startsWith("http://")) {
            ip = "http://$ip"
        }
        return "$ip:5000/runnerhub"
    }
}



