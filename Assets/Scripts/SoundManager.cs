using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    // Used to create encapsulation in the singleton pattern
    private static SoundManager instance;

    [SerializeField]
    // Stores the music source of our project
    private AudioSource musicSource;
    [SerializeField]
    // Stores a global sfx source to our project
    private AudioSource sfxSource;
    [SerializeField]
    // Stores a global enemies sfx source to our project
    private AudioSource enemySfxSource;

    // Return the instance
    public static SoundManager Instance {
        get {
            return instance;
        }
    }

    // Method called in the component's awake
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(instance);
        }
        DontDestroyOnLoad(instance);
    }

    private void Update() {
        enemySfxSource.volume = sfxSource.volume;
    }

    // Play a audio clip in the SFX source
    public void PlaySFX(AudioClip clip) {
        sfxSource.Stop();
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    // Play a audio clip in the SFX source
    public void PlayEnemySFX(AudioClip clip) {
        enemySfxSource.Stop();
        enemySfxSource.clip = clip;
        enemySfxSource.Play();
    }

    // Gets the actual music volume
    public float GetMusicVolume() {
        return musicSource.volume;
    }

    // Sets a new volume to the music source
    public void SetMusicVolume(float newVolume) {
        musicSource.volume = newVolume;
    }

    // Gets the actual SFX volume
    public float GetSFXVolume() {
        return sfxSource.volume;
    }

    // Sets a new volume to the SFX source
    public void SetSFXVolume(float newVolume) {
        sfxSource.volume = newVolume;
    }
}
