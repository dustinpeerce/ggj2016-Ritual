using UnityEngine;
using System.Collections;

public class MovingWall : MonoBehaviour {
    bool Activated;
    public Vector3 pos1;
    public Vector3 pos2;
    public float speed = 8.0f;
    public Vector3 ActivationVector;
    public int WallType;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (WallType == 0)//generic movement
            transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);
        else {//other possible movement types.

        }
    }
}
