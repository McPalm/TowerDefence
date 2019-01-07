using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject Map1;
        public GameObject Map2;
        public GameObject Map3;

        int selectedStage;
        public int SelectedStage
        {
            set
            {
                Map1.SetActive(value == 2);
                Map2.SetActive(value == 3);
                Map3.SetActive(value == 4);
                selectedStage = value;
            }
            get
            {
                return selectedStage;
            }
        }

        private void Start()
        {
            SelectedStage = 2;
        }

        public void StartGame()
        {
            SceneManager.LoadScene(selectedStage);
#if UNITY_ANDROID
            SceneManager.LoadScene("TouchGameplay", LoadSceneMode.Additive);
#else
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
#endif
        }
    }
}
