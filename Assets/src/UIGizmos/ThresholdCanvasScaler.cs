using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThresholdCanvasScaler : MonoBehaviour
{

    public int baseHeight = 800;

    CanvasScaler scaler;

    int lastHeight = 0;


    private void Start()
    {
        scaler = GetComponent<CanvasScaler>();
    }

    // Update is called once per frame
    void Update () {
        if (lastHeight != Screen.height)
            ReScale();
	}

    void ReScale()
    {
        lastHeight = Screen.height;
        if (lastHeight <= baseHeight)
            scaler.scaleFactor = 1f;
        else
            scaler.scaleFactor = (float)lastHeight / baseHeight;
    }
}
