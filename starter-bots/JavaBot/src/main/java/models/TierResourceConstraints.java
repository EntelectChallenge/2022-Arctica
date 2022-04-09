package models;

public class TierResourceConstraints {
    private int food;
    private int wood;
    private int stone;

    public TierResourceConstraints() {
    }

    public TierResourceConstraints(int food, int wood, int stone) {
        this.food = food;
        this.wood = wood;
        this.stone = stone;
    }

    public int getFood() {
        return food;
    }

    public void setFood(int food) {
        this.food = food;
    }

    public int getWood() {
        return wood;
    }

    public void setWood(int wood) {
        this.wood = wood;
    }

    public int getStone() {
        return stone;
    }

    public void setStone(int stone) {
        this.stone = stone;
    }

    @Override
    public String toString() {
        return "TierResourceConstraints{" +
                "food=" + food +
                ", wood=" + wood +
                ", stone=" + stone +
                '}';
    }
}
