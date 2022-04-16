import com.google.gson.Gson;
import com.microsoft.signalr.*;
import models.GameState;
import models.PlayerCommand;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import services.BotService;

import java.util.*;

public class Main {

    // CHANGE THIS 'BOT_NAME' NAME TO YOUR OWN CUSTOM NAME !!!
    private static final String BOT_NAME = "FreeBot";

    // DO NOT CHANGE THESE SETTINGS BELOW !!!
    // -----| BEGIN |-----
    private static final String HUB_DISCONNECTED = "Disconnect";
    private static final String HUB_REGISTERED = "Registered";
    private static final String HUB_REGISTER = "Register";
    private static final String HUB_RECEIVED_GAME_STATE = "ReceiveBotState";
    private static final String HUB_SEND_PLAYER_ACTION = "SendPlayerCommand";
    private static final String HUB_RECEIVE_GAME_COMPLETE = "ReceiveGameComplete";
    private static final int HUB_SLEEP_TIME = 1000;
    private static final int GAME_LOOP_SLEEP_TIME = 30;
    // -----| END |-----

    private static final Gson GSON = new Gson();
    private static final Logger logger = LoggerFactory.getLogger(Main.class);
    private static final BotService botService = new BotService();

    public static void main(String[] args) throws Exception {
        String token = getToken();
        HubConnection hubConnection = buildHubConnection();

        logger.info("Started with version " + 1);

        hubConnection.on(HUB_DISCONNECTED, (id) -> {
            logger.error("Disconnected from SignalR hub.");
            hubConnection.stop().subscribe();
        }, UUID.class);

        hubConnection.on(HUB_REGISTERED, (id) -> {
            logger.info("Registered with the runner. Bot ID " + id);
            botService.setBotId(id);
        }, UUID.class);

        hubConnection.on(HUB_RECEIVED_GAME_STATE, (gameState) -> {
            logger.info("Received Game State: " + GSON.toJson(gameState));
            botService.setGameState(gameState);
        }, GameState.class);

        hubConnection.on(HUB_RECEIVE_GAME_COMPLETE, (data) -> {
            logger.info(data.toString());
        }, Object.class);

        hubConnection.start().blockingAwait();

        Thread.sleep(HUB_SLEEP_TIME);
        logger.info("Registering with the runner.");
        hubConnection.send(HUB_REGISTER, token, BOT_NAME);

        //This is a blocking call
        hubConnection.start().subscribe(() -> {
            while (hubConnection.getConnectionState() == HubConnectionState.CONNECTED) {
                Thread.sleep(GAME_LOOP_SLEEP_TIME);

                if (botService.shouldComputeNext()) {
                    botService.computeNextAction();
                    hubConnection.send(HUB_SEND_PLAYER_ACTION, botService.getPlayerCommand());
                }

            }
        }).dispose();

        hubConnection.stop().subscribe();
    }

    static HubConnection buildHubConnection() {
        String environmentIp = System.getenv("RUNNER_IPV4");
        String ip = (environmentIp != null && !environmentIp.isBlank()) ? environmentIp : "localhost";
        ip = ip.startsWith("http://") ? ip : "http://" + ip;
        String url = ip + ":" + "5000" + "/runnerhub";

        return HubConnectionBuilder.create(url)
                .build();
    }

    static String getToken() {
        String token = System.getenv("Token");
        token = (token != null) ? token : UUID.randomUUID().toString();
        return token;
    }
}
