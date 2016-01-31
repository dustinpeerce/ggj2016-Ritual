using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    void Start() {
        Time.timeScale = 1.0f;
    }

    public void Play() {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void Exit() {
        Application.Quit();
    }
}
