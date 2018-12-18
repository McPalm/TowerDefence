using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

namespace Attack
{
    public class SpawnOnHurt : MonoBehaviour, Enemy.IOnStruck
    {

        public GameObject prefab;
        public int spawns;

        int spawnCounter;

        public void OnHurt(int damage, float healthPercentage)
        {
            float threshold = 1f / (spawns + 1);
            threshold *= spawns - spawnCounter - 1;
            if (healthPercentage < threshold && spawnCounter < spawns)
                Spawn();
        }

        void Spawn()
        {
            var fab = Instantiate(prefab);
            fab.GetComponent<Mobile>().location = GetComponent<Mobile>().location;
            fab.GetComponent<Mobile>().mesh = GetComponent<Mobile>().mesh;
            fab.transform.position = transform.position;
            spawnCounter++;
        }

    }
}