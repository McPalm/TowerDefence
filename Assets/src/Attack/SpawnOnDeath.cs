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

        public void OnKilled()
        {
            

            float position = GetComponent<Mobile>().location;
            var mesh = GetComponent<Mobile>().mesh;

            for (int i = 0; i < qty; i++)
            {
                var fab = Instantiate(prefab);
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
