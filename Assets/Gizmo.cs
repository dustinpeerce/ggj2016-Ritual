using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour {
    EdgeCollider2D edge;
    Vector3 min;
    Vector3 max;

    void Awake() {

    }
	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update () {
	
	}
    void OnDrawGizmos() {
        edge = GetComponent<EdgeCollider2D>();
        min = edge.bounds.min;
        max = edge.bounds.max;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(min, max);
    }
}
