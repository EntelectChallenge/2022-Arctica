using System;
using System.Collections.Generic;
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

        //TODO: test
        public void PushPlayerAction(Guid botId, CommandAction commandAction)
        {
            var playerAction = GetPlayerActionFromCommand(commandAction);

            var targetBot = worldStateService.GetBotById(botId);
            playerAction.Bot = targetBot;

            if (targetBot == null) return;

            var resourceNode = worldStateService.ResolveNode(playerAction);

            if (resourceNode != null && IsValid(playerAction, resourceNode.Type))
            {
                var travelTime = calculationService.GetTravelTime(resourceNode, targetBot);
                var tickDuration = calculationService.GetWorkTime(resourceNode, playerAction);
                // Todo: does this still work with the resource workTime that has been added?
                playerAction.SetStartAndEndTicks(worldStateService.GetCurrentTick(), travelTime, tickDuration);

                // Logger.LogInfo("ActionService", $"Bot: {botId}, Issued command: {playerAction.ActionType} in tick {worldStateService.GetCurrentTick()}, with {playerAction.NumberOfUnits} units to start at: {playerAction.StartTick}, and to end at: {playerAction.ExpectedCompletedTick}");

                targetBot.AddAction(playerAction, resourceNode);
            }
            else
            {
                Logger.LogInfo("ActionService", "Invalid action added");
            }
        }

        public void HandleCompletedPlayerAction(ResourceNode resourceNode, List<PlayerAction> playerActions,
            ActionType type)
        {
            var handler = actionHandlerResolver.ResolveHandler(type);
            handler.ProcessActionComplete(resourceNode, playerActions);
        }

        private bool IsValid(PlayerAction action, ResourceType resourceType)
        {
            // TODO: Check resource cost for buildings - PHASE 2
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
                ActionType.Mine => resourceType == ResourceType.Stone,
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