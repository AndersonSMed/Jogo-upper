using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour {

    // Call method to Start the game
    public void StartGame() {
        GameManager.Instance.StartGame();
    }

    // Call method to close the game
    public void ExitGame() {
        GameManager.Instance.ExitGame();
    }

    public void LoadMenu() {

    }

    public void LoadAudioOptions() {

    }

}
