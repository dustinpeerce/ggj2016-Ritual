using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Settings : MonoBehaviour {

    private Slider bgmSlider;
    private Slider sfxSlider;
    private MusicManager musicManager;
    private MenuManager menuManager;

    void Awake() {
        bgmSlider = GameObject.Find("bgmSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("sfxSlider").GetComponent<Slider>();
        musicManager = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();

        if (PlayerPrefs.HasKey("bgmVolume")) {
            bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume");
        }
        else {
            PlayerPrefs.SetFloat("bgmVolume", bgmSlider.value);
        }

        if (PlayerPrefs.HasKey("sfxVolume")) {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        else {
            PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        } 
        
        
    }

	void Start () {
        

        
	}

    public void ChangeBackgroundVolume() {
        PlayerPrefs.SetFloat("bgmVolume", bgmSlider.value);
        musicManager.UpdateVolume();
    }

    public void ChangeSoundEffectsVolume() {
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        menuManager.UpdateVolume();
    }
}
