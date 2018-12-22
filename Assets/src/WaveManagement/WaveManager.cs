using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WaveManagement
{
    public class WaveManager : MonoBehaviour
    {
        public Army[] armies;
        public Army BackgroundWaves;
        List<Army.Wave> waves;

        float SpawnRateFactor = 1f;

        public int debugStart = 0;
        public int CurrentWave
        {
            get
            {
                return currentWave;
            }
        }

        public string WaveName
        {
            get
            {
                int count = currentWave - 1;

                foreach (var army in armies)
                {
                    if(count < army.waves.Count)
                    {
                        return $"{army.name}:{count}";
                    }
                    count -= army.waves.Count;
                }

                return "N/A";
            }
        }

        public string StateToString
        {
            get
            {
                if (State == S_Downtime)
                    return "Downtime";
                else if (State == S_RunningWave)
                    return "Playing";
                return "Invalid state";
            }
        }

        public Army.Wave NextWave
        {
            get
            {
                return waves[currentWave];
            }
        }

        public UnityEvent OnStartWave;
        public UnityEvent OnStartDowntime;
        public UnityEvent OnFinalWaveDefeated;

        System.Action State;

        int runningSpawns = 0;
        int currentWave = 0;

        void Awake()
        {
            State = S_Downtime;
            waves = new List<Army.Wave>();
            foreach (var army in armies)
            {
                foreach (var wave in army.waves)
                {
                    waves.Add(wave);
                }
            }

            switch(DifficultySelector.Difficulty)
            {
                case Difficulty.easy:
                    SpawnRateFactor = 1.35f;
                    break;
                case Difficulty.medium:
                    SpawnRateFactor = 1.2f;
                    break;
                case Difficulty.hard:
                default:
                    SpawnRateFactor = 1f;
                    break;
            }

            currentWave = debugStart;
        }

        void Start()
        {
            var wallet = FindObjectOfType<Score.Wallet>();
            wallet.Add(waves[currentWave].expectedWealth - wallet.Money);
            OnStartDowntime.AddListener(() => wallet.Add(waves[currentWave].expectedWealth - wallet.TotalWorth));
        }

        void Update()
        {
            State();
        }

        void S_Downtime()
        {
            if (Input.GetButtonDown("Next"))
                StartWave();
        }

        void S_RunningWave()
        {
            if(runningSpawns == 0 && Attack.Enemy.Enemies.Count == 0)
            {
                State = S_Downtime;
                if (currentWave == waves.Count)
                {
                    OnFinalWaveDefeated.Invoke();
                    FindObjectOfType<Menu.GameOver>().Win();
                }
                else
                {
                    OnStartDowntime.Invoke();
                    
                }
            }
        }

        void StartWave()
        {
            if (currentWave < waves.Count)
            {
                var wave = waves[currentWave];
                int bonusCash = 0;
                if(currentWave+1 < waves.Count && wave.filler != null)
                {
                    bonusCash = waves[currentWave + 1].expectedWealth - wave.Bounty - FindObjectOfType<Score.Wallet>().TotalWorth;
                }
                StartCoroutine(SpawnWave(wave, bonusCash));
                StartCoroutine(SpawnWave(BackgroundWaves.waves[currentWave / 5], 0, .5f));
                currentWave++;
                State = S_RunningWave;
                OnStartWave.Invoke();
            }
        }

        IEnumerator SpawnWave(Army.Wave wave, int bonusCash, float waveGap = 0f)
        {
            runningSpawns++;

            foreach (var unit in wave.units)
            {
                for (int i = 0; i < unit.qty; i++)
                {
                    var enemy = Instantiate(unit.enemy);
                    enemy.transform.position = new Vector3(-25, -25);
                    yield return new WaitForSeconds(delayFor(unit.spawnRate) * SpawnRateFactor);
                }
                if (waveGap > 0f)
                    yield return new WaitForSeconds(waveGap);
            }
            
            runningSpawns--;
        }

        IEnumerator SimpleSpawnRoutine(GameObject o, float delay, int qty)
        {
            runningSpawns++;
            for (int i = 0; i < qty; i++)
            {
                var enemy = Instantiate(o);
                enemy.transform.position = new Vector3(-25, -25);
                yield return new WaitForSeconds(delay);
            }
            runningSpawns--;
        }

        float delayFor(Army.SpawnRate rate)
        {
            switch(rate)
            {
                case Army.SpawnRate.dense:
                    return .25f;
                case Army.SpawnRate.sparse:
                    return 1f;
                case Army.SpawnRate.moderate:
                    return .5f;
                case Army.SpawnRate.very_sparse:
                    return 5f;
                case Army.SpawnRate.ultra_dense:
                    return .1f;
            }
            Debug.LogWarning("Borp");
            return .5f;
        }
    }
}