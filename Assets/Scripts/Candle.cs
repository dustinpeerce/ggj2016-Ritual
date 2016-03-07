using UnityEngine;
using System.Collections;

public class Candle : MonoBehaviour {
    //this script covers the activation of moving objects based on player colliding with candle.
    public int candleNumber;
    int currLife;
    public int candleLife;
    GameObject[] gos;
    Wheel[] wheelList;//list of items with wheel script
    MovingWall[] wallList;//lis of items with movingwall script.
    Movable[] movableList;
    Candle[] candleList;
    Fireball player;
    public bool activated;
    public Vector3 pos1;//incase we need to hard-code a position, etc.
    //public GameObject[] Obstacle_List;//initialized your array.  Cannot test it but it is there.
    public Movable[] Obstacle_List;//initialized your array.  Cannot test it but it is there.
    private GameObject flame;
    private float volume;
    public AudioClip candleFlame;

    void Start() {
        player = GameObject.FindObjectOfType<Fireball>();
        activated = false;
        flame = transform.FindChild("flame").gameObject;
        flame.SetActive(false);
        volume = PlayerPrefs.GetFloat("sfxVolume");
        currLife = 0;
    }
    public void PressureSwitch()
    {
        if (!activated)
        {
            activated = !activated;
            flame.SetActive(true);
            player.MakeSmall();
            AudioSource.PlayClipAtPoint(candleFlame, Camera.main.transform.position, volume);
            Activate();

        }
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (!activated) {
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
            if (movableList.Length != 0) {
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