package Models;

import java.util.ArrayList;
import java.util.List;

public class Map {

    public List<ScoutTower> scoutTowers;
    public List<ResourceNode> nodes;

    public Map(List<ScoutTower> scoutTowers, List<ResourceNode> resourceNodes) {
        this.scoutTowers = scoutTowers;
        nodes = resourceNodes;
    }

    public Map(){
        scoutTowers = new ArrayList<ScoutTower>();
        nodes = new ArrayList<ResourceNode>();
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

//    public ResourceNode getResourceNode(UUID id) {
//        for (ResourceNode node : Nodes) {
//            if (node.getId().equals(id)) {
//                return node;
//            }
//        }
//        return null;
//    }

    @Override
    public String toString() {
        return "Map [ResourceNodes=" + nodes + ", ScoutTowers=" + scoutTowers + "]";
    }


}
