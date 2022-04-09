package enums;

import java.util.Objects;

public enum ActionType {
    ERROR(0),
    SCOUT(1),
    MINE(2),
    FARM(3),
    LUMBER(4),
    START_CAMPFIRE(5);

    private final int value;

    ActionType(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public static ActionType valueOf(int value) {
        for (ActionType actionType : ActionType.values()) {
            if (Objects.equals(actionType.value, value)) {
                return actionType;
            }
        }

        throw new IllegalArgumentException("Value not found");
    }

}
