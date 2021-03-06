using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WaveManagement
{
    public class WaveManager : MonoBehaviour
    {
        public Army[] armies;
        List<Army.Wave> waves;
        public ArmyGenerator generator;

        float SpawnRateFactor = 1f;

        public int debugStart = 0;
        public int CurrentWave
        {
            get
            {
                return currentWave;
            }

        }
        public int WaveCount => waves.Count;

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
            var army = Progression.Challenger.Army;
            if (army)
                armies = new Army[] { army };
            else if (generator)
            {
                armies = new Army[] { generator.GetArmy(1, 4, 10, Random.Range(0, int.MaxValue)) };
            }

            State = S_Downtime;
            waves = new List<Army.Wave>();
            int armyCount = armies.Length;

            for (int i = 0; i < armyCount; i++)
            {
                army = armies[i];
                foreach (var wave in army.waves)
                {
                    waves.Add(wave);
                }
            }
#if UNITY_EDITOR
            currentWave = debugStart;
#endif
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

        internal void StartWave()
        {
            if (State != S_Downtime) return; // reject if not in the correct state
            if (currentWave < waves.Count)
            {
                var wave = waves[currentWave];
                StartCoroutine(SpawnWave(-wave.BackgroundOffset, wave)); // Main Wave
                currentWave++;
                State = S_RunningWave;
                OnStartWave.Invoke();
            }
            // FindObjectOfType<MusicPlayer>().Play();
        }

        IEnumerator SpawnWave(float startDelay, Army.Wave wave, float waveGap = 0f, int repetitions = 1)
        {
            runningSpawns++;
            if (startDelay > 0f)
                yield return new WaitForSeconds(startDelay);
            for (int r = 0; r < repetitions; r++)
            {
                foreach (var unit in wave.units)
                {


                    for (int i = 0; i < unit.qty; i++)
                    {
                        var enemy = Instantiate(unit.enemy);
                        enemy.transform.position = new Vector3(-25, -25);
                        enemy.GetComponent<Attack.Enemy>().Level = unit.level;
                        
                        yield return new WaitForSeconds(delayFor(unit.spawnRate) * SpawnRateFactor);
                    }
                    if (waveGap > 0f)
                        yield return new WaitForSeconds(waveGap);
                }
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