using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {

    public GameObject musicManager;
    public GameObject mainPanel;
    public GameObject mainPanelFirstSelected;
    public GameObject settingsPanel;
    public GameObject settingsPanelFirstSelected;
    public AudioClip buttonClick;
    public AudioClip buttonHover;
    public AudioClip sliderDrop;

    private float volume;

    void Awake() {
        if (GameObject.Find("MusicManager") == null) {
            Instantiate(musicManager);
        }
    }

    void Start() {
        mainPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainPanelFirstSelected);
        settingsPanel.SetActive(false);

        Time.timeScale = 1.0f;

        volume = 1.0f;
    }

    public void SliderDrop() {
        AudioSource.PlayClipAtPoint(sliderDrop, Camera.main.transform.position, volume);
    }

    public void PointerEnter() {
        AudioSource.PlayClipAtPoint(buttonHover, Camera.main.transform.position, volume);
    }

    public void LevelSelect() {
        AudioSource.PlayClipAtPoint(buttonClick, Camera.main.transform.position, volume);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings() {
        AudioSource.PlayClipAtPoint(buttonClick, Camera.main.transform.position, volume);
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsPanelFirstSelected);
    }

    public void Close() {
        AudioSource.PlayClipAtPoint(buttonClick, Camera.main.transform.position, volume);
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainPanelFirstSelected);
    }

    public void Exit() {
        AudioSource.PlayClipAtPoint(buttonClick, Camera.main.transform.position, volume);
        Application.Quit();
    }

    public void UpdateVolume() {
        volume = PlayerPrefs.GetFloat("sfxVolume");
    }
}
