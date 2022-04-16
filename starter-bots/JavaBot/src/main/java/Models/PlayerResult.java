package Models;

public class PlayerResult {
  
    int placement = 0;
    int seed =0; 
    int score =0;
    String id ="";
    String nickname ="";
    int matchPoints = 0;
    
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

    @Override
    public String toString() {
        return "PlayerResult [id=" + id + ", matchPoints=" + matchPoints + ", nickname=" + nickname + ", placement="
                + placement + ", score=" + score + ", seed=" + seed + "]";
    }

    
}
