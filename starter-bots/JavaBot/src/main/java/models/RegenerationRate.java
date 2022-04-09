package models;

public class RegenerationRate {
    private int ticks;
    private int amount;

    public RegenerationRate() {
    }

    public RegenerationRate(int ticks, int amount) {
        this.ticks = ticks;
        this.amount = amount;
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
