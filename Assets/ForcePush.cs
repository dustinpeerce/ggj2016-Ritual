using UnityEngine;
using System.Collections;

public class ForcePush : MonoBehaviour {

    private PointEffector2D boomBitch;
    private ParticleSystem firePushParticle;
    private Fireball fiyaaaa;

	// Use this for initialization
	void Start () {
        boomBitch = GetComponent<PointEffector2D>();
        firePushParticle = GetComponent<ParticleSystem>();
        fiyaaaa = GameObject.FindObjectOfType<Fireball>();
    }
	
	// Update is called once per frame
	void Update () {
        if (firePushParticle.time == 1) {

            bool fireMe = boomBitch.enabled = Input.GetKeyDown(KeyCode.Mouse0);

            if (fireMe) {
                firePushParticle.time = 0;

                firePushParticle.Emit(50);
            }

        }

	}
    void OnTriggerEnter2D(Collider2D col) {
        
    }
}
