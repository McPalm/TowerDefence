using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    public Button Easy;
    public Button Medium;
    public Button Hard;

    static public Difficulty Difficulty { set; get; }
	
    void Start()
    {
        if (Difficulty == 0)
            Difficulty = Difficulty.medium;

        Easy.onClick.AddListener( () =>
        {
            Difficulty = Difficulty.easy;
            Easy.interactable = false;
            Medium.interactable = true;
            Hard.interactable = true;
        });
        Medium.onClick.AddListener(() =>
        {
            Difficulty = Difficulty.easy;
            Easy.interactable = true;
            Medium.interactable = false;
            Hard.interactable = true;
        });
        Hard.onClick.AddListener(() =>
        {
            Difficulty = Difficulty.easy;
            Easy.interactable = true;
            Medium.interactable = true;
            Hard.interactable = false;
        });

        Easy.interactable = Difficulty != Difficulty.easy;
        Medium.interactable = Difficulty != Difficulty.medium;
        Hard.interactable = Difficulty != Difficulty.hard;
    }
}
