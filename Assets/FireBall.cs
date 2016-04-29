using UnityEngine;

public class FireBall : MonoBehaviour {
    public float Life;

    private float depLife;
    private bool followPlayer;
    private Player player;
    private ParticleSystem fireTail;
    Vector3 path,origScale;
    private float expirationTime;
    public const float expirationWait = .5f;
    private GameObject[] fireObjects;
    private ParticleSystem[] particles;

    // Use this for initialization
    void Start () {
        this.enabled = false;
        followPlayer = true;
        player = GameObject.FindObjectOfType<Player>();
        fireTail = GameObject.Find("FireBallFireTail").GetComponent<ParticleSystem>();
        origScale = transform.localScale;
        particles = GetComponentsInChildren<ParticleSystem>();
        setScale(Vector3.zero,true);
    }
	
    private void setScale(Vector3 trans, bool followPlayer = false) {
        transform.localScale = trans;
        if (followPlayer) {
            foreach (Transform t in transform)
                t.localScale = trans + new Vector3(0,.9f,0);

        }
    }

	void Update () {
        //scaler for the transform to make sure the fireball just doesn't magically disappear
        float scaler = 0;

        if (!followPlayer)
            //if we haven't traveled that many frames...
            if (depLife < Life) {
                transform.position += path;
                depLife++;
                foreach (ParticleSystem ps in particles) {
                    ps.Emit(1);
                }
                scaler = (Life - depLife + 2) / Life * 2;
            }
            //play the particle effects til it dies..
            else if (Time.time - (expirationTime + Life / 60) > expirationWait) {
                followPlayer = true;
                Debug.Log(transform.position);
            }
            //i dont feel like using braces and i might do something with this later
            else {

            }

        setScale(origScale * scaler,followPlayer);
    }

    public void FireTheFireball() {
        depLife = 0;
        this.enabled = true;
        transform.position = player.transform.position - new Vector3(0,player.sizeFactor,0);
        transform.rotation = Quaternion.Euler(0, 0, player.ParticleRotation + 90);
        fireTail.startRotation = player.ParticleRotation;
        
        followPlayer = false;
        setScale(origScale,true);

        float x = -Mathf.Cos(Mathf.Deg2Rad * (90 + transform.rotation.eulerAngles.z));// * Math.cos(yaw);
        float y = -Mathf.Sin(Mathf.Deg2Rad * (90 + transform.rotation.eulerAngles.z));// * Math.sin(yaw);
        path = new Vector3(x,y,0);

        expirationTime = Time.time;

        Debug.Log(transform.position + " _ " + path);

    }
}
