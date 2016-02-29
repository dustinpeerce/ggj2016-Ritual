using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {
    public bool activated;
    public int attachedCandle;
    public Vector3 finalPosition;
    Vector3 startPosition;
    public int Obstacle_Type;
    public float speed = 8;
    public float timer = 0f;
    public int timerType = 1;
    // Use this for initialization

    void Start() {
        activated = false;
        speed /= 100;
        timer = 0; //<-- in case someone accidently messes with timer in unity.  Made it public to see it change.
        startPosition = transform.position;
    }

    // Update is called once per frame
    public void Activation() {
         
         timer = 0;
        
    }
    void Update() {

        if (activated) {//work on getting a better delay?

            if (timer <= 1.0f) {
                timer += Time.deltaTime * speed;
                transform.position = new Vector3(Mathf.SmoothStep(startPosition.x, finalPosition.x, timer), Mathf.SmoothStep(startPosition.y, finalPosition.y, timer), 0);
                //transform.position = Vector3.Lerp(startPosition, finalPosition, timer);
            }


            /*
            if (Obstacle_Type == 0)//generic move type
                transform.position = Vector3.Lerp(startPosition, finalPosition, (Time.time * speed));
            else {//every other type of obstacle movement for these guys goes in this part.
                if (Obstacle_Type == 1) {//current idea is to have set timers with different vars for each type, possibly.
                    //this should be functioning, but it isnt right now. uncertain as to why.
                    if (timerType == 1) {
                        if (transform.position != finalPosition)
                            transform.position = Vector3.Lerp(startPosition, finalPosition, (Time.time * speed));
                        else {
                            if (timer != 100)
                                timer += 1.0;//should be... 4 seconds?
                            else {
                                timerType *= -1;
                                timer = 0;
                            }
                        }


                    }
                    else if (timerType == -1) {
                        if (transform.position != startPosition)//for some odd reason, it starts jumping instantly between them once timer hits 100. I don't know why.
                            transform.position = Vector3.Lerp(finalPosition, startPosition, (Time.time * speed));
                        else {
                            if (timer != 100)
                                timer += 1.0;//should be... 4 seconds?
                            else {
                                timerType *= -1;
                                timer = 0;
                            }

                        }
                    }
                }
            }
             * */
        }
        else
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime * speed;
                transform.position = new Vector3(Mathf.SmoothStep(finalPosition.x, startPosition.x, 1.0f - timer), Mathf.SmoothStep(finalPosition.y, startPosition.y, 1.0f - timer), 0);
            }
        }
    }
}
