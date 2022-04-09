package models;

public class PlayerResult {
    private int placement;
    private int seed;
    private int score;
    private String id;
    private String nickname;
    private int matchPoints;

    public PlayerResult() {
    }

    public PlayerResult(int placement, int seed, int score, String id, String nickname, int matchPoints) {
        this.placement = placement;
        this.seed = seed;
        this.score = score;
        this.id = id;
        this.nickname = nickname;
        this.matchPoints = matchPoints;
    }

    public int getPlacement() {
        return placement;
    }

    public void setPlacement(int placement) {
        this.placement = placement;
    }

    public int getSeed() {
        return seed;
    }

    public void setSeed(int seed) {
        this.seed = seed;
    }

    public int getScore() {
        return score;
    }

    public void setScore(int score) {
        this.score = score;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getNickname() {
        return nickname;
    }

    public void setNickname(String nickname) {
        this.nickname = nickname;
    }

    public int getMatchPoints() {
        return matchPoints;
    }

    public void setMatchPoints(int matchPoints) {
        this.matchPoints = matchPoints;
    }
}
