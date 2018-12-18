using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour {

    public Color start;
    public Color end;
    public float duration;

    SpriteRenderer _renderer;

    float progress = 0f;

	void Start ()
    {
        _renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        progress += Time.deltaTime / duration;
        _renderer.color = Color.Lerp(start, end, progress);
        if (progress >= 1f)
            enabled = false;
	}
}
