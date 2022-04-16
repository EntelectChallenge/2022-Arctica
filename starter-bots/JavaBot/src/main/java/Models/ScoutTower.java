package Models;

import Enums.GameObjectType;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class ScoutTower extends GameObject {

    List<UUID> nodes;

    public ScoutTower() {
        super(GameObjectType.SCOUT_TOWER);
        this.nodes = new ArrayList<UUID>();
    }

//    public ScoutTower(UUID id, GameObjectType gameObjectType, Position position, List<UUID> nodes) {
//        super(id, gameObjectType, position);
//
//        this.nodes = nodes;
//
//    }

    public List<UUID> getNodes() {
        return nodes;
    }

    public void setNodes(List<UUID> nodes) {
        this.nodes = nodes;
    }
//    @Override
//    public String toString() {
//        return "ScoutTower [   nodes=" + nodes + ", position=" + position  "]";
//    }
}
