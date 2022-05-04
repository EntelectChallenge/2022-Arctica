# -*- coding: utf-8 -*-
"""
Entelect StarterBot 2022 for Python3
"""
import logging
import os
import time
import uuid
from dotmap import DotMap
import json
import traceback

import asyncio
from signalrcore_async.hub_connection_builder import HubConnectionBuilder

from BotService import BotService
from Bot import Bot

logging.basicConfig(filename='sample_python_bot.log', filemode='w', level=logging.DEBUG)
logger = logging.getLogger(__name__)

botService = BotService()
hub_connected = False
hub_connection = None

def on_register(args):
    bot = Bot()
    bot.set_bot_id(args[0])
    botService.set_bot(bot)
    set_hub_connection(True)
    print("Registered bot with Runner")

def on_disconnect():
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

async def run_bot() -> None:
    #get registrationToken
    token = os.getenv("REGISTRATION_TOKEN")
    token = token if token is not None else uuid.uuid4()
    print("Token: ",token)
    #get environmentIp
    environmentIp = os.getenv('RUNNER_IPV4', "http://localhost") #default value hardcoded, not from appsettings file
    environmentIp = environmentIp if environmentIp.startswith("http://") else "http://" + environmentIp
    
    port = '5000' #default value hardcoded, not from appsettings file

    url = environmentIp + ":" + port + "/runnerhub"

    print(url)
    global hub_connection, hub_connected
    hub_connection = HubConnectionBuilder() \
        .with_url(url) \
        .configure_logging(logging.DEBUG) \
        .build()
        
    try:
        
        await hub_connection.start()  
        
        hub_connection.on_open(lambda: (print("Connection opened and handshake received, ready to send messages"),
                                    set_hub_connection(True)))
        # hub_connection.on_error(lambda data: print(f"An exception was thrown closed: {data.error}"))
        hub_connection.on_close(lambda: (print("Connection closed"),
                                     set_hub_connection(False)))

        hub_connection.on("Registered", on_register)
        hub_connection.on("ReceiveBotState", get_next_player_action)
        hub_connection.on("Disconnect", lambda data: (print("Disconnect Called"),(set_hub_connection(False))))
        hub_connection.on("ReceiveGameComplete", lambda data: (print("Game complete"), (on_disconnect)))
        
        time.sleep(1)
        print("Registering with the runner...")
        
        bot_nickname = "<example_bot_name>"
        await hub_connection.send("Register", [str(token),bot_nickname])
        
        time.sleep(5)
        # print("Hub connected", hub_connected)
        while hub_connected:
            # await hub_connection.on("ReceiveBotState", get_next_player_action)
            await asyncio.sleep(0.1)
            
    
    except Exception as e:
        print(e)
        print(traceback.format_exc())
     
    
    finally:
        
        await hub_connection.stop()
    


def get_next_player_action(args): 
    #Get the current WorldState along with the last known state of the current client. 

    try:
        bot_state = DotMap(args[0])
        print("tick:", bot_state.world.currentTick)
        player_command = botService.compute_next_player_command(bot_state)
        # print(type(player_command),player_command)
        hub_connection.send("SendPlayerCommand", [player_command])


        print("Send Action to Runner")
    except Exception as e:
        print(e)
        print(traceback.format_exc())


if __name__ == "__main__":
    asyncio.run(run_bot())
