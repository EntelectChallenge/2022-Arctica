package Enums;

public enum  GameObjectType {
    ERROR(0),
    PLAYER_BASE(1),
    SCOUT_TOWER(2),
    RESOURCE_NODE(3);

    private final int type;
    private GameObjectType(int gameObjectType)
    {
        type = gameObjectType;
    }

   

    public static GameObjectType valueOf(int Value){
        for (GameObjectType Types : GameObjectType.values()) {
            if (Types.type == Value) return Types;
          }
      
          throw new IllegalArgumentException("Value not found"); 
    }

}
