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

        int selectedStage;
        public int SelectedStage
        {
            set
            {
                Map1.SetActive(value == 2);
                Map2.SetActive(value == 3);
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
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}
