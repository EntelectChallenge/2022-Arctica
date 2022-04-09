package models;

import java.util.UUID;

public class CommandAction {
    private int type; // Connects to 'ActionType'
    private int units;
    private UUID id;

    public CommandAction() {
    }

    public CommandAction(int type, int units, UUID id) {
        this.type = type;
        this.units = units;
        this.id = id;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
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
