# -*- coding: utf-8 -*-
"""
Entelect StarterBot 2022 for Python3
"""
import logging
import os
import time
import uuid
from dotmap import DotMap

import asyncio
from signalrcore_async.hub_connection_builder import HubConnectionBuilder

from BotService import BotService
from Bot import Bot

logging.basicConfig(filename='sample_python_bot.log', filemode='w', level=logging.DEBUG)
logger = logging.getLogger(__name__)

botService = BotService()
hub_connected = False
hub_connection = None

def on_register(args) -> None:
    bot = Bot()
    bot.set_bot_id(args[0])
    botService.set_bot(bot)
    set_hub_connection(True)
    print("Registered bot with Runner")

def on_disconnect() -> None:
    #Clients should disconnect from the server when the server sends the request to do so.
    global hub_connection
    print("Disconnected:")    
    hub_connection.StopAsync()
    hub_connection.DisposeAsync()
    set_hub_connection(False)

def print_message(x) -> None:
    print(x)


def set_hub_connection(connected) -> None:
    global hub_connected
    hub_connected = connected

"""
private static async Task Main(string[] args)
    {

        





        var botService = new BotService();

        await connection.StartAsync()
            .ContinueWith(
                task =>
                {
                    Console.WriteLine("Connected to Runner");
                    /* Clients should disconnect from the server when the server sends the request to do so. */
                    connection.On<Guid>(
                        "Disconnect",
                        (id) =>
                        {
                            Console.WriteLine("Disconnected:");

                            connection.StopAsync();
                            connection.DisposeAsync();
                        });
                    connection.On<Guid>(
                        "Registered",
                        (id) =>
                        {
                            Console.WriteLine("Registered Bot with the runner");
                            botService.SetBot(
                                new BotDto()
                                {
                                    Id = id
                                });
                        });

                    /* Get the current WorldState along with the last known state of the current client. */
                    connection.On<GameStateDto>(
                        "ReceiveBotState",
                        (gameStateDto) =>
                        {
                            Console.WriteLine("GameStateDTO hit");
                            var gameState = new GameState { World = null, Bots = new List<BotDto>() };
                            gameState.World = gameStateDto.World;
                            gameState.Bots = gameStateDto.Bots;

                            botService.SetGameState(gameState);
                        });

                    var token = Environment.GetEnvironmentVariable("REGISTRATION_TOKEN");
                    token = !string.IsNullOrWhiteSpace(token) ? token : Guid.NewGuid().ToString();

                    Thread.Sleep(1000);
                    Console.WriteLine("Registering with the runner...");
                    connection.SendAsync("Register", token, "NetNickName");

                    while (connection.State == HubConnectionState.Connected)
                    {
                        Thread.Sleep(30);
                        Console.WriteLine($"ConState: {connection.State}");
                        Console.WriteLine($"Bot: {botService.GetBot()?.Id.ToString()}");

                        var bot = botService.GetBot();
                        if (bot == null)
                        {
                            continue;
                        }

                        if (botService.GetGameState().World != null)
                        {
                            botService.ComputeNextPlayerAction(botService.GetPlayerCommand());
                            connection.InvokeAsync("SendPlayerCommand", botService.GetPlayerCommand());
                        }
                    }
                });
    }

"""
async def run_bot() -> None:
    #get registrationToken
    token = os.getenv("REGISTRATION_TOKEN")
    token = token if token is not None else uuid.uuid4()
    
    #get environmentIp
    environmentIp = os.getenv('RUNNER_IPV4', "http://localhost") #default value hardcoded, not from appsettings file
    environmentIp = environmentIp if environmentIp.startswith("http://") else "http://" + environmentIp
    
    port = '5000' #default value hardcoded, not from appsettings file

    url = environmentIp + ":" + port + "/runnerhub"

    print(url)
    global hub_connection
    hub_connection = HubConnectionBuilder() \
        .with_url(url) \
        .configure_logging(logging.DEBUG) \
        .with_automatic_reconnect({
        "type": "raw",
        "keep_alive_interval": 10,
        "reconnect_interval": 5,
        "max_attempts": 5
    }).build()
        
    try:
        
        await hub_connection.start()  
        
        hub_connection.on("Disconnect", on_disconnect)
        hub_connection.on("Registered", on_register)
        hub_connection.on("ReceiveBotState", get_next_player_action)
        
        time.sleep(1)
        print("Registering with the runner...")
        
        bot_nickname = "<example_bot_name>"
        await hub_connection.invoke("Register", [str(token), bot_nickname])
        
        time.sleep(5)
        while hub_connected:
            time.sleep(1)
            continue
    
    except Exception as e:
        print(e)
    
    finally:
        
        await hub_connection.stop()
    
    
        
    #I am here

    # hub_connection.on_open(lambda: (print("Connection opened and handshake received, ready to send messages"),
    #                                 set_hub_connection(True)))
    # hub_connection.on_error(lambda data: print(f"An exception was thrown closed: {data.error}"))
    # hub_connection.on_close(lambda: (print("Connection closed"),
    #                                  set_hub_connection(False)))

    # hub_connection.on("Registered", on_register)
    # hub_connection.on("ReceiveBotState", get_next_player_action)
    # hub_connection.on("Disconnect", lambda data: (print("Disconnect Called"),(set_hub_connection(False))))
    # hub_connection.on("ReceiveGameComplete", lambda data: (print("Game complete"), (on_disconnect)))

    
    # time.sleep(1)



    # print("Registering with the runner...")
    
    # registration_args = [str(token), bot_nickname]
    # hub_connection.send("Register", registration_args)

    # time.sleep(5)
    # while hub_connected:
    #     continue
       
    # hub_connection.stop()

def get_next_player_action(args) -> None: 
    #Get the current WorldState along with the last known state of the current client. 
    try:
        bot_state = DotMap(args[0])
        player_command = botService.compute_next_player_command(bot_state)
        hub_connection.send("SendPlayerCommand", [player_command])
        print("Send Action to Runner")
    except Exception as e:
        print(e)


if __name__ == "__main__":
    asyncio.run(run_bot())
