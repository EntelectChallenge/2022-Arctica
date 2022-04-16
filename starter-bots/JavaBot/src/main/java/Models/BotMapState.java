package Models;

import java.util.List;
import java.util.UUID;

public class BotMapState {
    List<UUID> scoutTowers;
    List<UUID> nodes;

    public BotMapState(List<UUID> scoutTower, List<UUID> nodes) {
        this.scoutTowers = scoutTower;
        this.nodes = nodes;
    }

    public List<UUID> getScoutTowers() {
        return scoutTowers;
    }

    public void setScoutTowers(List<UUID> scoutTowers) {
        this.scoutTowers = scoutTowers;
    }

    public List<UUID> getNodes() {
        return nodes;
    }

    public void setNodes(List<UUID> nodes) {
        this.nodes = nodes;
    }

    @Override
    public String toString() {
        return "BotMapState [nodes=" + nodes + ", scoutTower=" + scoutTowers + "]";
    }

}
