package utils;

import java.util.Random;

import Models.Position;

public class MathUtils {
    public static double calculateDistanceBetweenPoints(
  Position positionTo, Position positionFrom) {       
    return Math.sqrt((positionFrom.y - positionTo.y) * (positionFrom.y - positionTo.y) + (positionFrom.x - positionTo.x) * (positionFrom.x - positionTo.x));
}

public  static int randNumGenerator(int max){
    Random rand = new Random();
    return rand.nextInt(max);
}
}
