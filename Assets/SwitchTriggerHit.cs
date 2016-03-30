using UnityEngine;
using System.Collections;

public class SwitchTriggerHit : MonoBehaviour {
    private EdgeCollider2D switchFloor;
    private bool switchEnabled;

    public bool SwitchEnabled {
        get { return switchEnabled; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }
    
    void OnTriggerEnter2D(Collider2D col) {
        if (col.name == "PurpleBlock") {
            switchFloor = GetComponentsInChildren<EdgeCollider2D>()[1];
            switchEnabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (col.name == "PurpleBlock") {
            switchFloor.enabled = true;
        }
    }
}
