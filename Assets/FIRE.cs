using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FIRE : MonoBehaviour {

    RectTransform rect;
    Image image;
    private const float sizeLiar = 2.700352f;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        FireSize();
	}

    public void FireSize() {

    }
}
