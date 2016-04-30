using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance = null;

    private float volume;
    public AudioClip audioMainMenu;
    public AudioClip audioGame;
    private AudioSource audioSource;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        
        volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource.volume = volume;
    }

    void OnLevelWasLoaded(int level) {
        if (level == 1 || level == 0) {
            ResetToMainMenuMusic();
        }
        else {
            if (GameObject.Find("StoryPanel") == null) {
                SetMusic(audioGame);
            }
        }
    }

    public void UpdateVolume() {
        volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource.volume = volume;
    }

    public void SetMusic(AudioClip audio) {
        if (audioSource.clip != audio) {
            audioSource.Stop();
            audioSource.clip = audio;
            audioSource.Play();
        }
    }

    public void ResetToMainMenuMusic() {
        if (audioSource.clip != audioMainMenu) {
            audioSource.Stop();
            audioSource.clip = audioMainMenu;
            audioSource.Play();
        }
    }
}
