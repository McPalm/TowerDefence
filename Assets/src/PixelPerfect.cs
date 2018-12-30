using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfect : MonoBehaviour {

    public int PixelPerUnit = 52;
    public int pixelScale = 1;
	
	// Update is called once per frame
	void LateUpdate ()
    {
        var camera = Camera.main;
        var height = camera.pixelHeight;

        camera.orthographicSize = .5f * height / pixelScale / PixelPerUnit;
	}
}
