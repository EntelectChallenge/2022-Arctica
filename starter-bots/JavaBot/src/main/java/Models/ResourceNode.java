package Models;

import Enums.GameObjectType;
import Enums.ResourceType;


public class ResourceNode extends GameObject {
    int type;
    int amount = 0;
    int maxUnits = 0;
    int currentUnits = 0;
    int reward = 0;
    int workTime = 0;
    RegenerationRate regenerationRate;
   

    public ResourceNode() {
        super(GameObjectType.RESOURCE_NODE);
        this.currentUnits = 0;
    }

//    public ResourceNode(UUID id, GameObjectType gameObjectType, Position position) {
//        super(id, gameObjectType, position);
//    }

    public ResourceType getType() {
        return ResourceType.valueOf(type);
    }

    public void setType(int type) {
        this.type = type;
    }

    public int getAmount() {
        return amount;
    }

    public void setAmount(int amount) {
        this.amount = amount;
    }

    public int getMaxUnits() {
        return maxUnits;
    }

    public void setMaxUnits(int maxUnits) {
        this.maxUnits = maxUnits;
    }

    public int getCurrentUnits() {
        return currentUnits;
    }

    public void setCurrentUnits(int currentUnits) {
        this.currentUnits = currentUnits;
    }

    public int getReward() {
        return reward;
    }

    public void setReward(int reward) {
        this.reward = reward;
    }

    public int getWorkTime() {
        return workTime;
    }

    public void setWorkTime(int workTime) {
        this.workTime = workTime;
    }


    public RegenerationRate getRegenerationRate() {
        return regenerationRate;
    }

    public void setRegenerationRate(RegenerationRate regenerationRate) {
        this.regenerationRate = regenerationRate;
    }

    @Override
    public String toString() {
        return "ResourceNode [ID=" + getId() + ", amount=" + amount + ", currentUnits=" + currentUnits + ", gameObjectType=" + getGameObjectType() + ", maxUnits=" + maxUnits + ", position=" + getPosition() + ", regenerationRate=" + regenerationRate + ", reward=" + reward + ", type=" + type + ", worktime=" + workTime + "]";
    }


}

class RegenerationRate {

    int ticks = 0;
    int amount = 0;

    @Override
    public String toString() {
        return "RegenerationRate [amount=" + amount + ", ticks=" + ticks + "]";
    }

    public RegenerationRate(int ticks, int amount) {
        this.ticks = ticks;
        this.amount = amount;
    }
    public RegenerationRate() {
        this.ticks = 0;
        this.amount = 0;
    }

    public int getTicks() {
        return ticks;
    }

    public void setTicks(int ticks) {
        this.ticks = ticks;
    }

    public int getAmount() {
        return amount;
    }

    public void setAmount(int amount) {
        this.amount = amount;
    }

}