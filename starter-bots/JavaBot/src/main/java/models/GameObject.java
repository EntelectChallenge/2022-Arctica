package models;

import enums.GameObjectType;

import java.util.UUID;

public abstract class GameObject {
    private UUID id;
    private GameObjectType gameObjectType;
    private Position position;

    public GameObject(GameObjectType gameObjectType) {
        this.gameObjectType = gameObjectType;
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
}
