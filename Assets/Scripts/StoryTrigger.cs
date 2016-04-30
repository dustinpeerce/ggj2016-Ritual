using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryTrigger : MonoBehaviour {

    // Public Attributes
    public GameObject[] storyPanels;
    public AudioClip changePanel;
    public AudioClip audioStoryPanel;
    public AudioClip audioGame;

    // Private Attributes
    private int currentPanelIndex = 0;
    private float volume;

    void Start() {

        MusicManager.instance.SetMusic(audioStoryPanel);

        GameManager.instance.ActivateStoryPanel();

        storyPanels[0].SetActive(true);
        for (int i = 1; i < storyPanels.Length; i++) {
            storyPanels[i].SetActive(false);
        }

        foreach (GameObject panel in storyPanels) {
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2((float)Screen.height, (float)Screen.height);
        }

        volume = PlayerPrefs.GetFloat("sfxVolume");
    }

    void Update() {
        if (Input.anyKeyDown) {
            NextPanel();
        }
    }

    public void NextPanel() {

        AudioSource.PlayClipAtPoint(changePanel, Camera.main.transform.position, volume);

        if (currentPanelIndex < storyPanels.Length - 1) {
            storyPanels[currentPanelIndex].SetActive(false);

            currentPanelIndex += 1;
            storyPanels[currentPanelIndex].SetActive(true);
        }
        else {
            Invoke("ClosePanel", 0.2f);
        }
    }

    public void ClosePanel() {
        if (SceneManager.GetActiveScene().buildIndex != 2)
            MusicManager.instance.SetMusic(audioGame);

        GameManager.instance.DeactivateStoryPanel();
        this.gameObject.SetActive(false);
    }
}
