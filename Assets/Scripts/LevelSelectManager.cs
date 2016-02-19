using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour {

    // Public Attributes
    public GameObject[] worldPanels;
    
    // Private Attributes
    private int currentPanelIndex = 0;

	void Start () {
        worldPanels[0].SetActive(true);
        for (int i = 1; i < worldPanels.Length; i++) {
            worldPanels[i].SetActive(false);
        }
	}

    public void NextPanel() {
        if (currentPanelIndex < worldPanels.Length - 1) {
            worldPanels[currentPanelIndex].SetActive(false);

            currentPanelIndex += 1;
            worldPanels[currentPanelIndex].SetActive(true);
        }
    }

    public void PreviousPanel() {
        if (currentPanelIndex > 0) {
            worldPanels[currentPanelIndex].SetActive(false);

            currentPanelIndex -= 1;
            worldPanels[currentPanelIndex].SetActive(true);
        }
    }

    public void LoadLevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
	
}
