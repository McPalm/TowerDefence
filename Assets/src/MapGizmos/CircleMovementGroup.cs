using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovementGroup : MonoBehaviour
{
    public GameObject[] circles;
    public float DeltaX = 1f;
    public float DeltaY = 1f;
    public float Frequency = 1f;

    private void Start()
    {
        float offset = Random.value * Mathf.PI * 2f;
        float offsetDelta = Mathf.PI * 2f / circles.Length;
        foreach (var item in circles)
        {
            var c = item.AddComponent<CircleMovement>();
            c.deltaX = DeltaX;
            c.deltaY = DeltaY;
            c.frequency = Frequency;
            c.offset = offset;
            offset += offsetDelta;
        }
    }
    
}
