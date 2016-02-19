using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {


    // Private Attributes
    private float speed;
	private const float speedMod = .1f;
	private const float emissionMod = 40;
	private const float particleSpeedMod = 2f;
    private Vector3 mousePos;
    private Vector3 mouseWorldPos;
    private Vector3 objectPos;
    private float playerRotationAngle;
    private bool moving = false;
	private ParticleSystem fire;
	private ParticleSystem.EmissionModule fireEmission;
	private ParticleSystem.ShapeModule fireShape;
    private bool rotated,distanceGreatEnough;
    private float rotateLerpTime;
    public bool CameraShouldNOTMove {
        get { return distanceGreatEnough||!moving; }
    }

    void Start () {
		fire = GetComponent<ParticleSystem> ();
		fireEmission = fire.emission;
		fireShape = fire.shape;
        distanceGreatEnough = true;
	}

    void Update() {
        // Calculate Rotation
        mousePos = Input.mousePosition;
        objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        input();
        rotationAndMovement();
        
	}

    //handles input and how we react to such input...
    private void input() {
        if (!moving && Input.GetMouseButtonDown(0))
        {
            moving = true;
            GameManager.instance.SetTimer(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(moving)
                GameManager.instance.ActivatePausePanel();
            moving = !moving;
            GameManager.instance.SetTimer(false);
        }

    }

    //rotation and movement...what did you expect?
    private void rotationAndMovement() {
        // Apply Rotation
        if (!rotated)
        {
            transform.rotation = Quaternion.Euler(new Vector3(playerRotationAngle, 270, 0));
            fire.startRotation = playerRotationAngle * -Mathf.Deg2Rad;
        }
        //if it's suppose to be standing still make the fire not just completley static yknow?
        else {
            fire.startRotation = (playerRotationAngle) * -Mathf.Deg2Rad;
            playerRotationAngle = Mathf.Clamp((playerRotationAngle + Random.Range(-.03f, .03f)), -93, -87);
        }
        //if it's moving we need to do shit...
        if (moving)
        {
            //detects if the distance is great enough to move
            Vector2 mouseXY = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            Vector2 transXY = new Vector2(transform.position.x, transform.position.y);

            distanceGreatEnough = Vector2.Distance(mouseXY, transXY) <= 2.1f;

            //if so do this
            if (!distanceGreatEnough)
            {
                //init speed, it's used alot...
                speed = speedMod * Time.deltaTime * Vector3.Distance(transform.position, mousePos);

                //apply speed modifer
                fire.startSpeed = particleSpeedMod * speed;
                fireEmission.rate = new ParticleSystem.MinMaxCurve(emissionMod * speed + 15);
                fireShape.angle = speed;

                //just for safe keeping
                mouseWorldPos.z = transform.position.z;

                //we need to move toward the mouse yeah?
                transform.position = Vector3.MoveTowards(transform.position, mouseWorldPos, speed);

                rotated = false;

                //if it had rotated we need to lerp back there
                if (rotateLerpTime > 0)
                {
                    playerRotationAngle = Mathf.Lerp(-90, Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg, 1 - rotateLerpTime);
                    rotateLerpTime -= .02f;
                }
                //other wise make if fly in the rotation angle...
                else
                    playerRotationAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


            }
            //if we are stopped and we haven't reach our rotation limit...
            else if (!rotated)
            {
                //rotate until the fire is hovering at a -90...
                if (playerRotationAngle != -90)
                {
                    rotateLerpTime += .01f;
                    playerRotationAngle = Mathf.Lerp(playerRotationAngle, -90, rotateLerpTime);
                }
                //then we need to make sure we don't rotate anymore...
                else
                    rotated = true;
            }

        }
    }

    void FixedUpdate() {

    }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col.name);
        if (col.gameObject.tag == "Obstacle") {
            GameManager.instance.ActivateRetryPanel();
        }
        else if (col.gameObject.tag == "Teleporter") {
            GameManager.instance.ActivateNextLevelPanel();
        }
    }

    public void activateMovement() {
        moving = !moving;
    }
}
