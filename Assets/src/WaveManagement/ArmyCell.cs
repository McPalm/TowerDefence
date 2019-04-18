using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveManagement
{
    [CreateAssetMenu(fileName = "New Cell", menuName = "Army Cell", order = 0)]
    public class ArmyCell : ScriptableObject
    {
        public int qty;
        public Army.SpawnRate spawnRate;
        public GameObject EnemyPrefab;
        [Range(0, 2)]
        public int complexity;

        public Army.Unit GetUnit()
        {
            return new Army.Unit
            {
                enemy = EnemyPrefab,
                qty = qty,
                spawnRate = spawnRate
            };
        }
    }
}