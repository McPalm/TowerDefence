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
                else
                    Debug.Log("Lost a life at wave " + FindObjectOfType<WaveManagement.WaveManager>().CurrentWave);
            }
        }

        [System.Serializable]
        public class LifeEvent : UnityEvent<int> { }
    }
}
