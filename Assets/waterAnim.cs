using UnityEngine;
using System.Collections;

public class waterAnim : MonoBehaviour {
    public Gradient water;

    Rigidbody sexyBody;

	// Use this for initialization
	void Start () {
        sexyBody = GetComponent<Rigidbody>();
        sexyBody.angularVelocity = new Vector3(360, 360, 360);

    }

    // Update is called once per frame
    void Update () {

    }

}
