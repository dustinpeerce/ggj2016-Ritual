using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    Player player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = player.transform.position;
        newPos.z = 0;
        newPos.x -= Screen.width / 24;
        transform.position = newPos;

	}
}
