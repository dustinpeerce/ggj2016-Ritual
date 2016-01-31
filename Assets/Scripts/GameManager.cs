using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject pausePanel;
    public GameObject retryPanel;
    public GameObject nextLevelPanel;

	
	void Awake () {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
	}

    void Start() {
        pausePanel.SetActive(false);
        retryPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            ActivatePausePanel();
        }
    }

    public void ActivatePausePanel() {
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
        if (Time.timeScale == 0) 
            Time.timeScale = 1.0f;
        else 
            Time.timeScale = 0;
    }

    public void ActivateRetryPanel() {
        Time.timeScale = 0;
        retryPanel.SetActive(!retryPanel.activeInHierarchy);
    }

    public void ActivateNextLevelPanel() {
        Time.timeScale = 0;
        nextLevelPanel.SetActive(!nextLevelPanel.activeInHierarchy);
    }

    public void Retry() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Continue() {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void NextLevel() {
        if (Application.loadedLevel + 1 == Application.levelCount)
            Application.LoadLevel(0);
        else
            Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void Exit() {
        Application.LoadLevel(0);
    }
}
