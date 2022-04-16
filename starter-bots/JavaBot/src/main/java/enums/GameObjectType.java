package enums;

import java.util.Objects;

public enum GameObjectType {
    ERROR(0),
    PLAYER_BASE(1),
    SCOUT_TOWER(2),
    RESOURCE_NODE(3);

    private final int value;

    GameObjectType(int gameObjectType) {
        value = gameObjectType;
    }

    public int getValue() {
        return value;
    }

    public static GameObjectType valueOf(int value) {
        for (GameObjectType type : GameObjectType.values()) {
            if (Objects.equals(type.value, value)) {
                return type;
            }
        }

        throw new IllegalArgumentException("Value not found");
    }

}
