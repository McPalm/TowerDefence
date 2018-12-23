using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZoom : MonoBehaviour {

    public float finalSize = 2f;
    public float time = 4f;

    void Start() => StartCoroutine(ZoomRoutine());
	
	IEnumerator ZoomRoutine()
    {
        yield return null;

        var camera = Camera.main;
        var startSize = camera.orthographicSize;
        var startPosition = camera.transform.position;

        for(float t = 0; t < 1f; t += Time.deltaTime / time)
        {
            camera.orthographicSize = Mathf.Lerp(startSize, finalSize, EndFeather(EndFeather(t)));
            camera.transform.position = Vector3.Lerp(startPosition, transform.position + new Vector3(0f, 0f, -10f), EndFeather(EndFeather(t)));

            yield return null;
        }
    }

    static float EndFeather(float f) => 1f - (1 - f) * (1 - f);
}
