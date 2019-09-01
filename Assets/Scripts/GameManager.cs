using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // Player
    public static Vector3 defaultPlayerPos = new Vector3(0, -6.5f, 0);
    public static Quaternion defaultPlayerRotation = Quaternion.identity;

    // Enemy
    public static float minDistanceBetweenEnemies = 2f;

    // Boundaries
    public static readonly float MIN_X_BOUNDARY = -12.06596f;
    public static readonly float MAX_X_BOUNDARY = 11.81223f;
    public static readonly float MIN_Y_BOUNDARY = -7.054649f;
    public static readonly float MAX_Y_BOUNDARY = 7.02379f;

    // Camera
    public static Vector3 defaultCameraPos = new Vector3(0, 0, -5);
}
