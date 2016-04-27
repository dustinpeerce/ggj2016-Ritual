using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {

    private Vector3 moveBack;
    private float speed, afterWait, afterTime;
    private Vector3 finalPos;
    private Vector3 distance;
    private Vector3 aftaDistance;
    private Vector3 originalPosition;
    private Candle resetCandle;
    private bool cantActivateAgain;
    private Rigidbody2D body;
    private bool hitFirst,hitSecond;

    void Start() {
        finalPos = transform.position;
        afterTime = int.MinValue;
        afterWait = 0;
        body = GetComponent<Rigidbody2D>();
    }

    public void Activate(float speed, Vector3 distance, Vector3 moveBack, float afterWait, Candle resetCandle) {
        if (transform.position == distance+originalPosition)
            resetCandle.ResetActivation();
        else {
            this.speed = speed;
            this.finalPos = distance + transform.position;
            this.moveBack = moveBack + distance + transform.position;
            this.afterWait = afterWait;
            this.distance = distance;
            this.aftaDistance = moveBack;
            this.originalPosition = transform.position;
            this.resetCandle = resetCandle;
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
            else if (aftaDistance != Vector3.zero && !hitSecond) {
                if (Time.time - afterTime > afterWait)
                    if (Vector3.Distance(moveBack, transform.position) >= .1f)
                        body.AddForce(aftaDistance * speed * body.mass);
                    else
                        hitSecond = true;
            }
            else {
                hitFirst = false;
                hitSecond = false;
                resetCandle.ResetActivation();
                resetCandle = null;
                if (aftaDistance != Vector3.zero)
                    transform.position = originalPosition;
                else
                    transform.position = finalPos;
            }

        body.velocity = Vector2.zero;
        }
    }


}
