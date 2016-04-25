using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {

    private Vector3 moveBack;
    private float speed;
    private Vector3 finalPos;

    void Start() {
        finalPos = transform.position;
    }

    public void Activate(float speed, Vector3 distance, Vector3 moveBack) {
        this.speed = speed;
        this.moveBack = moveBack + distance + transform.position;
        finalPos = distance + transform.position;
    }

    void Update() {
        if (finalPos != transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, finalPos, speed / 60);
        }
        else if(moveBack != transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, moveBack, speed / 60);
            finalPos = transform.position;
        }
            
    }
}
