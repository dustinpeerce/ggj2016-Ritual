using UnityEngine;
using System.Collections;

public class EreDayBeTorching : MonoBehaviour {
    public Fireball.TorchColor colorMeHappy;
    public int TorchNumber;
    bool CanAccess;
    private GameObject flame;
    // Use this for initialization
    void Start () {
        flame = transform.FindChild("Torch").gameObject;
        flame.SetActive(true);
    }
    public void PressureSwitch()//change this to target effect.
    {
        // if (!CanAccess)//not sure if I will need this or not.
        // {
        CanAccess = !CanAccess;// candle is toggled to either accessible or locked away
        if (!CanAccess)//if it was just locked away
        {
            
            flame.SetActive(false);//turns off the candle since switch locks it away.
        }
        else
        {
            flame.SetActive(true);
        }
        //flame.SetActive(true);// we are making it accessible, not turning it on.
        //player.MakeSmall();//this will not happen because of the switch, but because of the fire's ability use.
        //AudioSource.PlayClipAtPoint(candleFlame, Camera.main.transform.position, volume);
        //Activate();

        // }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
