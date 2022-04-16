package Models;

public class World {
    int size = 0;
    int currentTick = 0;
    Map map = null;

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

    @Override
    public String toString() {
        return "world [currentTick=" + currentTick + ", map=" + map + ", size=" + size + "]";
    }


}
