using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldStar : MonoBehaviour
{

    public string stageName;
    public int requiredScore = 1;

	// Use this for initialization
	void Start ()
    {
        int points = PlayerPrefs.GetInt(stageName);
        gameObject.SetActive(points >= requiredScore);
	}
	
}
