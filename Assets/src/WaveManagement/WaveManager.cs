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

        public int debugStart = 0;
        public int CurrentWave
        {
            get
            {
                return currentWave;
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

            currentWave = debugStart;
        }

        void Start()
        {
            var wallet = FindObjectOfType<Score.Wallet>();
            wallet.Add(waves[currentWave].expectedWealth - wallet.Money);
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
                    OnStartDowntime.Invoke();
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
                currentWave++;
                State = S_RunningWave;
                OnStartWave.Invoke();
            }
        }

        IEnumerator SpawnWave(Army.Wave wave, int bonusCash)
        {
            runningSpawns++;
            int need = 0;

            if (wave.filler != null && bonusCash > 0)
            {
                need = wave.filler.nickle ? bonusCash * 2 : bonusCash / wave.filler.worth;
                if(need > 10)
                    Debug.Log("Need " + need + " at wave " + currentWave);
                float rate = 5f / need;
                if (rate < .75f) rate = .75f;
                StartCoroutine(SimpleSpawnRoutine(wave.filler.gameObject, rate, 10));
                need -= 10;
                yield return new WaitForSeconds(5f);
            }

            foreach (var unit in wave.units)
            {
                for (int i = 0; i < unit.qty; i++)
                {
                    var enemy = Instantiate(unit.enemy);
                    enemy.transform.position = new Vector3(-25, -25);
                    yield return new WaitForSeconds(delayFor(unit.spawnRate));
                }
            }

            if(need > 0)
            {
                yield return new WaitForSeconds(2f);
                StartCoroutine(SimpleSpawnRoutine(wave.filler.gameObject, .25f, need));
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