using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {

	// Public Attributes
    public float orbitTime = 5; // 5 seconds to complete a circle
    public float radius = 5;

    // Private Attributes
    private float angle = 0;
    private float speed;
    private Vector3 origin;

    void Start() {
        speed = (2 * Mathf.PI) / orbitTime;
        origin = transform.position;
    }

     void Update() {
         angle += speed*Time.deltaTime; //if you want to switch direction, use -= instead of +=
         transform.position = origin + new Vector3(Mathf.Cos(angle)*radius, Mathf.Sin(angle)*radius, transform.position.z);
     }
}
