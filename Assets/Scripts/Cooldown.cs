using UnityEngine;
using System.Collections;

public class Cooldown : MonoBehaviour {

    public float cooldownTime;
    public bool destroyOnTouch = false;

    private float cooldownTimer = 0.0f;
    private bool onCooldown = false;

	
	void Update () {
        if (onCooldown) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0.0f) {
                onCooldown = false;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (destroyOnTouch)
                Destroy(this.gameObject);
            else {
                onCooldown = true;
                cooldownTimer = cooldownTime;
            }
        }
    }
}
