using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetButtonDown("Reset"))
        {
            ReloadScene();
        }
	}

    static public void ReloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
#if UNITY_ANDROID
        SceneManager.LoadScene("TouchGameplay", LoadSceneMode.Additive);
#else
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
#endif
    }
}
