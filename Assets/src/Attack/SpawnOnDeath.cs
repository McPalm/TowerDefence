using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

namespace Attack
{
    public class SpawnOnDeath : MonoBehaviour, Enemy.IOnKilled
    {
        public GameObject prefab;
        public int qty = 3;
        public float spread = 0.1f;
        public float boost = 1f;
        public GameObject[] moreFabs;

        public void OnKilled()
        {
            

            float position = GetComponent<Mobile>().location;
            var mesh = GetComponent<Mobile>().mesh;

            if (prefab)
            {
                for (int i = 0; i < qty; i++)
                {
                    var fab = Instantiate(prefab);
                    var enemy = fab.GetComponent<Enemy>();
                    var mobile = fab.GetComponent<Mobile>();
                    if (mobile)
                    {
                        mobile.location = position;
                        mobile.mesh = mesh;
                    }
                    if(enemy)
                    {
                        enemy.Level = GetComponent<Enemy>().Level;
                    }
                    fab.transform.position = transform.position;
                    position -= spread;
                }
            }
            if(moreFabs.Length > 0)
            {
                foreach (var item in moreFabs)
                {
                    var fab = Instantiate(item);
                    var mobile = fab.GetComponent<Mobile>();
                    if (mobile)
                    {
                        mobile.location = position;
                        mobile.mesh = mesh;
                    }
                    fab.transform.position = transform.position;
                    position -= spread;
                }
            }
        }
    }
}
