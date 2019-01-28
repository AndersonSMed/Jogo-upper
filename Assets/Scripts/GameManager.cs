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

    // Stores if the game is paused
    private bool gamePaused = false;
    // Stores the actual life of our player
    private float actualLife;

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
        // Starts the actual life with the life defined before as initial
        actualLife = initialLife;
    }

    private void ActiveSceneChanged(Scene current, Scene next) {
        // When the new scene is called "Level1", call the ResetGame method
        if (next.name == "Level1") {
            ResetGame();
        }
    }

    // Method called when the player dies
    private void GameOver() {
        SceneManager.LoadScene("GameOver");
    }

    // Method called when this gameobject is destroied
    private void OnDestroy() {
        // Remove the delegator from activeSceneChanged
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    // Method called to reset the game parameters
    private void ResetGame() {
        gamePaused = false;
        actualLife = initialLife;
    }

    // Method called when we need to pause our game
    public void PauseGame() {
        gamePaused = true;
    }

    // Change Scene to start the game
    public void StartGame() {
        SceneManager.LoadScene("Level1");
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
