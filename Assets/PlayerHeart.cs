using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerHeart : MonoBehaviour {

    public int StartLife;
    public int MaxLife;
    Player player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void TickDown() {
        StartLife--;
        if(StartLife % 4 == 0) {
            player.MakeSmall(true);
        }
    }

}

#region EditorWindow
// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(PlayerHeart))]
[CanEditMultipleObjects]
public class MyHeartEditor : Editor {
    SerializedProperty startLife;
    SerializedProperty maxLife;
    Player player;

    void OnEnable() {
        player = FindObjectOfType<Player>();
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
        EditorGUI.ProgressBar(rect, ((float) value)/maxLife.intValue, label);
        EditorGUILayout.Space();
    }

    public void OnSceneGUI() {

        EditorGUI.BeginChangeCheck();

        if (EditorGUI.EndChangeCheck()) {
            updateProps();
        }
    }

    private void updateProps() {
        startLife = serializedObject.FindProperty("StartLife");
        maxLife = serializedObject.FindProperty("MaxLife");
    }

    private void honorMaxLife() {
        if(player.sizeFactor * 4 < maxLife.intValue) {
            maxLife.intValue = player.sizeFactor * 4;
        }

        if (maxLife.intValue < startLife.intValue) {
            startLife.intValue = maxLife.intValue;
        }
    }

    private void inspectorGagdet() {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();
        honorMaxLife();

        EditorGUILayout.HelpBox("How may hearts do we start off with? \n"+
            "Note: If you lower max life, it will also lower start life if start life > max life\n"+
            "If you change size factor of the player, it will not let you have more than size factor * 4", MessageType.Warning);
        EditorGUILayout.IntSlider(startLife, 1, maxLife.intValue, new GUIContent("Start Life"));
        ProgressBar(startLife.intValue, "Hearts " + startLife.intValue.ToString() + " / " + maxLife.intValue.ToString());


        EditorGUILayout.SelectableLabel("Size Factor : " + player.sizeFactor + "\nCurrent Max Life Limit : " + (player.sizeFactor * 4));
        EditorGUILayout.IntSlider(maxLife, 1, 16, new GUIContent("Max Life"));

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.


        serializedObject.ApplyModifiedProperties();
    }
}
#endregion