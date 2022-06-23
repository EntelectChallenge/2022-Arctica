using System;
using System.Collections.Generic;
using Domain.Configs;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Engine.Handlers.Interfaces;
using Engine.Interfaces;
using Engine.Models;

namespace Engine.Services
{
    public class ActionService : IActionService
    {
        private readonly IWorldStateService worldStateService;
        private readonly IActionHandlerResolver actionHandlerResolver;
        private readonly ICalculationService calculationService;
        private readonly EngineConfig engineConfig;

        public ActionService(IWorldStateService worldStateService,
            IActionHandlerResolver actionHandlerResolver,
            ICalculationService calculationService,
            IConfigurationService configurationService
        )
        {
            this.calculationService = calculationService;
            this.worldStateService = worldStateService;
            this.actionHandlerResolver = actionHandlerResolver;
            this.engineConfig = configurationService.Value;
        }

        private PlayerAction GetPlayerActionFromCommand(CommandAction command)
        {
            return new PlayerAction(
                command.Type,
                command.Units,
                command.Id);
        }

        public void PushPlayerAction(Guid botId, CommandAction commandAction)
        {
            var playerAction = GetPlayerActionFromCommand(commandAction);

            var targetBot = worldStateService.GetBotById(botId);
            playerAction.Bot = targetBot;

            if (targetBot == null) return;

            //Figure out if this is a building node or a resource node           
            var node = worldStateService.ResolveNode(playerAction);

            // var resourceNode = worldStateService.ResolveNode(playerAction);

            if (node != null && IsValid(playerAction, node.Type))
            {
                var travelTime = calculationService.GetTravelTime(node, targetBot);

                int workTime = 0;

                switch (node.GameObjectType)
                {
                    case GameObjectType.ResourceNode:
                        workTime = calculationService.GetWorkTime((ResourceNode)node, playerAction);
                        break;
                    case GameObjectType.AvailableNode:
                        workTime = calculationService.GetWorkTime((AvailableNode)node, playerAction);
                        break;
                }

                // Todo: does this still work with the resource workTime that has been added?
                playerAction.SetStartAndEndTicks(worldStateService.GetCurrentTick(), travelTime, workTime);

                // Logger.LogInfo("ActionService", $"Bot: {botId}, Issued command: {playerAction.ActionType} in tick {worldStateService.GetCurrentTick()}, with {playerAction.NumberOfUnits} units to start at: {playerAction.StartTick}, and to end at: {playerAction.ExpectedCompletedTick}");

                targetBot.AddAction(playerAction, node);
            }
            else
            {
                Logger.LogInfo("ActionService", "Invalid action added");
            }
        }


        public void HandleCompletedPlayerAction(Node node, List<PlayerAction> playerActions,
            ActionType type)
        {
            var handler = actionHandlerResolver.ResolveHandler(type);
            handler.ProcessActionComplete(node, playerActions);
        }

        private bool IsValid(PlayerAction action, ResourceType resourceType)
        {
            var validUnitAmount = (action.Bot.AvailableUnits >= action.NumberOfUnits) && action.NumberOfUnits > 0;
            var actionTypeAndResourceTypeMatch = ActionTypeMatchesResourceType(action.ActionType, resourceType);
            var resourceRequirementsMet = AreSufficientResourcesAvailable(action);

            return validUnitAmount && actionTypeAndResourceTypeMatch && resourceRequirementsMet;
        }

        public bool ActionTypeMatchesResourceType(ActionType actionType, ResourceType resourceType)
        {
            return actionType switch
            {
                ActionType.Error => false,
                ActionType.Scout => true,
                ActionType.StartCampfire => true,
                ActionType.Quarry => resourceType == ResourceType.Available,
                // ActionType.anotherBuilding => true,
                // ActionType.anotherBuilding => true,
                // ActionType.anotherBuilding => true,
                // ActionType.anotherBuilding => true,
                ActionType.Mine => ((resourceType == ResourceType.Stone) || (resourceType == ResourceType.Gold)),
                ActionType.Farm => resourceType == ResourceType.Food,
                ActionType.Lumber => resourceType == ResourceType.Wood,
                _ => false
            };
        }

        private bool AreSufficientResourcesAvailable(PlayerAction action)
        {
            if (action.ActionType == ActionType.StartCampfire)
            {
                return action.Bot.Wood >= calculationService.CalculateAmountUsed(action);
            }

            return true;
        }

    }
}