using UnityEngine;
using System.Collections;

public class IAmMouse : MonoBehaviour {

    private Vector3 mousePos;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        mousePos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
