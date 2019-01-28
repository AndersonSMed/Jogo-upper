using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGameOver : MonoBehaviour {

    // Exits the game
	public void ExitGame() {
        GameManager.Instance.ExitGame();
    }

    // Loads the Main Menu
    public void LoadMenu() {
        GameManager.Instance.LoadMenu();
    }
}
