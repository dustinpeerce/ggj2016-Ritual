using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance = null;

    private float volume;
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

    public void UpdateVolume() {
        volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource.volume = volume;
    }
}
