﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public int starTime1;
    public int starTime2;
    private int starsWon;
    public AudioClip audioLose;
    public AudioClip audioWin;
    public AudioClip audioPause;
    public AudioClip audioButtonClick;
    public AudioClip audioButtonHover;
    private float volume;
    private Fireball fire;
    private GameObject pausePanel;
    private GameObject retryPanel;
    private GameObject nextLevelPanel;
    private float timer,startTime;
    private bool timerActive,resetTimer;
    private Text timerText;
    private Text levelText;

	void Awake () {
        resetTimer = true;
        if (instance == null) {
            instance = this;
        }
        else {
            DestroyImmediate(gameObject);
            return;
        }
        
	}

    void Start() {
        fire = GameObject.FindGameObjectWithTag("Player").GetComponent<Fireball>();
        pausePanel = GameObject.Find("PausePanel");
        retryPanel = GameObject.Find("RetryPanel");
        nextLevelPanel = GameObject.Find("NextLevelPanel");
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        levelText = GameObject.Find("LevelText").GetComponent<Text>();


        string sceneName = SceneManager.GetActiveScene().name;
        sceneName = sceneName.Replace("_", " ");
        levelText.text = sceneName;
        timerText.text = "";
        pausePanel.SetActive(false);
        retryPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        Time.timeScale = 1.0f;
        volume = PlayerPrefs.GetFloat("sfxVolume");
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            ActivatePausePanel();
        }
        if (timerActive)
        {
            timer = Time.time - startTime;

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void ActivatePausePanel() {
        AudioSource.PlayClipAtPoint(audioPause, Camera.main.transform.position, volume);
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
        if (Time.timeScale == 0) 
            Time.timeScale = 1.0f;
        else 
            Time.timeScale = 0;
    }

    public void ActivateRetryPanel() {
        AudioSource.PlayClipAtPoint(audioLose, Camera.main.transform.position, volume);
        Time.timeScale = 0;
        retryPanel.SetActive(!retryPanel.activeInHierarchy);
    }

    public void ActivateNextLevelPanel() {
        if (Time.time - startTime > starTime2)
            if (Time.time - startTime > starTime1)
                starsWon = 1;
            else
                starsWon = 2;
        else
            starsWon = 3;
        AudioSource.PlayClipAtPoint(audioWin, Camera.main.transform.position, volume);
        Time.timeScale = 0;
        nextLevelPanel.SetActive(!nextLevelPanel.activeInHierarchy);
    }

    public void Retry() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        Time.timeScale = 0;
        resetTimer = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Continue() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        Time.timeScale = 0;
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void NextLevel() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        Time.timeScale = 0;
        resetTimer = true;
        if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCount)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PauseExit() {
        fire.activateMovement();
        ActivatePausePanel();
    }

    public void LevelSelect() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        Time.timeScale = 0;
        SceneManager.LoadScene(1);
    }

    public void Exit() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        Time.timeScale = 0;
        SceneManager.LoadScene(0);
    }

    public void SetTimer(bool active) {
        timerActive = active;
        if (resetTimer)
        {
            startTime = Time.time;
            resetTimer = false;
        }
    }
    public void ResetTimer()
    {
        resetTimer = true;
    }

    public void PointerEnter() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonHover, Camera.main.transform.position, volume);
        Time.timeScale = 0;
    }
}