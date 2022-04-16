package enums;

import java.util.Objects;

public enum ResourceType {
    ERROR(0),
    WOOD(1),
    FOOD(2),
    STONE(3),
    GOLD(4),
    HEAT(5);

    private final int value;

    ResourceType(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public static ResourceType valueOf(int value) {
        for (ResourceType type : ResourceType.values()) {
            if (Objects.equals(type.value, value)) {
                return type;
            }
        }

        throw new IllegalArgumentException("Value not found");
    }

}
