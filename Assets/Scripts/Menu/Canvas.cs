using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {

    [SerializeField]
    // Stores the main menu screen
    private GameObject menuScreen;
    [SerializeField]
    // Stores the audio menu screen
    private GameObject audioMenuScreen;
    [SerializeField]
    // Slider that will be storing the music volume
    private Slider musicVolume;
    [SerializeField]
    // Slider that will be storing the sfx volume
    private Slider sfxVolume;

    private void Start() {
        // When start get the volume from music and sfx
        musicVolume.value = SoundManager.Instance.GetMusicVolume();
        sfxVolume.value = SoundManager.Instance.GetSFXVolume();
        // Add a listener to our sliders
        musicVolume.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        sfxVolume.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });
    }

    // Call method to Start the game
    public void StartGame() {
        GameManager.Instance.StartGame();
    }

    // Call method to close the game
    public void ExitGame() {
        GameManager.Instance.ExitGame();
    }

    public void LoadMenu() {
        menuScreen.SetActive(true);
        audioMenuScreen.SetActive(false);
    }

    public void LoadAudioOptions() {
        menuScreen.SetActive(false);
        audioMenuScreen.SetActive(true);
    }

    // Listener that will change the music volume
    public void OnMusicVolumeChanged() {
        SoundManager.Instance.SetMusicVolume(musicVolume.value);
    }

    // Listener that will change the sfx volume
    public void OnSfxVolumeChanged() {
        SoundManager.Instance.SetSFXVolume(sfxVolume.value);
    }

}
