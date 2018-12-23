using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Score
{
    public class Lives : MonoBehaviour
    {

        [SerializeField]
        int lives = 10;

        public LifeEvent OnChangeLife;

        private void Start()
        {
            if (DifficultySelector.Difficulty == Difficulty.easy)
                HP *= 2;
            if (DifficultySelector.Difficulty == Difficulty.medium)
                HP = HP * 3 / 2;
        }

        public int HP
        {
            get
            {
                return lives;
            }

            set
            {
                lives = value;
                OnChangeLife.Invoke(value);
                if (lives == 0)
                {
                    Debug.Log("GAME OVER " + FindObjectOfType<WaveManagement.WaveManager>().CurrentWave);
                    FindObjectOfType<Menu.GameOver>().Lose();
                }
            }
        }

        [System.Serializable]
        public class LifeEvent : UnityEvent<int> { }
    }
}
