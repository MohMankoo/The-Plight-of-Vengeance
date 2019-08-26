using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // The indices of the enemies correspond to the indices of fixedSpawnTimes
    // So, enemies[i]'s spawn time = fixedSpawnTimes[i]
    public GameObject[] enemies;
    public Transform[] spawnPoints;

    // Current wave information
    private List<GameObject> waveEnemies;
    private int waveNumber;

    // Spawn times for final wave - Wave 7
    public float[] fixedSpawnTimes;
    private float[] spawnTimes;

    // Round Display Information
    public Canvas gameHUD;
    public GameObject rouncCompleteDisplay;

    private void Start() {
        // Initialize spawn time values
        spawnTimes = new float[fixedSpawnTimes.Length];

        for (int i = 0; i < fixedSpawnTimes.Length; i++) {
            spawnTimes[i] = fixedSpawnTimes[i];
        }

        // Set to non-valid wave # so that update can increment it to a valid wave once spawner is activated
        waveNumber = 5;
    }

    private void Update() {
        // Create waves once previous wave enemies are dead
        if (AllElementsNull(waveEnemies) && waveNumber < 7) {
            waveNumber++;

            // Display Round Complete message for its duration of 1.2f
            if (waveNumber > 1) {
                GameObject roundCompleteMsg = 
                    Instantiate(rouncCompleteDisplay, gameHUD.transform.position, Quaternion.identity);
                roundCompleteMsg.transform.SetParent(gameHUD.transform);

                Destroy(roundCompleteMsg, 1.2f);
            }

            // Determine which wave to spawn
            switch (waveNumber) {
                case 1:
                    //waveEnemies = CreateWave(null, null, null, null, enemies[3]);
                    waveEnemies = CreateWave(enemies[0], enemies[0], null, null, enemies[0]);
                    break;
                case 2:
                    waveEnemies = CreateWave(enemies[0], enemies[0], enemies[0], enemies[0], enemies[1]);
                    break;
                case 3:
                    waveEnemies = CreateWave(enemies[1], enemies[1], enemies[0], enemies[0], enemies[1]);
                    break;
                case 4:
                    waveEnemies = CreateWave(enemies[1], enemies[1], enemies[1], enemies[1], enemies[2]);
                    break;
                case 5:
                    waveEnemies = CreateWave(enemies[2], enemies[2], enemies[1], enemies[1], enemies[2]);
                    break;
                case 6:
                    waveEnemies = CreateWave(enemies[0], null, null, enemies[0], enemies[3]);
                    break;
                default:
                    break;  // Go to wave 7 below
            }
        }

        // If wave 7, loop through the times for each enemy and spawn them when necessary
        if (waveNumber == 7)
            for (int i = 0; i < enemies.Length; i++) 
            {
                if (spawnTimes[i] <= 0) {
                    int spawnPoint = Random.Range(0, spawnPoints.Length - 1);  // Randomize the position
                    Instantiate(enemies[i], spawnPoints[spawnPoint].position, Quaternion.identity);

                    spawnTimes[i] = fixedSpawnTimes[i];  // Reset timer
                } else {
                    spawnTimes[i] -= Time.deltaTime;
                }
            }
        
    }

    // Pass in the enemy that should be spawned at each of the spawn points, null otherwise
    // sp = Spawn Point
    public List<GameObject> CreateWave(GameObject topLeftEnemy, GameObject topRightEnemy,
                GameObject bottomLeftEnemy, GameObject bottomRightEnemy, GameObject CenterEnemy) {

        // Create list of all spawned enemies
        List<GameObject> waveEnemies = new List<GameObject>();
        Quaternion faceDown = new Quaternion(0, 0, 180, 0);

        if (topLeftEnemy)
            waveEnemies.Add(Instantiate(topLeftEnemy, spawnPoints[0].position, faceDown));
        if (topRightEnemy)
            waveEnemies.Add(Instantiate(topRightEnemy, spawnPoints[1].position, faceDown));
        if (bottomLeftEnemy)
            waveEnemies.Add(Instantiate(bottomLeftEnemy, spawnPoints[2].position, faceDown));
        if (bottomRightEnemy)
            waveEnemies.Add(Instantiate(bottomRightEnemy, spawnPoints[3].position, faceDown));
        if (CenterEnemy)
            waveEnemies.Add(Instantiate(CenterEnemy, spawnPoints[4].position, faceDown));

        return waveEnemies;
    }

    private bool AllElementsNull(List<GameObject> list) {
        if (list == null) return true;

        // Only fail condition - at least one element is not null
        foreach (GameObject element in list) {
            if (element) return false;
        }

        return true;
    }

    // When the script is reset
    private void OnEnable() {
        // Destory all alive enemies
        if (waveEnemies != null)
            foreach (GameObject enemy in waveEnemies)
                if (enemy) Destroy(enemy);

        // Restart script
        Start();
    }

    private void OnDisable() {
        // Destory all alive enemies
        if (waveEnemies != null)
            foreach (GameObject enemy in waveEnemies)
                if (enemy) Destroy(enemy);
    }

}
