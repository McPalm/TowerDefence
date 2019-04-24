using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Score
{
    public class Lives : MonoBehaviour
    {

        [SerializeField]
        int lives = 25;

        public LifeEvent OnChangeLife;
        bool alive = true;

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
                if (alive && lives <= 0)
                {
                    Debug.Log("GAME OVER " + FindObjectOfType<WaveManagement.WaveManager>().CurrentWave);
                    FindObjectOfType<Menu.GameOver>().Lose();
                    alive = false;
                }
            }
        }

        void Start()
        {
            HP = SaveData.Current.PlayerLevel * 2 + 8;
            OnChangeLife.Invoke(HP);
        }

        [System.Serializable]
        public class LifeEvent : UnityEvent<int> { }
    }
}
