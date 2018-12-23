using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
	void Start ()
    {
        transform.Rotate(Vector3.forward, Random.value * 360f);
    }
}
