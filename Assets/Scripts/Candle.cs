using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Candle : MonoBehaviour {
    //this script covers the activation of moving objects based on player colliding with candle.
    public int candleNumber;
    int currLife;
    public int candleLife;
    GameObject[] gos;
    Wheel[] wheelList;//list of items with wheel script
    MovingWall[] wallList;//lis of items with movingwall script.
    List<Movable> movableList;
    Candle[] candleList;
    Fireball player;
    public bool CanAccess;
    public bool activated;
    public Vector3 pos1;//incase we need to hard-code a position, etc.
    //public GameObject[] Obstacle_List;//initialized your array.  Cannot test it but it is there.
    public Movable[] Obstacle_List;//initialized your array.  Cannot test it but it is there.
    private GameObject flame;
    private float volume;
    public AudioClip candleFlame;

    void Start() {

        player = GameObject.FindObjectOfType<Fireball>();
        //activated = false;
        flame = transform.FindChild("flame").gameObject;
		if(activated)
		{
			currLife = candleLife;
			flame.SetActive(true);
            if (Obstacle_List.Length != 0)//THIS WILL BREAK IF SOMETHING IN IT DOES NOT HAVE THE SCRIPT.
            {
                foreach (Movable wall in Obstacle_List)
                {
                    if (wall.attachedCandle == candleNumber)
                    {
                        //wall.activated = true;//make this call an actual function.
                        wall.activated = true;
                        wall.Activation();
                    }
                    else {
                        wall.activated = false;
                    }
                }
            }
        }
		else
			flame.SetActive(false);
        volume = PlayerPrefs.GetFloat("sfxVolume");
        currLife = 0;
        movableList = new List<Movable>();
    }
    public void TargetSwitch()//change this to target effect.
    {
       // if (!CanAccess)//not sure if I will need this or not.
       // {
            CanAccess = !CanAccess;// candle is toggled to either accessible or locked away
            if(activated && !CanAccess)//if it was just locked away
            {
                activated = !activated;//lit candle was just locked away in ice... or something. no longer active.
                flame.SetActive(false);//turns off the candle since switch locks it away.
                candleLife = 0;//just in case.
            }
            //flame.SetActive(true);// we are making it accessible, not turning it on.
            //player.MakeSmall();//this will not happen because of the switch, but because of the fire's ability use.
            //AudioSource.PlayClipAtPoint(candleFlame, Camera.main.transform.position, volume);
            //Activate();

       // }
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (!activated && CanAccess) {
                activated = !activated;
                flame.SetActive(true);
                player.MakeSmall();
                AudioSource.PlayClipAtPoint(candleFlame, Camera.main.transform.position, volume);
                Activate();

            }

        }
    }
    void Activate()//the activation function that starts everything up.
    {
        if (activated) {
            currLife = candleLife;
            //this will get replaced with a public array later.
            if(Obstacle_List.Length != 0)//THIS WILL BREAK IF SOMETHING IN IT DOES NOT HAVE THE SCRIPT.
            {
                foreach(Movable wall in Obstacle_List)
                {
                    if (wall.attachedCandle == candleNumber)
                    {
                        //wall.activated = true;//make this call an actual function.
                        wall.activated = true;
                        wall.Activation();
                    }
                    else {
                        wall.activated = false;
                    }
                }
            }
            //could also grab an array of all candles using candlescript as the type in order to turn all other candles besides this one off.
            candleList = GameObject.FindObjectsOfType(typeof(Candle)) as Candle[];
            if (movableList.Count != 0) {
                foreach (Candle light in candleList) {
                    //wall.gameObject;
                    if (light.candleNumber != candleNumber) {
                        light.activated = false;
                        light.Activate();
                        //can access whatever it is that turns the light on and off here
                        //uncertain how to do that right now.  Given that you managed to animate the fireball, I am assuming you have a good idea on how to do this.

                        //can add a call here that accesses activate of the other arrays and setup something that checks if activated is true.
                    }
                }
            }
        }
        else//Other Candle is calling this candle.  What do you want this candle to do?
        {
            if (Obstacle_List.Length != 0)//THIS WILL BREAK IF SOMETHING IN IT DOES NOT HAVE THE SCRIPT.
            {
                foreach (Movable wall in Obstacle_List)
                {
                    if (wall.attachedCandle == candleNumber)
                    {
                        //wall.activated = true;//make this call an actual function.
                        wall.activated = false;
                    }
                }
            }
            
            //set up the case of it being called when activated is still false (other candle calls it to turn off its objects
            //code below is example but it is redundant here since the normal call will turn off everything else anyways.
            /*
            Moveable = GameObject.FindObjectsOfType(typeof(MoveableScript)) as MoveableScript[];
            if (Moveable.Length != 0)
            {
                foreach (MoveableScript wall in Moveable)
                {
                    //wall.gameObject;
                    if (wall.AttachedCandle == CandleNumber)
                    {
                        wall.Activated = false;//make this call an actual function.
                        wall.Activation();
                    }
                }
            }

            */
        }
    }
    // Update is called once per frame
    void Update() {
        if(activated)
        {
            if (currLife > 0)
                currLife--;
            else
            {
                activated = !activated;
                flame.SetActive(false);
                Activate();
            }
                
        }
          

    }
}