package models;

public class World {
    private int size;
    private int currentTick;
    private Map map;

    public World() {
    }

    public World(int size, int currentTick, Map map) {
        this.size = size;
        this.currentTick = currentTick;
        this.map = map;
    }

    public int getSize() {
        return size;
    }

    public void setSize(int size) {
        this.size = size;
    }

    public int getCurrentTick() {
        return currentTick;
    }

    public void setCurrentTick(int currentTick) {
        this.currentTick = currentTick;
    }

    public Map getMap() {
        return map;
    }

    public void setMap(Map map) {
        this.map = map;
    }

}
