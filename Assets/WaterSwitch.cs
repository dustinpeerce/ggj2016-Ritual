using UnityEngine;
using System.Collections;
using System;

public class WaterSwitch : MonoBehaviour, ISwitchTrigger {
    private Player player;
    bool CanAccess;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
	}

    public void SwitchTriggger() {
        gameObject.SetActive((CanAccess = !CanAccess));
    }
}
