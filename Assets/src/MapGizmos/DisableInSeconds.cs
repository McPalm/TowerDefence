using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInSeconds : MonoBehaviour
{
    public float seconds;

    float timeout;

    private void OnEnable()
    {
        timeout = seconds;
    }

    // Update is called once per frame
    void Update ()
    {
        timeout -= Time.unscaledDeltaTime;
        if (timeout < 0f)
            gameObject.SetActive(false);
	}
}
