﻿#if UNITY_EDITOR 
    using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    #region Initialization
    //Attributes to be set in inspector.
    public int speed;
    public int sizeFactor;
    public int deacclertionRate;
    public int scale = 3;
    public float maxScaleFactor;
    public float JoySensitivity;

    public TorchColor currentTorchType;
    public TorchColor CurrentTorchType {
        get { return currentTorchType; }
    }
    public enum TorchColor { Regular,Blue,Green,Yellow}

    //Attributes for particle effect changing...Don't touch these please :)
    private const float speedMod = .1f;
    private const float emissionMod = 40;
    private const float particleSpeedMod = 2f;

    //Private attributes
    private const int fireLimit = 5;

    private bool hasJoystick;
    private bool canLight;
    private float particleRotation;
    private Vector3 mousePos;
    private Vector3 oldMousePos;
    private Vector3 mouseWorldPos;
    private Vector3 objectPos;
    private Vector3 clickRotationDest;
    private Vector2 clickMoveDest;
    private Vector3? clickDest;
    private Vector3? joyDest;
    private Vector3? rightJoyDest;
    private float clickOrHoldTime;
    private const float clickOrHoldWait = .2f;
    private float clickDestTime;
    private const float clickDestWait = .5f;
    private bool killDest;
    private bool unkillAbleDest;

    private bool moving = false;
    private ParticleSystem fireTail;
    private ParticleSystem.EmissionModule fireEmission;
    private ParticleSystem.ShapeModule fireShape;
    private ParticleSystem makeMeBig;
    private ParticleSystem makeMeSmallIfForced;
    private ParticleSystem teleportBam;
    private bool rotated, distanceGreatEnough;
    private float rotateLerpTime;
    private Rigidbody2D sexyBody;
    private float hitCoolDown;
    public  float hitCoolWaitTime;
    private Gradient currentGradient;
    private float torchLerpTime;
    private float beatLevelTime;
    private const float beatLevelWait = 1f;

    private GameObject sweetHeart;
    private GameObject fireStandard;

    private GameObject flicker;
    private ParticleSystem.ColorOverLifetimeModule flickerGradient;
    private FlickerGradients flickerScript;

    private GameObject flickerHeart;
    private ParticleSystem.ColorOverLifetimeModule flickerHeartGradient;
    private FlickerGradients flickerHeartScript;

    private ParticleSystem.ColorOverLifetimeModule torchGradient;
    private FlickerGradients torchScript;

    private ParticleSystem.ColorOverLifetimeModule explosionGradient;
    private FlickerGradients explosionScript;

    private PlayerHeart heartSystemScript;
    private FIRE FIREUIScript;

    private bool changeTorchType;
    private ParticleSystem makeMeSmall;
    private float zoom;

    public bool CameraShouldNOTMove {
        get { return distanceGreatEnough || !moving; }
    }

    //hmm...what's this for?
    void Start() {
        //when you reload a scene if you don't reset the particles sometimes they 
        //are added or not reset correctly, so...bam, fixed
        ParticleSystem[] pses = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in pses) {
            ps.Clear();
        }
        
        heartSystemScript = FindObjectOfType<PlayerHeart>();
        FIREUIScript = FindObjectOfType<FIRE>();

        sweetHeart = GameObject.Find("SweetHeartLife");
        canLight = false;
        fireTail = GameObject.Find("FireTail").GetComponent<ParticleSystem>();
        makeMeBig = GameObject.Find("IWannaBeBig").GetComponent<ParticleSystem>();
        makeMeSmall = GameObject.Find("IWannaBeSmall").GetComponent<ParticleSystem>();
        makeMeSmallIfForced = GameObject.Find("SmallForced").GetComponent<ParticleSystem>();
        teleportBam = GameObject.Find("TeleportBam").GetComponent<ParticleSystem>();
        fireStandard = GameObject.Find("FireAbilityStandard");

        flicker = GameObject.Find("Flicker");
        flickerGradient = flicker.GetComponent<ParticleSystem>().colorOverLifetime;
        flickerScript = flicker.GetComponent<FlickerGradients>();

        flickerHeart = GameObject.Find("FlickerHeart");
        flickerHeartGradient = flickerHeart.GetComponent<ParticleSystem>().colorOverLifetime;
        flickerHeartScript = flickerHeart.GetComponent<FlickerGradients>();

        torchGradient = fireTail.colorOverLifetime;
        torchScript = fireTail.GetComponent<FlickerGradients>();

        explosionGradient = makeMeBig.colorOverLifetime;
        explosionScript = makeMeBig.GetComponent<FlickerGradients>();

        fireEmission = fireTail.emission;
        fireShape = fireTail.shape;
        distanceGreatEnough = true;
        sexyBody = GetComponent<Rigidbody2D>();
        unkillAbleDest = true;

        beatLevelTime = int.MaxValue;

        hasJoystick = Input.GetJoystickNames().Length > 0;
        Debug.Log(hasJoystick);

        SizeFix();   
    }
    #endregion


    #region Updating
    //if something is rigidbody...
    void FixedUpdate() {
        if (sizeFactor > 0 && beatLevelTime == int.MaxValue) {
            //if the camera is done moving once stopped to prevent stuttering...
            if (moving)
                if (clickDest != null) {
                    clickAndGo();
                } else if (joyDest != null)
                    move((Vector2)joyDest);
                else {
                    sexyBody.velocity *= .2f;
                }
            if (killDest) {
                sexyBody.velocity *= 0;
                killDest = false;
            }
        }
    }

    //if something is NOT rigidbody lol.
    void Update() {
        //sets up vars
        varControl();

        //we want input and we want it now...
        if (sizeFactor > 0 && beatLevelTime == int.MaxValue)
            input();
        //unless we die or win
        else {
            sexyBody.velocity = Vector2.zero;
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
            Camera.main.transform.position.z);
            Camera.main.orthographicSize -= zoom;

            transform.localScale *= .99f;

            if (Camera.main.orthographicSize - zoom > 10) 
                zoom += Camera.main.orthographicSize / 30;
            //win
            if (Time.time - beatLevelTime > beatLevelWait) {
                GameManager.instance.ActivateNextLevelPanel();
            }
            //dead
            else if(sizeFactor == 0){

            }
        }
        //kinky fire changing lol...
        if (changeTorchType)
            fiftyShadesOfFire();

        //oldMousePos = mousePos;
    }

    //sexy changing fire stuff
    void fiftyShadesOfFire() {
        flickerGradient.color = flickerScript.ChangeGradient(currentTorchType);
        flickerHeartGradient.color = flickerHeartScript.ChangeGradient(currentTorchType);
        torchGradient.color = torchScript.ChangeGradient(currentTorchType);
        explosionGradient.color = explosionScript.ChangeGradient(currentTorchType);
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

    #region input
    //handles input and how we react to such input...
    private void input() {

        if (moving) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                setClickDest(true);
                clickOrHoldTime = Time.time;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0)) {
                clickOrHoldTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.Mouse0)) {
                if (Time.time - clickOrHoldTime > clickOrHoldWait)
                    setClickDest();
            }
            //otherwise give me joystick
            else if (hasJoystick) {
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");



                x = Mathf.Abs(x) > .5f ? x : 0;
                y = Mathf.Abs(y) > .5f ? y : 0;

                if (x != 0 || y != 0) {
                    joyDest = new Vector3(x, y, 0) * JoySensitivity;
                    clickDest = null;
                }
                else
                    joyDest = null;

                
            }
            else if (!unkillAbleDest) {
                killDest = true;
                clickDest = null;
            }
        }

    }

    private void setClickDest(bool kill = false) {
        clickDest = mouseWorldPos;
        clickMoveDest = mouseWorldPos - transform.position;
        clickRotationDest = mousePos;
        unkillAbleDest = kill;
    }

    //that there movement
    private void move(Vector2 dist) {
        sexyBody.AddForce(dist*(speed));
        sexyBody.velocity /= deacclertionRate;
    }

    //if we just click instead of holding and moving
    private void clickAndGo() {
        Vector2 clickXY = new Vector2(((Vector3) clickDest).x, ((Vector3) clickDest).y);
        Vector2 transXY = new Vector2(transform.position.x, transform.position.y);
        Vector2 dist = clickXY - transXY;
        
        if (dist.magnitude >= 1) {
            move(clickMoveDest);
        }
        else {
            clickDest = null;
            unkillAbleDest = false;
        }

        sexyBody.velocity *=( 100 / ((float)deacclertionRate)/100);
    }

    //deprecated rotmove
    //rotation and movement...what did you expect?
    //private void rotationAndMovement() {
    //    Vector2 mouseXY = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    //    Vector2 transXY = new Vector2(transform.position.x, transform.position.y);
    //    Vector2 dist = mouseXY - transXY;

    //    float angle = -90;
    //    distanceGreatEnough = dist.magnitude <= 2f && Vector3.Distance(mousePos,oldMousePos) <= 50f;
    //    if (!distanceGreatEnough) {
    //        rotated = false;
    //        //movement, so simple...so clean...
    //        move(dist);

    //        //rotation
    //        if (rotateLerpTime > 0) {
    //            angle = Mathf.Lerp(-90, Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg, 1 - rotateLerpTime);
    //            rotateLerpTime -= .02f;
    //        }
    //        //other wise make if fly in the rotation angle...
    //        else
    //            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

    //    }
    //    else if (!rotated) {
    //        angle = -90;
    //        rotated = true;
    //        sexyBody.velocity = Vector2.zero;
    //    }
    //    rotation(angle);
    //}
    #endregion

    #region nonFire
    public float ParticleRotation {
        get {
            float rx = Input.GetAxis("RHorizontal");
            float ry = Input.GetAxis("RVertical");
            rx = Mathf.Abs(rx) > .5f ? rx : 0;
            ry = Mathf.Abs(ry) > .5f ? ry : 0;
            if (rx != 0 || ry != 0)
                rightJoyDest = new Vector3(rx, ry, 0) * JoySensitivity;
            else
                rightJoyDest = null;

            if (rightJoyDest == null)
                return (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(((Vector2)rightJoyDest).y, ((Vector2)rightJoyDest).x) * Mathf.Rad2Deg);
        }
    }
    public bool CanLight {
        get { return canLight; }
    }
    public void FlipCanLightSwitch() {
        canLight = !canLight;
    }

    public void MakeSmall(bool forced = false)
    {
        if (sizeFactor > 0) {
            sizeFactor--;
            if (forced) {
                makeMeSmallIfForced.startLifetime = 1f;
                makeMeSmallIfForced.Emit(300);
            }
            else {
                if (sizeFactor == 0) {
                    makeMeSmallIfForced.startLifetime = 1f;
                    makeMeSmallIfForced.Emit(300);
                }
                makeMeSmall.startLifetime = 1f;
                makeMeSmall.Emit(300);
            }
            if (sizeFactor == 0) {
                GameManager.instance.ActivateRetryPanel();
            }
            SizeFix();

            hitCoolDown = Time.time;
        }
    }
    public void MakeBigAndChangeColor(Collider2D col) {
        if (sizeFactor < maxScaleFactor) {
            sizeFactor++;
            makeMeBig.startLifetime = 1f;
            makeMeBig.Emit(300);
            changeTorchType = true;
            FIREUIScript.ChangeColor(currentTorchType = col.gameObject.GetComponent<EreDayBeTorching>().colorMeHappy);
            hitCoolDown = Time.time;
            SizeFix();
        }
    }

    //if we hit some type of trigger...

    void OnTriggerEnter2D(Collider2D col) {

        if (col.gameObject.tag == "Obstacle") {
            GameManager.instance.ActivateRetryPanel();
        }
        else if (col.gameObject.tag == "Teleporter") {
            teleportBam.Emit(300);
            beatLevelTime = Time.time;
        }
        if (Time.time - hitCoolDown > hitCoolWaitTime) {
                if (col.gameObject.tag == "Torch") {
                    GameManager.instance.TorchAudioPlay();
                    MakeBigAndChangeColor(col);
                }
                else if (col.gameObject.tag == "Water") {
                    GameManager.instance.WaterAudioPlay();
                    MakeSmall();
                }
        }

    }

    void OnCollisionEnter2D(Collision2D collision) {
        clickDestTime = Time.time;
    }

    void OnCollisionStay2D(Collision2D collision) {
        if(Time.time - clickDestTime > clickDestWait)
            clickDest = null;
    }

    //When a fire gets big, or small...
    public void SizeFix() {
        float si = (float)(sizeFactor) * 2/3 * scale;

        transform.localScale = new Vector3(si,si,si);
        flicker.transform.localScale = new Vector3(si, si, si) / 2.5f;
        flickerHeart.transform.localScale = new Vector3(si, si, si) / 5.5f;
        fireTail.transform.localScale = new Vector3(si, si, si);
        fireStandard.transform.localScale = new Vector3(si, si, si) / 2.2f;

        heartSystemScript.Reset();
        FIREUIScript.SetInactive(sizeFactor);
    }

    //I WANT TO MOVE!
    public void activateMovement() {
        moving = !moving;
    }

    public bool Moving {
        get { return moving; }
    }
    #endregion

    #endregion

}

#if UNITY_EDITOR 
#region EditorWindow



// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]
public class MyPlayerEditor : Editor {
    SerializedProperty scale;
    SerializedProperty speed;
    SerializedProperty sizeFactor;
    SerializedProperty deacclertionRate;
    SerializedProperty hitCoolWaitTime;
    SerializedProperty joySens;
    void OnEnable() {
        // Setup the SerializedProperties.
        updateProps();
    }

    public override void OnInspectorGUI() {
        inspectorGagdet();
    }

    // Custom GUILayout progress bar.
    void ProgressBar(int value, string label) {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, ((float) value) / 4, label);
        EditorGUILayout.Space();
    }

    public void OnSceneGUI() {
        EditorGUI.BeginChangeCheck();

        if (EditorGUI.EndChangeCheck()) {
            updateProps();
        }
    }

    private void updateProps() {
        scale = serializedObject.FindProperty("scale");
        speed = serializedObject.FindProperty("speed");
        sizeFactor = serializedObject.FindProperty("sizeFactor");
        deacclertionRate = serializedObject.FindProperty("deacclertionRate");
        hitCoolWaitTime = serializedObject.FindProperty("hitCoolWaitTime");
        joySens = serializedObject.FindProperty("JoySensitivity");
    }

    private void inspectorGagdet() {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        // Show the custom GUI controls.
        EditorGUILayout.IntPopup(sizeFactor, new GUIContent[]
        {
        new GUIContent("SuperSmall - 1"),
        new GUIContent("Small - 2"),
        new GUIContent("Normal - 3"),
        new GUIContent("Large - 4")},
        new int[] { 1, 2, 3, 4 });

        ProgressBar(sizeFactor.intValue, "Fire Size " + sizeFactor.intValue.ToString() + " / 4");

        EditorGUILayout.IntSlider(speed, 50, 200, new GUIContent("Speed"));

        EditorGUILayout.HelpBox("100 will make it linear, 1 will make it pure acceleration based", MessageType.Info);
        EditorGUILayout.IntSlider(deacclertionRate, 1, 100, new GUIContent("Deaccleration Rate"));

        EditorGUILayout.HelpBox("Don't Edit the scale unless you ask dustin first... :) thanks!", MessageType.Warning);

        EditorGUILayout.DelayedIntField(scale);

        EditorGUILayout.DelayedFloatField(joySens);

        EditorGUILayout.DelayedFloatField(hitCoolWaitTime);
        
        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

}

#endregion
#endif