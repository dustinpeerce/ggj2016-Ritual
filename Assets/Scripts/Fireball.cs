using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    #region Initialization
    //Attributes to be set in inspector.
    public float speed;
    public int sizeFactor;
    public float scale;
    public float maxScaleFactor;
    
    public TorchColor currentTorchType;
    public enum TorchColor { Regular,Blue,Green,Yellow}

    //Attributes for particle effect changing...Don't touch these please :)
    private const float speedMod = .1f;
    private const float emissionMod = 40;
    private const float particleSpeedMod = 2f;

    //Private attributes
    private const int fireLimit = 5;
    private Vector3 mousePos;
    private Vector3 mouseWorldPos;
    private Vector3 objectPos;
    private bool moving = false;
    private ParticleSystem fire;
    private ParticleSystem.EmissionModule fireEmission;
    private ParticleSystem.ShapeModule fireShape;
    private ParticleSystem makeMeBig;
    private bool rotated, distanceGreatEnough;
    private float rotateLerpTime;
    private Rigidbody2D sexyBody;
    private float hitCoolDown;
    public  float hitCoolWaitTime;
    private Gradient currentGradient;
    private float torchLerpTime;

    private GameObject flicker;
    private ParticleSystem.ColorOverLifetimeModule flickerGradient;
    private FlickerGradients flickerScript;

    private ParticleSystem.ColorOverLifetimeModule torchGradient;
    private FlickerGradients torchScript;

    private ParticleSystem.ColorOverLifetimeModule explosionGradient;
    private FlickerGradients explosionScript;

    private bool changeTorchType;
    private ParticleSystem makeMeSmall;

    public bool CameraShouldNOTMove {
        get { return distanceGreatEnough || !moving; }
    }

    //hmm...what's this for?
    void Start() {
        fire = GameObject.Find("FireTail").GetComponent<ParticleSystem>();
        makeMeBig = GameObject.Find("IWannaBeBig").GetComponent<ParticleSystem>();
        makeMeSmall = GameObject.Find("IWannaBeSmall").GetComponent<ParticleSystem>();

        flicker = GameObject.Find("Flicker");
        flickerGradient = flicker.GetComponent<ParticleSystem>().colorOverLifetime;
        flickerScript = flicker.GetComponent<FlickerGradients>();

        torchGradient = fire.colorOverLifetime;
        torchScript = fire.GetComponent<FlickerGradients>();

        explosionGradient = makeMeBig.colorOverLifetime;
        explosionScript = makeMeBig.GetComponent<FlickerGradients>();

        fireEmission = fire.emission;
        fireShape = fire.shape;
        distanceGreatEnough = true;
        sexyBody = GetComponent<Rigidbody2D>();

        sizeFix();   
    }
    #endregion


    #region Updating
    //if something is rigidbody...
    void FixedUpdate() {
        //if the camera is done moving once stopped to prevent stuttering...
        if (moving)
            rotationAndMovement();
    }

    //if something is NOT rigidbody lol.
    void Update() {
        //sets up vars
        varControl();

        //we want input and we want it now...
        input();
        
        //kinky fire changing lol...
        if (changeTorchType)
            fiftyShadesOfFire();
    }

    //sexy changing fire stuff
    void fiftyShadesOfFire() {
        flickerGradient.color = flickerScript.changeFlicker(currentTorchType);
        torchGradient.color = torchScript.changeFlicker(currentTorchType);
        explosionGradient.color = explosionScript.changeFlicker(currentTorchType);
        changeTorchType = false;
    }

    //sets up vars for using
    void varControl() {
        //We need the mouse compared to the object...
        mousePos = Input.mousePosition;
        objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);        
    }

    //handles input and how we react to such input...
    private void input() {
        /*
        if (!moving && Input.GetMouseButtonDown(0)) {
            moving = true;
            GameManager.instance.SetTimer(true);
        }
        */
    }

    //that there movement
    private void move(Vector2 dist) {
        sexyBody.AddForce(dist*speed);
        sexyBody.velocity *= 0;//dampening...
    }

    //that there rotation
    private void rotation(float angle) {
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        fire.startRotation = (angle) * -Mathf.Deg2Rad;
    }

    //rotation and movement...what did you expect?
    private void rotationAndMovement() {
        Vector2 mouseXY = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        Vector2 transXY = new Vector2(transform.position.x, transform.position.y);
        Vector2 dist = mouseXY - transXY;

        float angle = -90;
        distanceGreatEnough = dist.magnitude <= 2.1f;
        if (!distanceGreatEnough) {
            rotated = false;
            //movement, so simple...so clean...
            move(dist);

            //rotation
            if (rotateLerpTime > 0) {
                angle = Mathf.Lerp(-90, Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg, 1 - rotateLerpTime);
                rotateLerpTime -= .02f;
            }
            //other wise make if fly in the rotation angle...
            else
                angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        }
        else if (!rotated) {
            angle = -90;
            rotated = true;
            sexyBody.velocity = Vector2.zero;
        }
        rotation(angle);
    }
    public void MakeSmall()
    {
        if (Time.time - hitCoolDown > hitCoolWaitTime)
        {
            sizeFactor--;
            makeMeSmall.startLifetime = 1f;
            makeMeSmall.Emit(300);
            sizeFix();
            if (sizeFactor == 0)
                GameManager.instance.ActivateRetryPanel();
            hitCoolDown = Time.time;
        }
    }
    //if we hit some type of trigger...
    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col.name);
        if (col.gameObject.tag == "Obstacle") {
            GameManager.instance.ActivateRetryPanel();
        }
        else if (col.gameObject.tag == "Teleporter") {
            GameManager.instance.ActivateNextLevelPanel();
        }
        if (Time.time - hitCoolDown > hitCoolWaitTime) {
            
                if (col.gameObject.tag == "Torch") {
                    if (sizeFactor < maxScaleFactor) {
                        sizeFactor++;
                        makeMeBig.startLifetime = 1f;
                        makeMeBig.Emit(300);
                        changeTorchType = true;
                        currentTorchType = col.gameObject.GetComponent<EreDayBeTorching>().colorMeHappy;
                    }

                }
                else if (col.gameObject.tag == "Water") {
                    sizeFactor--;
                    makeMeSmall.startLifetime = 1f;
                    makeMeSmall.Emit(300);
                }
                sizeFix();

                if (sizeFactor == 0)
                    GameManager.instance.ActivateRetryPanel();

                hitCoolDown = Time.time;
            
        }

    }

    //When a fire gets big, or small...
    private void sizeFix() {
        float si = (float)(sizeFactor) * 2/3 * scale;
        transform.localScale = new Vector3(si,si,si);
        flicker.transform.localScale = new Vector3(si, si, si) / 2.5f;
        fire.transform.localScale = new Vector3(si, si, si);
    }

    //I WANT TO MOVE!
    public void activateMovement() {
        moving = !moving;
    }

    public bool Moving {
        get { return moving; }
    }
    
    #endregion
}