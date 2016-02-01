using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {


    // Private Attributes
    private float speed = 10.0f;
    private Vector3 mousePos;
    private Vector3 mouseWorldPos;
    private Vector3 objectPos;
    private float playerRotationAngle;
    private bool moving = false;
	
	void Start () {
	    
	}
	
	void Update () {

        // Calculate Rotation
        mousePos = Input.mousePosition;
        objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        playerRotationAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            moving = !moving;
        }
	}

    void FixedUpdate() {

        if (moving) {
            // Apply Rotation
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerRotationAngle));
            mouseWorldPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, mouseWorldPos, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Obstacle") {
            GameManager.instance.ActivateRetryPanel();
        }
        else if (col.gameObject.tag == "Teleporter") {
            GameManager.instance.ActivateNextLevelPanel();
        }
    }
}
