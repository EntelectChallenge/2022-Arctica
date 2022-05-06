using System;
using NETCoreBot.Enums;

namespace NETCoreBot.Models
{
    public class PlayerActionDto
    {
        public Guid TargetNodeId { get; set; }
        public int NumberOfUnits { get; set; }
        public int TickActionCompleted { get; set; }
        public int TickActoinStart { get; set; }
        public int TickReceived { get; set; }
        public ActionType ActionType { get; set; }
    }
}