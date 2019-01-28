using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField]
    // Stores the enemy prefab that will be spawned
    private GameObject enemyPrefab;
    [SerializeField]
    // Stores the spawns in the scene
    private GameObject[] spawns;
    [SerializeField]
    // At every x seconds, spawns a new enemy, this attribute maintains that x
    private float spawnRate = 1f;
    [SerializeField]
    // At every timeToDecrease, decrease the spawnRate
    private float timeToDecrease = 10f;
    [SerializeField]
    // This is the decreaseRate of spawnRate
    private float decreaseRate = 0.1f;

    //Maintains a list of enemies
    private List<GameObject> enemies;

    private void Start() {
        // Start the enemies list
        enemies = new List<GameObject>();
        // Invoke the method DecreaseRate at a timeToDecrease seconds
        Invoke("DecreaseRate", timeToDecrease);
        // Spawns a enemy when this object is started
        SpawnEnemy();
    }

    private void Update() {
        // Remove all the enemies who are null
        enemies.RemoveAll(enemy => enemy == null);
        // And then update the enemies left
        GameManager.Instance.ChangeEnemiesLeft(enemies.Count);
    }

    private void SpawnEnemy() {
        // If there's still time to spawn enemies
        if (GameManager.Instance.GetTimeLeft() > 0) {
            // If the game is paused, wait for it to spawn a new enemy
            if (GameManager.Instance.IsGamePaused()) {
                Invoke("SpawnEnemy", spawnRate);
            } else {
                // Create a new enemy
                GameObject enemy = Instantiate(enemyPrefab);
                // Then take a random spawn and set the enemy's position to it
                GameObject spawn = spawns[Random.Range(0, spawns.Length - 1)];
                enemy.transform.position = new Vector2(spawn.transform.position.x, spawn.transform.position.y);
                // And store the enemy in the enemies list
                enemies.Add(enemy);
                // Call this method again, at a spawnRate seconds
                Invoke("SpawnEnemy", spawnRate);
            }
        }
    }

    private void DecreaseRate() {
        // If there's still time to spawn enemies
        if (GameManager.Instance.GetTimeLeft() > 0) {
            // Verifies if the game is paused, to decrease the rate
            if (GameManager.Instance.IsGamePaused()) {
                Invoke("DecreaseRate", 0.5f);
            } else {
                spawnRate -= decreaseRate;
                Invoke("DecreaseRate", timeToDecrease);
            }
        }
    }


}
