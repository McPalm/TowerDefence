using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace WaveManagement
{
    public class WaveUI : MonoBehaviour
    {
        public Text[] WaveText;

        WaveManager manager;

        private void Start()
        {
            manager = FindObjectOfType<WaveManager>();
            manager.OnStartWave.AddListener(OnWaveStart);
        }

        void OnWaveStart()
        {
            foreach (var text in WaveText)
            {
                text.text = "Wave " + manager.CurrentWave;
            }
        }
        
    }
}