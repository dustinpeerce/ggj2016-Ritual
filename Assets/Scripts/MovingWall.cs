using UnityEngine;
using System.Collections;

public class MovingWall : MonoBehaviour {

    public Vector3 pos1;
    public Vector3 pos2;
    public float speed = 8.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);
	}
}
