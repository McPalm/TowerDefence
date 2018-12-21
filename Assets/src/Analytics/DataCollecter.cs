using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WaveManagement;

namespace Analytics
{
    public class DataCollecter : MonoBehaviour
    {
        WaveObserver observer;
        WaveManager manager;
        Score.Lives lives;

        public Text finalReport;

        List<WaveAnalytics> waves;

        // Use this for initialization
        void Start()
        {
            observer = FindObjectOfType<WaveObserver>();
            observer.OnReport.AddListener(CompileWave);

            manager = FindObjectOfType<WaveManager>();
            manager.OnFinalWaveDefeated.AddListener(FinalReport);

            lives = FindObjectOfType<Score.Lives>();

            waves = new List<WaveAnalytics>();
        }

        void CompileWave()
        {
            waves.Add(
            new WaveAnalytics()
            {
                name = manager.WaveName,
                lost = observer.LivesLost,
                distanceMoved = observer.BestDistance,
            });
            if (lives.HP < 1)
                FinalReport();
        }

        void FinalReport()
        {
            string text = "";
            waves.Sort();
            waves.ForEach(w => text += w.ToString() + "\n");
            finalReport.text = text;
        }

        public class WaveAnalytics : IComparable<WaveAnalytics>
        {
            public string name;
            public int lost;
            public float distanceMoved;

            static public int Compare(WaveAnalytics x, WaveAnalytics y)
            {
                if (x.lost == y.lost)
                    return x.distanceMoved < y.distanceMoved ? -1 : 1;
                return x.lost - y.lost;
            }

            public int CompareTo(WaveAnalytics other)
            {
                return Compare(this, other);
            }

            public override string ToString()
            {
                return $"{name} lost:{lost} disc:{distanceMoved}";
            }
        }
    }
}