using UnityEngine;
using System.Collections.Generic;

public class Movable : MonoBehaviour {

    private Vector3 moveBack;
    private float speed, afterWait, afterTime;
    private Vector3 finalPos;
    private Vector3 distance;
    private List<Vector3> aftaDistances;
    private Vector3 originalPosition;
    private List<Vector3> moveBacks;
    private Candle resetCandle;
    private bool cantActivateAgain;
    private Rigidbody2D body;
    private bool hitFirst;
    private int hitSecond;

    private bool aftaStarted;
    private bool newCollisionEnter;
    private const float collisionWait = .1f;
    private float collisionTime;


    void Start() {
        finalPos = transform.position;
        afterTime = int.MinValue;
        afterWait = 0;
        body = GetComponent<Rigidbody2D>();
        moveBacks = new List<Vector3>();
        aftaDistances = new List<Vector3>();
    }

    public void Activate(float speed, Vector3 distance, Vector3 aftaDistance, float afterWait, Candle resetCandle) {
        if (transform.position == distance+originalPosition)
            resetCandle.ResetActivation(true);
        else {
            if (this.resetCandle != null)
                this.resetCandle.ResetActivation(aftaDistances[aftaDistances.Count - 1] == Vector3.zero);
            this.resetCandle = resetCandle;
            this.speed = speed;
            this.afterWait = afterWait;
            this.distance = distance;
            finalPos = distance + transform.position;
            if(aftaDistance!=Vector3.zero)
                moveBacks.Add(aftaDistance);
            aftaDistances.Add(aftaDistance);
            originalPosition = transform.position;
        }
    }

    void Update() {
        if (resetCandle != null) {
            if (!hitFirst)
                if (Vector3.Distance(finalPos, transform.position) >= .1f)
                    body.AddForce(distance * speed * body.mass);
                else {
                    hitFirst = true;
                    afterTime = Time.time;
                }
            else if (hitSecond < moveBacks.Count) {
                if (Time.time - afterTime > afterWait)
                    if (Vector3.Distance(moveBacks[moveBacks.Count - 1 - hitSecond], transform.position) >= .1f) {
                        aftaStarted = true;
                        body.AddForce(moveBacks[moveBacks.Count - 1 - hitSecond] * speed * body.mass);
                    }
                    else
                        hitSecond++;
            }
            else 
                reset(aftaDistances.Count > 1);
            
        body.velocity = Vector2.zero;
        }
    }

    private void reset(bool moreThanOneAfta = false) {
        hitFirst = false;
        hitSecond = 0;
        resetCandle.ResetActivation(aftaDistances[aftaDistances.Count - 1] == Vector3.zero);
        resetCandle = null;
        if(!moreThanOneAfta)
            if (aftaDistances[aftaDistances.Count - 1] != Vector3.zero)
                transform.position = originalPosition;
            else
                transform.position = finalPos;
        aftaDistances.Clear();
        moveBacks.Clear();
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (aftaStarted) {
            collisionTime = Time.time;
            newCollisionEnter = true;
        }
    }
    void OnCollisionStay2D(Collision2D col) {
        if(newCollisionEnter)
            if (Time.time - collisionTime > collisionWait) {
            collisionTime = Time.time;
            hitSecond++;
            newCollisionEnter = false;
        }
    }
    
    //We might implement this...we'll see
    //void OnCollisionEnter2D(Collision2D col) {
    //    Debug.Log("Did i hit it?");
    //    if(resetCandle!=null)
    //        if(col.gameObject.tag == "Block") 
    //            reset(true);
    //}

}
