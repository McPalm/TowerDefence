using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class GameOver : MonoBehaviour
    {
        public GameObject BadBackground;
        public GameObject GoodBackground;

        bool lost = false;
        bool won = false;


        public void Lose()
        {
            if (won == false)
            {
                lost = true;
                BadBackground.SetActive(true);
                FindObjectOfType<MusicPlayer>().Stop();
            }
        }

        public void Win()
        {
            if(lost == false)
            {
                won = true;
                StartCoroutine(ShowAfterSeconds(GoodBackground, 5f));
                int score = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name);
                if(score < (int)DifficultySelector.Difficulty)
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, (int)DifficultySelector.Difficulty);
                PlayerPrefs.Save();
                SaveData.Current.Win();
            }
        }

        public void QuitToMain()
        {
            SceneManager.LoadScene(2);
        }

        public void Retry()
        {
            Reset.ReloadScene();
        }

        IEnumerator ShowAfterSeconds(GameObject o, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            o.SetActive(true);
            FindObjectOfType<MusicPlayer>()?.Stop();
        }
    }
}
