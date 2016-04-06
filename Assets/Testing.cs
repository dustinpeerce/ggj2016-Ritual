
using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]
public class MyPlayerEditor : Editor {
    SerializedProperty scale;
    SerializedProperty speed;
    SerializedProperty sizeFactor;
    SerializedProperty deacclertionRate;
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
        EditorGUI.ProgressBar(rect, ((float)value)/4, label);
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

        ProgressBar(sizeFactor.intValue, "Fire Size");

        EditorGUILayout.IntSlider(speed, 1, 15, new GUIContent("Speed"));


        EditorGUILayout.HelpBox("100 will make it linear, 1 will make it pure acceleration based", MessageType.Info);
        EditorGUILayout.IntSlider(deacclertionRate, 1, 100, new GUIContent("Deaccleration Rate"));

        EditorGUILayout.HelpBox("Don't Edit the scale unless you ask dustin first... :) thanks!", MessageType.Warning);

        EditorGUILayout.DelayedIntField(scale);

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

}