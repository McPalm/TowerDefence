using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageColorLerp : MonoBehaviour {

    public Color StartColor;
    public Color FinalColor;
    public float time;

    // Use this for initialization
    void OnEnable() => StartCoroutine(LerpRoutine());
	
    IEnumerator LerpRoutine()
    {
        var image = GetComponent<UnityEngine.UI.Image>();
        image.color = StartColor;

        yield return null;

        for(float t = 0; t < 1f; t += Time.unscaledDeltaTime / time)
        {
            image.color = Color.Lerp(StartColor, FinalColor, t);
            yield return null;
        }
        image.color = FinalColor;
    }
}
