using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveManagement;
using Attack;
using Movement;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Analytics
{
    public class WaveObserver : MonoBehaviour
    {
        public Text HudOutput;
        

        public UnityEvent OnReport;


        // Use this for initialization
        void Start()
        {
            WaveInProgress = false;
            var waveManager = FindObjectOfType<WaveManager>();
            waveManager.OnStartWave.AddListener(StartObserving);
            waveManager.OnStartDowntime.AddListener(StopObserving);
            waveManager.OnFinalWaveDefeated.AddListener(StopObserving);

            var score = FindObjectOfType<Score.Lives>();
            score.OnChangeLife.AddListener(i => LivesLost++);
        }

        public float BestDistance { private set; get; } = 0f;
        bool WaveInProgress { set; get; } = false;
        public int LivesLost { private set; get; } = 0;
        bool lastFrame = false;

        void StartObserving()
        {
            WaveInProgress = true;
            BestDistance = 0f;
            LivesLost = 0;
        }


        void StopObserving()
        {
            WaveInProgress = false;
        }


        private void Update()
        {
            if (WaveInProgress || lastFrame)
            {
                foreach (Enemy enemy in Enemy.Enemies)
                {
                    var mobile = enemy.GetComponent<Mobile>();
                    BestDistance = mobile.location > BestDistance ? mobile.location : BestDistance;
                }
                if (HudOutput)
                {
                    HudOutput.text = $"dist: {BestDistance}" +
                        $"\nlost: {LivesLost}";
                }
                if (lastFrame && !WaveInProgress)
                    OnReport.Invoke();
                lastFrame = WaveInProgress;
                   
            }
        }

    }
}