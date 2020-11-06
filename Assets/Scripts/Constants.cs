using UnityEngine;
using System.Collections;

public class Constants
{
    public const int OBSTACLES_INTER_DISTANCE = 8;
    public const float REPULSION_FORCE = 10f;
    public const float MOVEMENT_FORCE = 500f;
    public const float CLOUD_BRAKE = 0.90f;
    public enum GameStatus { STOPPED, TUTORIAL, PLAYING }
    public enum ObstacleType { CLOUD_1, CLOUD_2, EAGLE, ASTEROID, SPACESHIP, SPIKETRAP, ROCKET, PYRAMID, VORTEX };

    public static readonly (ObstacleType type, int probability)[] OBSTACLE_PROBABILITY = new (ObstacleType, int)[9] {
        //(ObstacleType.PYRAMID, 100),    // TEST
        (ObstacleType.CLOUD_1, 5),     // 5 
        (ObstacleType.CLOUD_2, 10),    // 5
        (ObstacleType.EAGLE, 31),      // 21
        (ObstacleType.ASTEROID, 48),   // 17
        (ObstacleType.SPACESHIP, 65),  // 17
        (ObstacleType.SPIKETRAP, 71),  // 6
        (ObstacleType.ROCKET, 89),     // 17
        (ObstacleType.PYRAMID, 91),    // 2
        (ObstacleType.VORTEX, 100),    // 9
    };

    public enum ObstaclesMovement { ONE_DIMENSION, TWO_DIMENSIONS, STATIC}



}
