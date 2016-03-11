using UnityEngine;
using System.Collections;

public class TargetSwitchScript : MonoBehaviour {
    public int Light_Target;//the numbers can be used twice since targets will never check torch list, vice versa.
    Candle[] candleList;
    EreDayBeTorching[] torches;
    public int obstacleType;//this will later be changed based on whether the object has the specific tag, etc.
    // Use this for initialization
    void Start () {
        
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (obstacleType == 1)//pressure_switch
        {
            if(col.gameObject.tag == "SwitchBlock")//will need a way to check when it exits as well. Would be nice if a single function covered both.
            {
                
                torches = GameObject.FindObjectsOfType(typeof(EreDayBeTorching)) as EreDayBeTorching[];
                foreach (EreDayBeTorching light in torches)
                {
                    if(light.TorchNumber == Light_Target)
                    {
                        light.PressureSwitch();

                    }
                    
                }
            }
        }
        else if(obstacleType == 0)//target
        {
            if(col.gameObject.tag == "FireBullet")//temp name, obviously
            {
                candleList = GameObject.FindObjectsOfType(typeof(Candle)) as Candle[];
                foreach (Candle light in candleList)
                {
                    if (light.candleNumber == Light_Target)
                    {
                        light.TargetSwitch();

                    }

                }
            }
        }
    }
        // Update is called once per frame
        void Update () {
	
	}
}
