using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveManagement
{
    public class WaveSpawner : MonoBehaviour
    {
        public GameObject prefab;

        public int quantity;
        public float interval;
        public GameObject spawnPoint;
        public bool IsSpawning { get { return activeCount > 0; } }

        int activeCount = 0;

        public void SpawnWave(float startTimer = 0f, float difficulty = 1f)
        {
            float hp = difficulty;
            float speed = interval * Mathf.Pow(1.2f, difficulty-1f);
            activeCount++;
            StartCoroutine(SpawnSequence(startTimer, speed, quantity, hp, prefab));
        }

        private IEnumerator SpawnSequence(float delay, float interval, int qty, float hpfactor, GameObject prefab)
        {
            if (qty < 1) qty = 1;
            if (delay > 0)
                yield return new WaitForSeconds(delay);
            for (int i = 0; i < qty; i++)
            {
                var fab = Instantiate(prefab);
                fab.transform.position = spawnPoint.transform.position;
                fab.GetComponent<Attack.Enemy>().Boost(hpfactor);
                yield return new WaitForSeconds(interval);
            }
            activeCount--;
        }
    }
}
