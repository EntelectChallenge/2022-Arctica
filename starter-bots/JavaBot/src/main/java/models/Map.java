package models;

import java.util.List;

public class Map {
    private List<ScoutTower> scoutTowers;
    private List<ResourceNode> nodes;

    public Map() {
    }

    public Map(List<ScoutTower> scoutTowers, List<ResourceNode> nodes) {
        this.scoutTowers = scoutTowers;
        this.nodes = nodes;
    }

    public List<ScoutTower> getScoutTowers() {
        return scoutTowers;
    }

    public void setScoutTowers(List<ScoutTower> scoutTowers) {
        this.scoutTowers = scoutTowers;
    }

    public List<ResourceNode> getNodes() {
        return nodes;
    }

    public void setNodes(List<ResourceNode> nodes) {
        this.nodes = nodes;
    }

}
