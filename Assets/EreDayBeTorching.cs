using UnityEngine;
using System.Collections;
using System;


public class EreDayBeTorching : MonoBehaviour, ISwitchTrigger {
    public Player.TorchColor colorMeHappy;
    public int TorchNumber;
    bool CanAccess;
    private GameObject flame;
    private Player player;

    public void SwitchTriggger() {
        // if (!CanAccess)//not sure if I will need this or not.
        // {
        if (player.CurrentTorchType == Player.TorchColor.Green &&
            player.CanLight) {
            CanAccess = !CanAccess;// candle is toggled to either accessible or locked away
            if (!CanAccess)//if it was just locked away
            {
                flame.SetActive(false);//turns off the candle since switch locks it away.
            }
            else {
                flame.SetActive(true);
            }
        }
        //flame.SetActive(true);// we are making it accessible, not turning it on.
        //player.MakeSmall();//this will not happen because of the switch, but because of the fire's ability use.
        //AudioSource.PlayClipAtPoint(candleFlame, Camera.main.transform.position, volume);
        //Activate();

        // }
    }

    // Use this for initialization
    void Start () {
        flame = transform.FindChild("Torch").gameObject;
        flame.SetActive(true);
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
