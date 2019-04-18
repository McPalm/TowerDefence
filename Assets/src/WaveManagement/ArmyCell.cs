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

        public Army.Unit GetUnit(int level)
        {
            return new Army.Unit
            {
                enemy = EnemyPrefab,
                qty = qty,
                spawnRate = spawnRate,
                level = level
            };
        }

        public Army.Unit GetUnit(int level, int modifier)
        {
            var ret = new Army.Unit
            {
                enemy = EnemyPrefab,
                qty = qty,
                spawnRate = spawnRate,
                level = level
            };
            switch (modifier % 4)
            {
                case 0:
                    if (level < 3)
                        break;
                    ret.qty *= 2;
                    ret.level -= 2;
                    if ((int)ret.spawnRate < 2)
                        ret.spawnRate++;
                    break;
                case 1:
                    if (qty < 2)
                        break;
                    ret.qty /= 2;
                    ret.level += 2;
                    if ((int)ret.spawnRate > -2)
                        ret.spawnRate--;
                    break;
            }

            return ret;
        }
    }
}