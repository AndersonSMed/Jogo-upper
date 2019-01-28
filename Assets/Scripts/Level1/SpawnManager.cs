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

    private void SpawnEnemy() {
        // If the game is paused, wait for it to spawn a new enemy
        if (GameManager.Instance.IsGamePaused()) {
            Invoke("SpawnEnemy", spawnRate);
        } else {
            GameObject enemy = Instantiate(enemyPrefab);
            GameObject spawn = spawns[Random.Range(0, spawns.Length - 1)];
            enemies.Add(enemy);
            enemy.transform.position = new Vector2(spawn.transform.position.x, spawn.transform.position.y);
            Invoke("SpawnEnemy", spawnRate);
        }
    }

    private void DecreaseRate() {
        // Verifies if the game is paused, to decrease the rate
        if (GameManager.Instance.IsGamePaused()) {
            Invoke("DecreaseRate", 0.5f);
        } else {
            spawnRate -= decreaseRate;
            Invoke("DecreaseRate", timeToDecrease);
        }
    }


}
