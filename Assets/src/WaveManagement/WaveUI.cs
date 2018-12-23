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

        public Button nextWaveButton;

        private void Start()
        {
            manager = FindObjectOfType<WaveManager>();
            manager.OnStartWave.AddListener(OnWaveStart);
            nextWaveButton.onClick.AddListener(StartWave);

            manager.OnStartDowntime.AddListener(() => nextWaveButton.gameObject.SetActive(true));
            manager.OnStartWave.AddListener(() => nextWaveButton.gameObject.SetActive(false));
        }

        void OnWaveStart()
        {
            foreach (var text in WaveText)
            {
                text.text = $"Wave {manager.CurrentWave} / {manager.WaveCount}";
            }
        }
        
        void StartWave()
        {
            manager.StartWave();
        }
    }
}