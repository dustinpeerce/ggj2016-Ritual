using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface ISwitchTrigger {
    void SwitchTriggger();
}

public class GameManager : MonoBehaviour {

    public enum GameState { Play, Paused, End, Story }
    private GameState gameState;

    public static GameManager instance;
    public int starTime1;
    public int starTime2;
    private int starsWon;
    public AudioClip audioLose;
    public AudioClip audioWin;
    public AudioClip audioPause;
    public AudioClip audioButtonClick;
    public AudioClip audioButtonHover;
    public AudioClip audioWater;
    public AudioClip audioTorch;
    public AudioClip audioTarget;
    public AudioClip audioFireball;
    private float volume;


    private const float deathWait = 1f;
    private float deathTime;
    private bool deathInit;

    private Player player;
    private FireBall fireBall;
    private GameObject hudPanel;
    private GameObject pausePanel;
    private GameObject retryPanel;
    private GameObject nextLevelPanel;
    private GameObject retryPanelFirstSelected;
    private GameObject nextLevelPanelFirstSelected;
    private GameObject pausePanelFirstSelected;
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

        gameState = GameState.Play;

        player = FindObjectOfType<Player>();
        hudPanel = GameObject.Find("hudPanel");
        pausePanel = GameObject.Find("PausePanel");
        pausePanelFirstSelected = pausePanel.transform.FindChild("RetryButton").gameObject;
        retryPanel = GameObject.Find("RetryPanel");
        retryPanelFirstSelected = retryPanel.transform.FindChild("RetryButton").gameObject;
        nextLevelPanel = GameObject.Find("NextLevelPanel");
        nextLevelPanelFirstSelected = nextLevelPanel.transform.FindChild("NextLevelButton").gameObject;
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
	}

    void Start() {

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
        
        if (Input.GetMouseButtonDown(0) ||
            Mathf.Abs(Input.GetAxis("Horizontal")) >= .2f || 
            Mathf.Abs(Input.GetAxis("Vertical"))   >= .2f) {
            if (!player.Moving && gameState == GameState.Play)
                player.activateMovement();
            GameManager.instance.SetTimer(true);
            if (!player.Moving && gameState == GameState.Play) {
                player.activateMovement();
                GameManager.instance.SetTimer(true);
            }
        }

        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape)) {
            if (gameState == GameState.Play)
                ActivatePausePanel();
            else if (gameState == GameState.Paused)
                Continue();
        }

        if (timerActive)
        {
            timer = Time.time - startTime;

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }

        if (deathInit && Time.time - deathTime > deathWait) {
            retryPanelDeathOrNot();
        }
    }

    public void ActivatePausePanel() {
        gameState = GameState.Paused;
        AudioSource.PlayClipAtPoint(audioPause, Camera.main.transform.position, volume);
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pausePanelFirstSelected);
        Time.timeScale = 0;
    }

    public void ActivateRetryPanel() {
        if (!deathInit && player.sizeFactor == 0) {
            deathInit = true;
            deathTime = Time.time;
        }
        else {
            retryPanelDeathOrNot();
        }
    }
    private void retryPanelDeathOrNot() {
        if (gameState != GameState.End) {
            gameState = GameState.End;
            AudioSource.PlayClipAtPoint(audioLose, Camera.main.transform.position, volume);
            retryPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(retryPanelFirstSelected);
            Time.timeScale = 0;
        }
    }

    public void ActivateNextLevelPanel() {
        if (gameState != GameState.End) {
            gameState = GameState.End;
            if (Time.time - startTime > starTime2)
                if (Time.time - startTime > starTime1)
                    starsWon = 1;
                else
                    starsWon = 2;
            else
                starsWon = 3;

            AudioSource.PlayClipAtPoint(audioWin, Camera.main.transform.position, volume);
            nextLevelPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(nextLevelPanelFirstSelected);
            Time.timeScale = 0;
        }
    }

    public void ActivateStoryPanel() {
        gameState = GameState.Story;
        hudPanel.SetActive(false);
    }

    public void DeactivateStoryPanel() {
        gameState = GameState.Play;
        hudPanel.SetActive(true);
    }

    public void Retry() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        resetTimer = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Continue() {
        gameState = GameState.Play;
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        pausePanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void NextLevel() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        resetTimer = true;

        if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LevelSelect() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
        SceneManager.LoadScene(1);
    }

    public void Exit() {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(audioButtonClick, Camera.main.transform.position, volume);
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

    public void WaterAudioPlay() {
        AudioSource.PlayClipAtPoint(audioWater, Camera.main.transform.position, volume);
    }

    public void TorchAudioPlay() {
        AudioSource.PlayClipAtPoint(audioTorch, Camera.main.transform.position, volume);
    }

    public void TargetAudioPlay() {
        AudioSource.PlayClipAtPoint(audioTarget, Camera.main.transform.position, volume);
    }

    public void FireballAudioPlay() {
        AudioSource.PlayClipAtPoint(audioFireball, Camera.main.transform.position, volume);
    }
}
