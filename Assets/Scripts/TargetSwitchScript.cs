using UnityEngine;
using System.Collections;

public class TargetSwitchScript : MonoBehaviour {
    public int Candle_Target;
    Candle[] candleList;
    public int obstacleType;//this will later be changed based on whether the object has the specific tag, etc.
    // Use this for initialization
    void Start () {
        
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(obstacleType == 1)//pressure_switch
            {
                candleList = GameObject.FindObjectsOfType(typeof(Candle)) as Candle[];
                foreach (Candle light in candleList)
                {
                    if(light.candleNumber == Candle_Target)
                    {
                        light.PressureSwitch();

                    }
                    
                }
            }
        }
    }
        // Update is called once per frame
        void Update () {
	
	}
}
