package models;

import java.util.List;
import java.util.UUID;

public class BotMapState {
    private List<UUID> scoutTowers;
    private List<UUID> nodes;

    public BotMapState() {
    }

    public BotMapState(List<UUID> scoutTowers, List<UUID> nodes) {
        this.scoutTowers = scoutTowers;
        this.nodes = nodes;
    }

    public List<UUID> getScoutTowers() {
        return scoutTowers;
    }

    public List<UUID> getNodes() {
        return nodes;
    }

    public void setScoutTowers(List<UUID> scoutTowers) {
        this.scoutTowers = scoutTowers;
    }

    public void setNodes(List<UUID> nodes) {
        this.nodes = nodes;
    }

}
