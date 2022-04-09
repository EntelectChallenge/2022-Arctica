package models;

import enums.ActionType;

import java.util.UUID;

public class CommandAction {
    private ActionType type;
    private int units;
    private UUID id;

    public CommandAction() {
    }

    public CommandAction(ActionType type, int units, UUID id) {
        this.type = type;
        this.units = units;
        this.id = id;
    }

    public ActionType getType() {
        return type;
    }

    public void setType(ActionType type) {
        this.type = type;
    }

    public int getUnits() {
        return units;
    }

    public void setUnits(int units) {
        this.units = units;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }
}
