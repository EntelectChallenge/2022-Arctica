package services;

import models.Position;

import java.util.Random;

public class UtilsService {

    public static double calculateDistanceBetweenPoints(Position positionOne, Position positionTwo) {
        return Math.sqrt(
                Math.pow(positionTwo.getX() - positionOne.getX(), 2) + Math.pow(positionTwo.getY() - positionOne.getY(), 2)
        );
    }

    public static int randNumGenerator(int max) {
        Random rand = new Random();
        return rand.nextInt(max);
    }

}
