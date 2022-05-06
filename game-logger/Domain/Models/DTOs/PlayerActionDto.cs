using System;
using Domain.Enums;

namespace Domain.Models.DTOs;

public class PlayerActionDto
{
    public Guid TargetNodeId { get; set; }
    public int NumberOfUnits { get; set; }
    public int TickActionCompleted { get; set; }
    public int TickActionStart { get; set; }
    public int TickReceived { get; set; }
    public ActionType ActionType { get; set; }
}