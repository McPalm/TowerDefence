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

        public AudioClip GameOverSound;
        public AudioClip VictorySound;


        public void Lose()
        {
            if (won == false)
            {
                lost = true;
                BadBackground.SetActive(true);
                Camera.main.PlaySoundUnscaled(GameOverSound);
            }
        }

        public void Win()
        {
            if(lost == false)
            {
                won = true;
                GoodBackground.SetActive(true);
                int score = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name);
                if(score < (int)DifficultySelector.Difficulty)
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, (int)DifficultySelector.Difficulty);

                gameObject.AddComponent<AudioSource>().clip = VictorySound;
                Camera.main.PlaySoundUnscaled(VictorySound);
            }
        }

        public void QuitToMain()
        {
            SceneManager.LoadScene(0);
        }

        public void Retry()
        {
            FindObjectOfType<PauseMenu>().ReloadScene();
        }
    }
}
