using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance = null;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
