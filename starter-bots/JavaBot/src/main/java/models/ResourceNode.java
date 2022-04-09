package models;

import enums.GameObjectType;

public class ResourceNode extends GameObject {
    private int type; // Connects to 'ResourceType'
    private int amount;
    private int maxUnits;
    private int currentUnits;
    private int reward;
    private int workTime;
    private RegenerationRate regenerationRate;

    public ResourceNode() {
        super(GameObjectType.RESOURCE_NODE.getValue());
    }

    public ResourceNode(int type, int amount, int maxUnits, int currentUnits, int reward, int workTime, RegenerationRate regenerationRate) {
        super(GameObjectType.RESOURCE_NODE.getValue());
        this.type = type;
        this.amount = amount;
        this.maxUnits = maxUnits;
        this.currentUnits = currentUnits;
        this.reward = reward;
        this.workTime = workTime;
        this.regenerationRate = regenerationRate;
    }

    public int getType() {
        return type;
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

}
