using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchTurbo : MonoBehaviour {

    public Text text;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void Toggle()
    {
        if (Time.timeScale > 1f)
            Time.timeScale = 1f;
        else
            Time.timeScale = 3f;
        text.text = Time.timeScale > 1f ? "x3" : "x1";
    }
}
