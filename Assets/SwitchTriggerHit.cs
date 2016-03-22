using UnityEngine;
using System.Collections;

public class SwitchTriggerHit : MonoBehaviour {
    private EdgeCollider2D switchFloor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }
    
    void OnTriggerEnter2D(Collider2D col) {
        if (col.name == "SwitchTrigger") {
            switchFloor = col.gameObject.GetComponentsInChildren<EdgeCollider2D>()[1];
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (col.name == "SwitchTrigger") {
            switchFloor.enabled = true;
        }
    }
}
