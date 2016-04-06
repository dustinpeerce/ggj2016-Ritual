using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {
    private bool followPlayer;
    private Player player;
    private ParticleSystem fireTail;
    Vector3 path,origScale;
    private float expirationTime;
    public const float expirationWait = .5f;
    private GameObject[] fireObjects;

    // Use this for initialization
    void Start () {
        this.enabled = false;
        followPlayer = true;
        player = GameObject.FindObjectOfType<Player>();
        fireTail = GameObject.Find("FireBallFireTail").GetComponent<ParticleSystem>();
        origScale = transform.localScale;
        fireObjects = GameObject.FindGameObjectsWithTag("FireBall");
        foreach (GameObject g in fireObjects) {
            g.transform.localScale = Vector3.zero;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (followPlayer) {

        }
        else {
            transform.position += path;
            if (Time.time - expirationTime > expirationWait) {
                if (transform.localScale.x >= 0.2f)
                    foreach (GameObject g in fireObjects) {
                        g.transform.localScale *= .9999f;
                    }
                else {
                    followPlayer = true;
                }
            }
        }

    }
    public void FireTheFireball() {
        this.enabled = true;
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
        fireTail.startRotation = player.ParticleRotation;
        followPlayer = false;
        foreach(GameObject g in fireObjects) {
            g.transform.localScale = origScale;
        }
        float x = -Mathf.Cos(Mathf.Deg2Rad * (90 + transform.rotation.eulerAngles.z));// * Math.cos(yaw);
        float y = -Mathf.Sin(Mathf.Deg2Rad * (90 + transform.rotation.eulerAngles.z));// * Math.sin(yaw);
        path = new Vector3(x,y,0);

        expirationTime = Time.time;
    }
}
