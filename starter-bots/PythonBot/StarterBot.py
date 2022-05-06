# -*- coding: utf-8 -*-
"""
Entelect StarterBot 2022 for Python3
"""
import logging
import os
import time
import uuid
from dotmap import DotMap
from signalrcore.hub_connection_builder import HubConnectionBuilder

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
    print("Registered")

def on_disconnect() -> None:
    print("Disconnected")
    global hub_connection
    hub_connection.close()

def print_message(x) -> None:
    print(x)

def on_receive_config(args) -> None:
    botService.set_engine_config(args)

def set_hub_connection(connected) -> None:
    global hub_connected
    hub_connected = connected

def run_bot() -> None:
    environmentIp = os.getenv('RUNNER_IPV4', "http://localhost")

    environmentIp = environmentIp if environmentIp.startswith("http://") else "http://" + environmentIp

    url = environmentIp + ":" + "5000" + "/runnerhub"

    print(url)
    global hub_connection
    hub_connection = HubConnectionBuilder() \
        .with_url(url) \
        .configure_logging(logging.INFO) \
        .with_automatic_reconnect({
        "type": "raw",
        "keep_alive_interval": 10,
        "reconnect_interval": 5,
        "max_attempts": 5
    }).build()

    hub_connection.on_open(lambda: (print("Connection opened and handshake received, ready to send messages"),
                                    set_hub_connection(True)))
    hub_connection.on_error(lambda data: print(f"An exception was thrown closed: {data.error}"))
    hub_connection.on_close(lambda: (print("Connection closed"),
                                     set_hub_connection(False)))

    hub_connection.on("Registered", on_register)
    hub_connection.on("ReceiveBotState", get_next_player_action)
    hub_connection.on("Disconnect", lambda data: (print("Disconnect Called"),(set_hub_connection(False))))
    hub_connection.on("ReceiveGameComplete", lambda data: (print("Game complete"), (on_disconnect)))
    hub_connection.on("ReceiveConfigValues", on_receive_config)

    hub_connection.start()
    time.sleep(1)

    token = os.getenv("REGISTRATION_TOKEN")
    token = token if token is not None else uuid.uuid4()

    print("Registering with the runner...")
    bot_nickname = "<example_bot_name>"
    registration_args = [str(token), bot_nickname]
    hub_connection.send("Register", registration_args)

    time.sleep(5)
    while hub_connected:
        continue
       
    hub_connection.stop()

def get_next_player_action(args) -> None: 
    try:
        bot_state = DotMap(args[0])
        player_command = botService.compute_next_player_command(bot_state)
        hub_connection.send("SendPlayerCommand", [player_command])
        print("Send Action to Runner")
    except Exception as e:
        print(e)


if __name__ == "__main__":
    run_bot()
