using UnityEngine;
using System.Collections;

public class Candle : MonoBehaviour {
    //this script covers the activation of moving objects based on player colliding with candle.
    public int candleNumber;
    GameObject[] gos;
    Wheel[] wheelList;//list of items with wheel script
    MovingWall[] wallList;//lis of items with movingwall script.
    Movable[] movableList;
    Candle[] candleList;
    bool activated;
    public Vector3 pos1;//incase we need to hard-code a position, etc.
    public GameObject[] Obstacle_List;//initialized your array.  Cannot test it but it is there.
    void Start() {
        activated = false;
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (!activated) {
                activated = !activated;
                Activate();

            }

        }
    }
    void Activate()//the activation function that starts everything up.
    {
        if (activated) {

            //this will get replaced with a public array later.
            movableList = GameObject.FindObjectsOfType(typeof(Movable)) as Movable[];
            if (movableList.Length != 0) {
                foreach (Movable wall in movableList) {
                    //wall.gameObject;
                    if (wall.attachedCandle == candleNumber) {
                        //wall.activated = true;//make this call an actual function.
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

    }
}