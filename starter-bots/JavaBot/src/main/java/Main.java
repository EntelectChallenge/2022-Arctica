
import Models.*;
import Models.Dtos.*;
import Services.*;
import com.microsoft.signalr.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


import java.util.*;

public class Main {

    public static void main(String[] args) throws Exception {
        Logger logger = LoggerFactory.getLogger(Main.class);
        BotService botService = new BotService();
        String token = System.getenv("Token");

        token = (token != null) ? token : UUID.randomUUID().toString();

        String environmentIp = System.getenv("RUNNER_IPV4");

        String ip = (environmentIp != null && !environmentIp.isBlank()) ? environmentIp : "localhost";
        ip = ip.startsWith("http://") ? ip : "http://" + ip;

        String url = ip + ":" + "5000" + "/runnerhub";
        // create the connection
        HubConnection hubConnection = HubConnectionBuilder.create(url)
                .build();

        hubConnection.on("Disconnect", (id) -> {
            System.out.println("Disconnected:");
            botService.setShouldQuite(true);
            hubConnection.stop();
        }, UUID.class);

        hubConnection.on("Registered", (id) -> {
            System.out.println("Registered with the runner, bot ID is: " + id);

            BotDto bot = new BotDto(id);
            botService.setBot(bot);

        }, UUID.class);

        hubConnection.on("ReceiveBotState", (gameStateDto) -> {

            botService.setReceivedBotState(true);
            botService.updateBotState(gameStateDto);
        }, GameStateDto.class);

        hubConnection.on("ReceiveConfigValues", (engineConfig) -> {

            botService.setEngineConfig(engineConfig);
            System.out.println(botService.getEngineConfig().worldLength);
        }, EngineConfigDto.class);


        hubConnection.start().blockingAwait();

        Thread.sleep(1000);
        System.out.println("Registering with the runner...");
        hubConnection.send("Register", token, "Coffee :) Bot");

        hubConnection.on("ReceiveGameComplete", (state) -> {
            System.out.println("Game complete");

        }, String.class);


        //This is a blocking call
        hubConnection.start().subscribe(()-> {

            while (!botService.getShouldQuite()) {
                Thread.sleep(20);

                    if (botService.getReceivedBotState()) {
                       PlayerCommand playerCommand = botService.computeNextPlayerAction();

                       if (hubConnection.getConnectionState() == HubConnectionState.CONNECTED && playerCommand != null) {
                           hubConnection.send("SendPlayerCommand", playerCommand);
                           System.out.println("sending a command");
                       }
                       botService.setReceivedBotState(false);
                     //   System.out.println("Not sending any action for Bot ID: " + botService.getBot().getId());
                    // System.out.println("Map: " + botService.getGameState().getWorld().getMap());
                    }
            }
        });

        hubConnection.stop();

    }
}
