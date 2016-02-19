using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject musicManager;
    public GameObject mainPanel;
    public GameObject settingsPanel;

    void Start() {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);

        Time.timeScale = 1.0f;

        if (GameObject.Find("MusicManager") == null) {
            Instantiate(musicManager);
        }
    }

    public void LevelSelect() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings() {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Close() {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void Exit() {
        Application.Quit();
    }
}
