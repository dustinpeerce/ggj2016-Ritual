using UnityEngine;
using System.Collections;
using System;

public class WaterSwitch : MonoBehaviour, ISwitchTrigger {
    private Player player;
    bool CanAccess;
    GameObject child;
    public void SwitchTriggger() {
        if (player.CurrentTorchType == Player.TorchColor.Green &&
            player.CanLight) {

            CanAccess = !CanAccess;// candle is toggled to either accessible or locked away
            if (!CanAccess)//if it was just locked away
            {
                child.SetActive(false);//turns off the candle since switch locks it away.
            }
            else {
                child.SetActive(true);
            }
        }
    }

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
        child = transform.FindChild("Agua").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
