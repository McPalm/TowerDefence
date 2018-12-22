using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class LastBossSpawning : MonoBehaviour
    {
        public GameObject flyer;
        public GameObject tank;
        public GameObject swarmer;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(3.5f);
            while (true)
            {
                yield return Spawn(tank, 1, 0);
                yield return new WaitForSecondsRealtime(3f + Random.value);
                yield return Spawn(flyer, 3, 1f);
                yield return new WaitForSecondsRealtime(3f + Random.value);
                yield return Spawn(swarmer, 12, .4f);
                yield return new WaitForSecondsRealtime(3f + Random.value);
            }
        }

        IEnumerator Spawn(GameObject prefab, int qty, float interval)
        {
            var myMobile = GetComponent<Mobile>();
            for (int i = 0; i < qty; i++)
            {
                var fab = Instantiate(prefab);
                var mobile = fab.GetComponent<Mobile>();
                mobile.location = myMobile.location;
                mobile.mesh = myMobile.mesh;
                yield return new WaitForSecondsRealtime(interval * Random.value);
            }
        }
    }
}
