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
            transform.position = new Vector3(Mathf.SmoothStep(pos1.x, pos2.x, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f), Mathf.SmoothStep(pos1.y, pos2.y, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f), 0);
        //transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);
        else {//other possible movement types.

        }
    }
}
