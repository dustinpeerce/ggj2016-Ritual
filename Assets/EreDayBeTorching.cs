using UnityEngine;
using System.Collections;
using System;


public class EreDayBeTorching : MonoBehaviour, ISwitchTrigger {
    public Player.TorchColor colorMeHappy;
    public int TorchNumber;
    bool CanAccess;
    
    private Player player;

    public void SwitchTriggger() {
        gameObject.SetActive((CanAccess = !CanAccess));

        //flame.SetActive(true);// we are making it accessible, not turning it on.
        //player.MakeSmall();//this will not happen because of the switch, but because of the fire's ability use.
        //AudioSource.PlayClipAtPoint(candleFlame, Camera.main.transform.position, volume);
        //Activate();

        // }
    }

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
