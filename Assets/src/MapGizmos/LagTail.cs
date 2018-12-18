using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagTail : MonoBehaviour {

    public Transform tail;
    Vector3 memory;

    private void Start()
    {
        memory = transform.position;
    }

    private void LateUpdate()
    {
        tail.transform.position = memory;
        memory = transform.position;
    }
}
