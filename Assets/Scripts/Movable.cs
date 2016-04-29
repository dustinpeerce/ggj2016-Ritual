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
    private Vector3 sign;

    void Start() {
        finalPos = transform.position;
        afterTime = int.MinValue;
        afterWait = 0;
        body = GetComponent<Rigidbody2D>();
        moveBacks = new List<Vector3>();
        aftaDistances = new List<Vector3>();
    }

    public void Activate(float speed, Vector3 distance, Vector3 aftaDistance, float afterWait, Candle resetCandle) {
        if (transform.position == distance + originalPosition)
            resetCandle.ResetActivation(true);
        else {
            if (this.resetCandle != null)
                this.resetCandle.ResetActivation(aftaDistances[aftaDistances.Count - 1] == Vector3.zero);
            this.resetCandle = resetCandle;
            this.speed = speed;
            this.afterWait = afterWait;
            this.distance = distance;
            finalPos = distance + transform.position;
            if (aftaDistance != Vector3.zero)
                moveBacks.Add(aftaDistance + finalPos);
            aftaDistances.Add(aftaDistance);
            originalPosition = transform.position;

            Debug.Log(sign = distance.normalized);


        }
    }
    //please don't change z...or you'll die.
    private bool hasChangedSign(Vector3 final, Vector3 pos) {
        bool xPass = true, yPass = true;
        Vector3 questionMe = final - pos;
        Debug.Log(questionMe);
        if (sign.x < 0) {
            if (questionMe.x > 0)
                xPass = false;
        }
        else if (questionMe.x < 0)
            xPass = false;
        else if (questionMe.x == 0)
            xPass = false;

        if (sign.y < 0) {
            if (questionMe.y > 0)
                yPass = false;
        }
        else if (questionMe.y < 0)
            yPass = false;
        else if (questionMe.y == 0)
            yPass = false;        

        //if both have flipped signs quit moving
        return xPass || yPass;
    }

    void FixedUpdate() {
        if (resetCandle != null) {
            if (!hitFirst) {
                if (hasChangedSign(finalPos,transform.position))
                    body.AddForce(distance * speed * body.mass);
                else {
                    hitFirst = true;
                    afterTime = Time.time;
                }
            }
            else if (hitSecond < moveBacks.Count) {
                if (Time.time - afterTime > afterWait)
                    if (hasChangedSign(moveBacks[moveBacks.Count - 1 - hitSecond], transform.position)) {
                        if (newCollisionEnter)
                            newCollisionEnter = false;
                        aftaStarted = true;
                        body.AddForce(aftaDistances[moveBacks.Count - 1 - hitSecond] * speed * body.mass);
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
        if(col.gameObject.tag != "Player")
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
