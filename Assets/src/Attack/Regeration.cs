using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Attack
{
    public class Regeration : MonoBehaviour
    {
        public int hitsPerSecond;

        private void Start()
        {
            StartCoroutine(RegenRoutine());
        }

        IEnumerator RegenRoutine()
        {
            var enemy = GetComponent<Enemy>();

            while(true)
            {
                yield return new WaitForSeconds(1f);
                enemy.Heal(hitsPerSecond * 100);
            }
        }
    }
}