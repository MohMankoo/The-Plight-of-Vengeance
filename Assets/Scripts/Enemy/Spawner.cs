using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [Header("Round Display Information")]
    public Canvas gameHUD;
    public GameObject rouncCompleteDisplay;

    // The indices of the enemies correspond to the indices of fixedSpawnTimes
    // So, enemies[i]'s spawn time = fixedSpawnTimes[i]
    [Header("Enemy types")]
    public GameObject[] enemies;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Times")]
    public float[] fixedSpawnTimes;  // Spawn times for final wave - Wave 7
    private float[] spawnTimes;

    // Current wave information
    private List<GameObject> waveEnemies;
    private int waveNumber;

    private void Start() {
        // Initialize spawn time values
        spawnTimes = new float[fixedSpawnTimes.Length];
        for (int i = 0; i < fixedSpawnTimes.Length; i++) {
            spawnTimes[i] = fixedSpawnTimes[i];
        }

        // Setup a clean wave
        // Let wave 0 indicate the "wave" preceding the start of the game
        waveNumber = 0;
        waveEnemies = new List<GameObject>();
    }

    private void Update() {
        // If wave 7, loop through the times for each enemy and spawn them when necessary
        if (waveNumber == 7)
            for (int i = 0; i < enemies.Length; i++) {
                if (spawnTimes[i] <= 0) {
                    int spawnPoint = Random.Range(0, spawnPoints.Length - 1);  // Randomize the spawn
                    waveEnemies.Add(Instantiate(enemies[i], spawnPoints[spawnPoint].position, Quaternion.identity));

                    spawnTimes[i] = fixedSpawnTimes[i];  // Reset timer
                } else {
                    spawnTimes[i] -= Time.deltaTime;
                }
            }

        // Create waves once previous wave enemies are dead
        if (waveNumber < 7 && (waveEnemies.Count == 0 || AreAllElementsNull(waveEnemies))) {
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

                    // Prepare for the infinite wave - Wave 7
                    // Have at least one enemy ready for the player to fight
                    spawnTimes[0] = 0;
                    break;
                default:
                    break;  // Go to wave 7 below
            }
        }  // End of if

    }

    // Pass in the enemy that should be spawned at each of the spawn points, null otherwise
    // sp = Spawn Point
    public List<GameObject> CreateWave(GameObject topLeftEnemy, GameObject topRightEnemy,
                GameObject bottomLeftEnemy, GameObject bottomRightEnemy, GameObject CenterEnemy) {

        // Create list of all spawned enemies
        List<GameObject> wave = new List<GameObject>();
        Quaternion faceDown = new Quaternion(0, 0, 180, 0);

        if (topLeftEnemy)
            wave.Add(Instantiate(topLeftEnemy, spawnPoints[0].position, faceDown));
        if (topRightEnemy)
            wave.Add(Instantiate(topRightEnemy, spawnPoints[1].position, faceDown));
        if (bottomLeftEnemy)
            wave.Add(Instantiate(bottomLeftEnemy, spawnPoints[2].position, faceDown));
        if (bottomRightEnemy)
            wave.Add(Instantiate(bottomRightEnemy, spawnPoints[3].position, faceDown));
        if (CenterEnemy)
            wave.Add(Instantiate(CenterEnemy, spawnPoints[4].position, faceDown));

        return wave;
    }

    private bool AreAllElementsNull(List<GameObject> list) {
        if (list == null) return true;

        // Only fail condition - at least one element is not null
        foreach (GameObject element in list) {
            if (element) return false;
        }

        return true;
    }

    // When the script is reset
    void OnEnable() {
        Debug.Log("Spawner enabled");
        Debug.Log("Wave enemies:");
        foreach (var enemy in waveEnemies) {
            Debug.Log(enemy);
        }

        DestroyWave();
        Start();  // Start waves from 0
    }

    /*void OnDisable() {
        Debug.Log("Spawner disabled");
        Debug.Log("Wave enemies:");
        foreach (var enemy in waveEnemies) {
            Debug.Log(enemy);
        }

        DestroyWave();
    }*/

    // Destory all alive wave enemies
    // Note: the list waveEnemies is still unclean and needs to be remade (full of nulls)
    // Start() will clean up this list automatically
    private void DestroyWave() {
        if (waveEnemies != null) {
            foreach (GameObject enemy in waveEnemies)
                if (enemy) Destroy(enemy);
            //waveEnemies = new List<GameObject>();
        }
    }

}
