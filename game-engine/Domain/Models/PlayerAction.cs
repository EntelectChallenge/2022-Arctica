using System;
using Domain.Enums;
using Newtonsoft.Json;

namespace Domain.Models
{
    public class PlayerAction
    {
        public Guid TargetNodeId { get; set; }

        public int NumberOfUnits { get; set; }
        public BotObject Bot { get; set; }

        public int ExpectedCompletedTick { get; set; }
        public int StartTick { get; set; }

        public ActionType ActionType { get; set; }
        
        public PlayerAction(ActionType action, int noOfUnits, Guid id)
        {
            ActionType = action;
            TargetNodeId = id;
            NumberOfUnits = noOfUnits;
        }

        public void SetStartAndEndTicks(int currentTick, int travelTime, int actionDuration)
        {
            StartTick = currentTick + travelTime;
            ExpectedCompletedTick = StartTick + actionDuration;
        }

        public bool CheckActionComplete(int currentTick)
        {
            return currentTick > ExpectedCompletedTick;
        }

        public void Remove()
        {
            Bot.RemoveAction(this);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}