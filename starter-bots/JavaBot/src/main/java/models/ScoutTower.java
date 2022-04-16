package models;

import enums.GameObjectType;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class ScoutTower extends GameObject {
    private List<UUID> nodes;

    public ScoutTower() {
        super(GameObjectType.SCOUT_TOWER.getValue());
        this.nodes = new ArrayList<>();
    }

    public ScoutTower(List<UUID> nodes) {
        super(GameObjectType.SCOUT_TOWER.getValue());
        this.nodes = nodes;
    }

    public List<UUID> getNodes() {
        return nodes;
    }

    public void setNodes(List<UUID> nodes) {
        this.nodes = nodes;
    }

}
