using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {
    bool Activated;
    // Public Attributes
    public float orbitTime = 5; // 5 seconds to complete a circle
    public float radius = 5;

    // Private Attributes
    private float angle = 0;
    private float speed;
    private Vector3 origin;
    public int WheelType;
    void Start() {
        if (speed == 0)
            speed = (2 * Mathf.PI) / orbitTime;
        else//change something here
            speed = (2 * Mathf.PI) / orbitTime;
        origin = transform.position;
    }

    void Update() {
        if (WheelType == 0)//original +
        {
            angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
            transform.position = origin + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, transform.position.z);
        }
        if (WheelType == 1)//original -
        {
            angle -= speed * Time.deltaTime;
            transform.position = origin + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, transform.position.z);
        }
        if (WheelType == 2)//original2 +
        {
            angle += speed * Time.deltaTime;
            transform.position = origin - new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, transform.position.z);
        }
        if (WheelType == 3)//original2 -
        {
            angle -= speed * Time.deltaTime;
            transform.position = origin - new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, transform.position.z);
        }
        else if (WheelType == 4)//oval up-down +
        {
            angle += speed * Time.deltaTime;
            transform.position = origin + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius * speed, transform.position.z / radius);
        }
        else if (WheelType == 5)//oval up-down -
        {
            angle -= speed * Time.deltaTime;
            transform.position = origin + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius * speed, transform.position.z / radius);
        }
        else if (WheelType == 6)//oval up-down2 +
        {
            angle += speed * Time.deltaTime;
            transform.position = origin - new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius * speed, transform.position.z / radius);
        }
        else if (WheelType == 7)//oval up-down2 -
        {
            angle -= speed * Time.deltaTime;
            transform.position = origin - new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius * speed, transform.position.z / radius);
        }
        else if (WheelType == 8)//Horizontal oval 1 +
        {
            angle += speed * Time.deltaTime;
            transform.position = origin - new Vector3(Mathf.Cos(angle) * radius * speed, Mathf.Sin(angle) * radius, transform.position.z / radius);
        }
        else if (WheelType == 9)//Horizontal oval 1 -
        {
            angle -= speed * Time.deltaTime;
            transform.position = origin - new Vector3(Mathf.Cos(angle) * radius * speed, Mathf.Sin(angle) * radius, transform.position.z / radius);
        }
        else if (WheelType == 10)//Horizontal oval 2 +
        {
            angle += speed * Time.deltaTime;
            transform.position = origin + new Vector3(Mathf.Cos(angle) * radius * speed, Mathf.Sin(angle) * radius, transform.position.z / radius);
        }
        else if (WheelType == 11)//Horizontal oval 2 -
        {
            angle -= speed * Time.deltaTime;
            transform.position = origin + new Vector3(Mathf.Cos(angle) * radius * speed, Mathf.Sin(angle) * radius, transform.position.z / radius);
        }

    }
}
