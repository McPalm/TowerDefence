using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBump : MonoBehaviour
{
    Vector3 standard;
    public Vector3 offset;
    public float threshold = 600f;

    private void Start()
    {
        standard = transform.localPosition;
        offset = transform.localPosition + offset;
    }


    // Update is called once per frame
    void LateUpdate ()
    {
        transform.localPosition = Screen.height < threshold ? offset : standard;
	}
}
