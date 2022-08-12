using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Engine.Models;

namespace Engine.Interfaces
{
    public interface ICalculationService
    {
        public int CalculateFoodUpkeep(BotObject bot);
        public int CalculateWoodUpkeep(BotObject bot);
        public int CalculateStoneUpkeep(BotObject bot);
        public int CalculateGoldUpkeep(BotObject bot);
        public int CalculateHeatUpkeep(BotObject bot);
        public int GetPopulationChange(BotObject bot);
        public int GetScore(BotObject bot);
        public int GetTravelTime(Node targetNodePosition, BotObject bot);
        public int CalculateAmountExtracted(ResourceNode resourceNode, PlayerAction playerAction);
        public int CalculateTotalAmountExtracted(ResourceNode resourceNode, List<PlayerAction> playerActions);
        public int GetWorkTime(ResourceNode node, PlayerAction playerAction);
        public int GetWorkTime(AvailableNode node, PlayerAction action);
        public PopulationTier GetBotPopulationTier(BotObject bot);
        public double CalculateDistributionFactor(int calculatedTotalAmount, int totalUnitsAtResource);
        public int CalculateAmountUsed(PlayerAction playerAction);
        public double CalculateDistance(Position a, Position b);
    }
}
