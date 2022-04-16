package Enums;

public enum ResourceType {
    ERROR(0),
    WOOD(1),
    FOOD(2),
    STONE(3),
    GOLD(4),
    HEAT(5);

    public final int value;
    private ResourceType(int value)
    {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public static ResourceType valueOf(int Value){
        for (ResourceType Types : ResourceType.values()) {
            if (Types.value == Value) return Types;
          }
      
          throw new IllegalArgumentException("Value not found"); 
    }

}