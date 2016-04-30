using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelectManager : MonoBehaviour {

    // Public Attributes
    public GameObject[] worldPanels;
    public AudioClip buttonClick;
    public AudioClip buttonHover;
    public AudioClip changePage;
    
    // Private Attributes
    private GameObject[] worldPanelsFirstSelected;
    private int currentPanelIndex = 0;
    private float volume;

	void Start () {
        worldPanels[0].SetActive(true);
        worldPanelsFirstSelected = new GameObject[worldPanels.Length];
        worldPanelsFirstSelected[0] = worldPanels[0].transform.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(worldPanelsFirstSelected[currentPanelIndex]);
        for (int i = 1; i < worldPanels.Length; i++) {
            worldPanelsFirstSelected[i] = worldPanels[i].transform.GetChild(0).gameObject;
            worldPanels[i].SetActive(false);
        }

        volume = PlayerPrefs.GetFloat("sfxVolume");
	}

    public void NextPanel() {

        AudioSource.PlayClipAtPoint(changePage, Camera.main.transform.position, volume);

        if (currentPanelIndex < worldPanels.Length - 1) {
            worldPanels[currentPanelIndex].SetActive(false);

            currentPanelIndex += 1;
            worldPanels[currentPanelIndex].SetActive(true);
        }
    }

    public void PreviousPanel() {

        AudioSource.PlayClipAtPoint(changePage, Camera.main.transform.position, volume);

        if (currentPanelIndex > 0) {
            worldPanels[currentPanelIndex].SetActive(false);

            currentPanelIndex -= 1;
            worldPanels[currentPanelIndex].SetActive(true);
        }
    }

    public void LoadLevel(string levelName) {

        AudioSource.PlayClipAtPoint(buttonClick, Camera.main.transform.position, volume);
        SceneManager.LoadScene(levelName);
    }

    public void MainMenu() {

        AudioSource.PlayClipAtPoint(buttonClick, Camera.main.transform.position, volume);
        SceneManager.LoadScene(0);
    }

    public void PointerEnter() {
        AudioSource.PlayClipAtPoint(buttonHover, Camera.main.transform.position, volume);
    }
	
}
