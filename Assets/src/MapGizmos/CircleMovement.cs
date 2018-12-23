using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float deltaX;
    public float deltaY;
    public float frequency;
    public float offset;
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad * frequency + offset) * deltaX, Mathf.Cos(Time.timeSinceLevelLoad * frequency + offset) * deltaY);
	}
}
