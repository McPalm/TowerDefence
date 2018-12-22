using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Attack
{
    public class Regeration : MonoBehaviour
    {
        public int hpPerSecond;

        private void Start()
        {
            StartCoroutine(RegenRoutine());
            
        }

        IEnumerator RegenRoutine()
        {
            var enemy = GetComponent<Enemy>();
            var frequency = .1f;
            if (DifficultySelector.Difficulty == Difficulty.medium)
                frequency = .12f;
            if (DifficultySelector.Difficulty == Difficulty.easy)
                frequency = .16f;
            while (true)
            {
                yield return new WaitForSeconds(frequency);
                enemy.Heal(hpPerSecond / 10);
            }
        }
    }
}