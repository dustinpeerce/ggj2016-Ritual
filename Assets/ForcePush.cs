using UnityEngine;
using System.Collections;

public class ForcePush : MonoBehaviour {

    private PointEffector2D boomBitch;
    private ParticleSystem firePushParticle;
    private ParticleSystem.EmissionModule emission;
    private Fireball fiyaaaa;
    private float burstTime;
    public const float burstWait = .25f;

	// Use this for initialization
	void Start () {
        boomBitch = GetComponent<PointEffector2D>();
        firePushParticle = GetComponent<ParticleSystem>();
        emission = firePushParticle.emission;
        emission.enabled = false;
        fiyaaaa = GameObject.FindObjectOfType<Fireball>();
    }
	
	// Update is called once per frame
	void Update () {
        if (fiyaaaa.CurrentTorchType == Fireball.TorchColor.Yellow) {
            if (particleReady) {
                if(emission.enabled)
                    emission.enabled = false;
                if (boomBitch.enabled = Input.GetKeyDown(KeyCode.A)) 
                    burstTime = Time.time;
            }
            else if(burstTime != 0)
                emission.enabled = true;
        }

	}

    private bool particleReady {
        get { return Time.time - burstTime > burstWait; }
    }
    void OnTriggerEnter2D(Collider2D col) {
        
    }
}
