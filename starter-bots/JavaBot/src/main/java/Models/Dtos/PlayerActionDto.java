package Models.Dtos;

import Enums.ActionTypes;

import java.util.UUID;

public class PlayerActionDto {
    public UUID TargetNodeId;
    public int NumberOfUnits;
    public int ExpectedCompletedTick;
    public int StartTick;
    public ActionTypes ActionType;

}
