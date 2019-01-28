using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField]
    // Stores the life slider
    private Slider lifeSlider;
    [SerializeField]
    // Stores the pause screen
    private GameObject pauseScreen;

    private void Update() {
        //Everyframe get the lifeslider value
        lifeSlider.value = GameManager.Instance.GetLife();
        pauseScreen.SetActive(GameManager.Instance.IsGamePaused());
    }

    // Method called to resume game
    public void ResumeGame() {
        GameManager.Instance.ResumeGame();
    }

    // Method called to call menu Scene
    public void LoadMenu() {
        GameManager.Instance.LoadMenu();
    }

}
