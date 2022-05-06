using System;
using System.Collections.Generic;

// The player's local map

namespace Domain.Models.DTOs
{
    public class BotDto
    {
        public Guid Id { get; set; }
        public int? CurrentTierLevel { get; set; }
        public int Tick { get; set; }
        public BotMapState Map { get; set; }
        public int? Population { get; set; }
        public Position BaseLocation { get; set; }
        public List<PlayerActionDto> PendingActions { get; set; }
        public List<PlayerActionDto> Actions { get; set; }
        /*public int TravellingUnits { get; set; }
        public int LumberingUnits { get; set; }
        public int MiningUnits { get; set; }
        public int FarmingUnits { get; set; }
        public int ScoutingUnits { get; set; }*/
        public int? AvailableUnits { get; set; }
        public int? Seed { get; set; }
        public int? Wood { get; set; }
        public int? Food { get; set; }
        public int? Stone { get; set; }
        public int? Heat { get; set; }

        public static BotDto GetStaticFields(BotDto botDto)
        {
            return new BotDto
            {
                Id = botDto.Id,
                //CurrentTierLevel = botDto.CurrentTierLevel,
                //Tick = botDto.Tick,
                //Map = botDto.Map,
                //Population = botDto.Population,
                BaseLocation = botDto.BaseLocation,
                //TravellingUnits = botDto.TravellingUnits,
                //LumberingUnits = botDto.LumberingUnits,
                //MiningUnits = botDto.MiningUnits,
                //FarmingUnits = botDto.FarmingUnits,
                //ScoutingUnits = botDto.ScoutingUnits,
                //AvailableUnits = botDto.AvailableUnits,
                Seed = botDto.Seed,
                //Wood = botDto.Wood,
                //Food = botDto.Food,
                //Stone = botDto.Stone,
                //Heat = botDto.Heat
            };

        }

        public static BotDto GetVariableFields(BotDto previousBDto, BotDto currentBDto)
        {
            if (previousBDto.Equals(currentBDto))
            {
                return null;
            }

            return new BotDto
            {
                Id = currentBDto.Id,
                CurrentTierLevel = currentBDto.CurrentTierLevel == previousBDto.CurrentTierLevel ? null : currentBDto.CurrentTierLevel,
                Tick = currentBDto.Tick,
                //Should there be a condition to see if the value has been changed
                Map = BotMapState.GetVariableFields(previousBDto.Map, currentBDto.Map),

                Population = currentBDto.Population == previousBDto.Population ? null : currentBDto.Population,
                PendingActions = currentBDto.PendingActions == previousBDto.PendingActions ? null : currentBDto.PendingActions,
                Actions = currentBDto.Actions == previousBDto.Actions ? null : currentBDto.Actions,
                Wood = currentBDto.Wood == previousBDto.Wood ? null : currentBDto.Wood,
                Food = currentBDto.Food == previousBDto.Food ? null : currentBDto.Food,
                Stone = currentBDto.Stone == previousBDto.Stone ? null : currentBDto.Stone,
                Heat = currentBDto.Heat == previousBDto.Heat ? null : currentBDto.Heat
            };
        }
    }
}