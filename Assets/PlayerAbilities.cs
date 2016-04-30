using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

    private PointEffector2D boomBitch;
    private ParticleSystem fireAbilityParticle;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.ColorOverLifetimeModule theGrad;
    private FlickerGradients grads;
    private Player firePlayer;
    private FireBall fireBall;
    private float burstTime;
    public const float burstWait =  1;
    private PlayerHeart playerHeart;

    // Use this for initialization
    void Start () {
        playerHeart = GameObject.FindObjectOfType<PlayerHeart>();
        boomBitch = GetComponent<PointEffector2D>();
        fireAbilityParticle = GetComponent<ParticleSystem>();
        emission = fireAbilityParticle.emission;
        emission.enabled = false;
        boomBitch.enabled = false;
        firePlayer = GameObject.FindObjectOfType<Player>();
        fireBall = GameObject.FindObjectOfType<FireBall>();
        theGrad = fireAbilityParticle.colorOverLifetime;
        grads = GetComponent<FlickerGradients>();
    }
	
	// Update is called once per frame
	void Update () {
        if(firePlayer.sizeFactor > 0) 
            update();
	}

    void update() {
        transform.position = firePlayer.transform.position + Vector3.down * .9f * transform.localScale.x;
        if (particleReady) {
            if (emission.enabled) {
                emission.enabled = false;
                firePlayer.FlipCanLightSwitch();
            }
            if (Input.GetMouseButton(1) || Mathf.Abs(Input.GetAxis("PlayerAbility")) >= .2f) {
                playerHeart.TickDown();
                firePlayer.transform.position += new Vector3(.0001f, .0001f, .0001f);
                if (firePlayer.sizeFactor <= 0)
                    emission.enabled = false;
                else {
                    burstTime = Time.time;
                    emission.enabled = true;
                    theGrad.color = grads.ChangeGradient(firePlayer.CurrentTorchType);
                    firePlayer.FlipCanLightSwitch();

                    switch (firePlayer.CurrentTorchType) {
                        case Player.TorchColor.Yellow:
                            boomBitch.enabled = true;
                            break;
                        case Player.TorchColor.Blue:
                            GameManager.instance.FireballAudioPlay();
                            fireBall.FireTheFireball();
                            break;
                    }
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
