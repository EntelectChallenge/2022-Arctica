package Models;

import java.util.UUID;

import Enums.GameObjectType;

public class GameObject {

    UUID id;
    GameObjectType gameObjectType;
    Position position;

//    public GameObject(UUID id, GameObjectType gameObjectType, Position position) {
//        this.id = id;
//        this.gameObjectType = gameObjectType;
//        this.position = position;
//    }
    

    public GameObject(GameObjectType gameObjectType) {
        this.id= new UUID(0, 10);
        this.gameObjectType = gameObjectType;
    }

    public GameObject() {
        this.id= new UUID(0, 10);
        this.gameObjectType = gameObjectType.SCOUT_TOWER;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public GameObjectType getGameObjectType() {
        return gameObjectType;
    }

    public void setGameObjectType(GameObjectType gameObjectType) {
        this.gameObjectType = gameObjectType;
    }

    public Position getPosition() {
        return position;
    }

    public void setPosition(Position position) {
        this.position = position;
    }

    @Override
    public String toString() {
        return "GameObject [id=" + id + ", position=" + position + ", type=" + gameObjectType + "]";
    }


}
