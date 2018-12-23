using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{

    public float DeltaX;
    public float DeltaY;
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = new Vector3(Mathf.PerlinNoise(Time.timeSinceLevelLoad * 20f, 0f) * DeltaX, Mathf.PerlinNoise(Time.timeSinceLevelLoad * 20f, 10f) * DeltaY);
	}
}
