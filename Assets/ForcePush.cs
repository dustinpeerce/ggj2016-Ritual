using UnityEngine;
using System.Collections;

public class ForcePush : MonoBehaviour {

    private PointEffector2D boomBitch;
    private ParticleSystem firePushParticle;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.ColorOverLifetimeModule theGrad;
    private FlickerGradients grads;
    private Player firePlayer;
    private FireBall fireBall;
    private float burstTime;
    public const float burstWait =  1;

	// Use this for initialization
	void Start () {
        boomBitch = GetComponent<PointEffector2D>();
        firePushParticle = GetComponent<ParticleSystem>();
        emission = firePushParticle.emission;
        emission.enabled = false;
        boomBitch.enabled = false;
        firePlayer = GameObject.FindObjectOfType<Player>();
        fireBall = GameObject.FindObjectOfType<FireBall>();
        theGrad = firePushParticle.colorOverLifetime;
        grads = GetComponent<FlickerGradients>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = firePlayer.transform.position;    
        if (particleReady) {
            if (emission.enabled) {
                emission.enabled = false;
                firePlayer.FlipCanLightSwitch();
            }
            if (Input.GetMouseButton(1)) {
                burstTime = Time.time;
                emission.enabled = true;
                theGrad.color = grads.ChangeGradient(firePlayer.CurrentTorchType);
                firePlayer.FlipCanLightSwitch();

                switch (firePlayer.CurrentTorchType) {
                    case Player.TorchColor.Yellow:
                        boomBitch.enabled = true;
                        break;
                    case Player.TorchColor.Blue:
                        fireBall.FireTheFireball();
                        break;
                }
                
            }
        }

	}


    private bool particleReady {
        get { return Time.time - burstTime > burstWait; }
    }
    void OnTriggerEnter2D(Collider2D col) {
        
    }
}
