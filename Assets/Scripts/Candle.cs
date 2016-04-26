using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Candle : MonoBehaviour,ISwitchTrigger {

    //to do with movables attached to candle
    public int ArraySize;
    public Movable[] MovableIEffect;
    public Vector3[] HowMuchIMoveThem;
    public Vector3[] MoveAfterFinished;
    public float[] MovableSpeed;
    public float[] MovableWaitTime;


  //  public float CandleActiveWait = 1;
    public Player.TorchColor MyCandleType;
    public bool FireCanLight;
    
    //my private vars
    private AudioClip candleFlameAudio;
    private float volume;

    private Player player;
    private GameObject flame;

    private bool activated;
    private float activeLerpTime;

    private int reseted;

    void Start() {
        //get playa
        player = GameObject.FindObjectOfType<Player>();

        //get flame object and set it inactive
        (flame = transform.FindChild("flame").gameObject).SetActive(false);
        
        //lerptimes set to big ass small value lol
        activeLerpTime = int.MinValue;

        //setup audioclip
        candleFlameAudio = Resources.Load<AudioClip>("Sounds/Candle");
        volume = 1;//i guess?

        activated = false;
        reseted = 0;
    }
    public void SwitchTriggger()//change this to target effect.
    {
        FireCanLight = !FireCanLight;
        if (activated && !FireCanLight)//if it was just locked away
            {
                activated = !activated;//lit candle was just locked away in ice... or something. no longer active.
                flame.SetActive(false);
            }
    }

    void OnTriggerEnter2D(Collider2D col) {
        lightCandle(col);
    }

    void OnTriggerStay2D(Collider2D col) {
        lightCandle(col);
    }

    void lightCandle(Collider2D col) {
        if ((col.gameObject.tag == "Player" &&
            player.CurrentTorchType == MyCandleType &&
            player.CanLight) ||
            (col.gameObject.tag == "FireBall"&&
             MyCandleType == Player.TorchColor.Blue)
            ) {
            if (!activated && FireCanLight) {
                if (col.gameObject.tag == "Player")
                    player.MakeSmall(true);
                activate();
            }
        }
    }

    private void activate() {
        flame.SetActive(true);
        AudioSource.PlayClipAtPoint(candleFlameAudio, Camera.main.transform.position, volume);
        activated = true;
        activeLerpTime = Time.time;
        int ind = 0;
        foreach(Movable moveMe in MovableIEffect) {
            moveMe.Activate(MovableSpeed[ind], HowMuchIMoveThem[ind], MoveAfterFinished[ind], MovableWaitTime[ind++], this);
        }
    }

    public void ResetActivation() {
        if(++reseted == ArraySize) {
            activated = false;
            flame.SetActive(false);
            reseted = 0;
        }
    }

}

#if UNITY_EDITOR 
#region EditorWindow
// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(Candle))]
[CanEditMultipleObjects]
public class MyCandleEditor : Editor {



    SerializedProperty movables;
    SerializedProperty movement;
    SerializedProperty movementAfter;
    SerializedProperty speed;
    SerializedProperty arraySize;
    SerializedProperty moveWait;

  //  SerializedProperty candleActiveWait;
    SerializedProperty candleType;
    SerializedProperty fireCanLight;

    void OnEnable() {
        // Setup the SerializedProperties.
        updateProps();
    }

    public override void OnInspectorGUI() {
        inspectorGagdet();
    }

    public void OnSceneGUI() {

        EditorGUI.BeginChangeCheck();

        if (EditorGUI.EndChangeCheck()) {
            updateProps();
        }
    }

    private void updateProps() {
        movement = serializedObject.FindProperty("HowMuchIMoveThem");
        movementAfter = serializedObject.FindProperty("MoveAfterFinished");
        movables = serializedObject.FindProperty("MovableIEffect");
        speed = serializedObject.FindProperty("MovableSpeed");
        arraySize = serializedObject.FindProperty("ArraySize");
        moveWait = serializedObject.FindProperty("MovableWaitTime");

        fireCanLight = serializedObject.FindProperty("FireCanLight");
    //    candleActiveWait = serializedObject.FindProperty("CandleActiveWait");
        candleType = serializedObject.FindProperty("MyCandleType");
    }

    private void inspectorGagdet() {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();       
        EditorGUILayout.PrefixLabel("Can the fire light me? : ");
        fireCanLight.boolValue = EditorGUILayout.Toggle(fireCanLight.boolValue);
        EditorGUILayout.EndHorizontal();


//        EditorGUILayout.DelayedFloatField(candleActiveWait);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Candle Torch Type : ");
        candleType.enumValueIndex = (int) (Player.TorchColor) EditorGUILayout.EnumPopup((Player.TorchColor) candleType.enumValueIndex);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.DelayedIntField(arraySize);

        movables.arraySize = arraySize.intValue;
        speed.arraySize = arraySize.intValue;
        movement.arraySize = arraySize.intValue;
        movementAfter.arraySize = arraySize.intValue;
        moveWait.arraySize = arraySize.intValue;

        for (int i = 0;i < arraySize.intValue;i++) {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            movables.GetArrayElementAtIndex(i).objectReferenceValue = 
                EditorGUILayout.ObjectField("Movable : ", movables.GetArrayElementAtIndex(i).objectReferenceValue, typeof(Movable), true);

            EditorGUILayout.BeginHorizontal();
            movement.GetArrayElementAtIndex(i).vector3Value =  
                EditorGUILayout.Vector3Field("Move : " ,movement.GetArrayElementAtIndex(i).vector3Value);
            EditorGUILayout.EndHorizontal();
            movementAfter.GetArrayElementAtIndex(i).vector3Value =
                EditorGUILayout.Vector3Field("After : ", movementAfter.GetArrayElementAtIndex(i).vector3Value);

            speed.GetArrayElementAtIndex(i).floatValue = 
                EditorGUILayout.DelayedFloatField("Speed : ",speed.GetArrayElementAtIndex(i).floatValue);


            moveWait.GetArrayElementAtIndex(i).floatValue =
                EditorGUILayout.DelayedFloatField("WaitTilAfter : ", moveWait.GetArrayElementAtIndex(i).floatValue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endregion
#endif