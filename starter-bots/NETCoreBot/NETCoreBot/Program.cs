using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using NETCoreBot.Enums;
using NETCoreBot.Models;
using NETCoreBot.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Domain.Models;

namespace NETCoreBot
{
    public class Program
    {
        public static IConfigurationRoot Configuration;

        private static async Task Main(string[] args)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);

            Configuration = builder.Build();
            var registrationToken = Environment.GetEnvironmentVariable("Token");
            var environmentIp = Environment.GetEnvironmentVariable("RUNNER_IPV4");
            var ip = !string.IsNullOrWhiteSpace(environmentIp) ? environmentIp : Configuration.GetSection("RunnerIP").Value;
            ip = ip.StartsWith("http://") ? ip : "http://" + ip;

            var port = Configuration.GetSection("RunnerPort");

            var url = ip + ":" + port.Value + "/runnerhub";

            var connection = new HubConnectionBuilder()
                                .WithUrl($"{url}")
                                .ConfigureLogging(logging =>
                                {
                                    logging.SetMinimumLevel(LogLevel.Debug);
                                })
                                .WithAutomaticReconnect()
                                .Build();

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
                        
                        connection.On<EngineConfigDto>(
                            "ReceiveConfigValues",
                            (engineConfigDto) =>
                            {
                                Console.WriteLine("engineConfigDto hit");
                                
                                botService.SetEngineConfigDto(engineConfigDto);
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
    }
}
