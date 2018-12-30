using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudio : MonoBehaviour {

    public float delay = 3f;
    
    IEnumerator Routine()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        var audio = FindObjectOfType<ObjectPooling.AudioPool>();

        audio.mute = true;
    }

    private void OnEnable()
    {
        StartCoroutine(Routine());
    }

    private void OnDisable()
    {
        var audio = FindObjectOfType<ObjectPooling.AudioPool>();

        audio.mute = true;
    }
}
