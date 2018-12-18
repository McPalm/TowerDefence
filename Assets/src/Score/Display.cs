using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour {

    public string prefix;
    public string affix;
    public Text text;

    public void OnChange(int i)
    {
        text.text = prefix + i + affix;
    }
}
