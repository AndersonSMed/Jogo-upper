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
    }

    // Returns the actual life of our player
    public float GetLife() {
        return actualLife;
    }

}
