using System;
using Domain.Enums;

namespace Domain.Models
{
    public class PlayerAction
    {
        public Position TargetNode { get; set; }
        public int NumberOfUnits { get; set; }
        public BotObject Bot { get; set; }
        public int ExpectedCompletedTick { get; set; }
        public ActionType ActionType { get; set; }
    }
}