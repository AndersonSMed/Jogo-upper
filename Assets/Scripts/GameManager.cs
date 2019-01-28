using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Instance created to use a singleton pattern
    private static GameManager instance;

    [SerializeField]
    // Stores the initial life of our player
    private float initialLife = 10f;
    [SerializeField]
    // Stores the key that will be used to pause and unpause the game
    private KeyCode pauseKey;
    [SerializeField]
    // Stores the initial time left to win the game
    private float initialTimeLeft = 60f;

    // Stores if the game is paused
    private bool gamePaused = false;
    // Stores the actual life of our player
    private float actualLife;
    // Stores if the game is already started
    private bool gameStarted = false;
    // Stores the actual time left to win the game
    private float timeLeft;
    // Stores the number of enemies left to win the game
    private int enemiesLeft = 0;

    // Getter of instance
    public static GameManager Instance {
        get {
            return instance;
        }
    }

    // When this object is created, call this method and create a instance, if there's none
    private void Awake() {
        if(instance == null) {
            instance = this;
        }else if(instance != this) {
            Destroy(instance);
        }
        // This method is called for Unity to know that instance can't be destroyed when we change our scene
        DontDestroyOnLoad(instance);
    }

    private void Start() {
        // Create a delegator to call everytime that our scene changed to a new scene
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void Update() {
        //  Verifies if the game has started and can be paused
        if (Input.GetKeyUp(pauseKey) && gameStarted) {
            gamePaused = !gamePaused;
        }
    }

    private void ActiveSceneChanged(Scene current, Scene next) {
        // When the new scene is called "Level1", call the ResetGame method
        if (next.name == "Level1") {
            ResetGame();
            Invoke("DecreaseTimer", 1f);
        } else {
            gameStarted = false;
        }
    }

    // Method called every 1 second to decrease the timer
    private void DecreaseTimer() {
        // If game is started and theres time left
        if (gameStarted && timeLeft > 0) {
            // And the game is not paused
            if (!gamePaused) {
                // Decrease the time left
                timeLeft--;
                // If time left and enemies left are equals to zero, then the player wins
                if(timeLeft == 0 && enemiesLeft == 0) {
                    YouWin();
                }
            }
            Invoke("DecreaseTimer", 1f);
        }
    }

    // Method called when the player dies
    private void GameOver() {
        SceneManager.LoadScene("GameOver");
    }

    // Method called when the player wins the game
    private void YouWin() {
        SceneManager.LoadScene("YouWin");
    }

    // Method called when this gameobject is destroied
    private void OnDestroy() {
        // Remove the delegator from activeSceneChanged
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    // Method called to reset the game parameters
    private void ResetGame() {
        gameStarted = true;
        gamePaused = false;
        actualLife = initialLife;
        timeLeft = initialTimeLeft;
    }

    // Method called when we need to pause our game
    public void PauseGame() {
        gamePaused = true;
    }

    // Change Scene to start the game
    public void StartGame() {
        SceneManager.LoadScene("Level1");
    }

    // Method called to update the enemies left
    public void ChangeEnemiesLeft(int enemies) {
        enemiesLeft = enemies;
        // If the enemies left is zero and time left is zero
        if (enemiesLeft == 0 && timeLeft == 0) {
            // Then the player wins
            YouWin();   
        }
    }

    public int GetEnemiesLeft() {
        return enemiesLeft;
    }

    public float GetTimeLeft() {
        return timeLeft;
    }

    // Method called when we need to change to menu scene
    public void LoadMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    // Closes the game
    public void ExitGame() {
        Application.Quit();
    }

    // Method called when our game is paused and we need to resume it
    public void ResumeGame() {
        gamePaused = false;
    }

    // Return if our game is paused or not
    public bool IsGamePaused() {
        return gamePaused;
    }

    // Method called when we need to damage our player
    public void DamagePlayer(float damage) {
        actualLife -= damage;
        if(actualLife <= 0) {
            GameOver();
        }
    }

    // Returns the actual life of our player
    public float GetLife() {
        return actualLife;
    }

}
