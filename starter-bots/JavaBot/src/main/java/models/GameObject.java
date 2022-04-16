package models;

import java.util.UUID;

public abstract class GameObject {
    private UUID id;
    private int gameObjectType;
    private Position position;

    public GameObject(int gameObjectType) {
        this.gameObjectType = gameObjectType;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public int getGameObjectType() {
        return gameObjectType;
    }

    public void setGameObjectType(int gameObjectType) {
        this.gameObjectType = gameObjectType;
    }

    public Position getPosition() {
        return position;
    }

    public void setPosition(Position position) {
        this.position = position;
    }

}
