using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaveManagement
{
    public class WavePicker : MonoBehaviour
    {
        
        WaveSpawner[] Spawners;
        public List<Wave> Waves;

        int waveCount = 0;
        float difficulty = 1f;
        public Text text;
        float delay = 1f;

        private void Start()
        {
            Spawners = GetComponentsInChildren<WaveSpawner>();
        }

        // Update is called once per frame
        void Update()
        {
            if (delay > 0f)
                delay -= Time.deltaTime;
            else if (Attack.Enemy.Enemies.Count == 0)
            {
                foreach (var item in Spawners)
                    if (item.IsSpawning) return;
                NextWave();
                delay = 1f;
            }
        }

        public void NextWave()
        {
            
            Wave wave = Waves[waveCount%Waves.Count];
            wave.Spawn(difficulty);
            if(waveCount > 0)
            {
                if(waveCount < 10)
                    Score.Wallet.Instance.Add(30);
                else if (waveCount < 20)
                    Score.Wallet.Instance.Add(100);
                else if (waveCount < 30)
                    Score.Wallet.Instance.Add(300);
                else if (waveCount < 40)
                    Score.Wallet.Instance.Add(1000);
                else
                    Score.Wallet.Instance.Add(3000);
            }
                

            waveCount++;
            if (waveCount % Waves.Count == 0)
                difficulty *= 2f;
            text.text = "Wave " + waveCount;
        }
        
        [System.Serializable]
        public class Wave
        {
            public WaveSpawner[] waves;
            public float delay;
            public void Spawn(float difficulty)
            {
                for (int i = 0; i < waves.Length; i++)
                {
                    waves[i].SpawnWave(i * delay, difficulty);
                }
            }
        }
    }
}