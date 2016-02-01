using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject musicManager;

    void Start() {
        Time.timeScale = 1.0f;

        if (GameObject.Find("MusicManager") == null) {
            Instantiate(musicManager);
        }
    }

    public void Play() {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void Exit() {
        Application.Quit();
    }
}
