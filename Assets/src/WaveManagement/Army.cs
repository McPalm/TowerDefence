using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveManagement
{
    [CreateAssetMenu(fileName = "New Army", menuName = "Tower Defence/New Army", order = 1)]
    public class Army : ScriptableObject
    {
        public List<Wave> waves;

        [System.Serializable]
        public class Wave
        {
            public int Bounty
            {
                get
                {
                    int v = 0;
                    foreach (var unit in units)
                    {
                        v += unit.Bounty;
                    }
                    return v;
                }
            }
            public int expectedWealth;
            [Range(0, 5)]
            public int BackgroundWaves = 0;
            [Range(0, 4)]
            public int BackgroundQty = 1;
            [Range(-10f, 10f)]
            public float BackgroundOffset = 0f;
            public List<Unit> units = new List<Unit>();
        }

        [System.Serializable]
        public class Unit
        {
            public GameObject enemy;
            public SpawnRate spawnRate;
            public int qty;
            public int Bounty { get
                {
                    if (enemy != null)
                    {
                        var bounty = enemy.GetComponent<Score.Bounty>();
                        if (bounty)
                        {
                            if (bounty.nickle)
                                return qty / 2;
                            return bounty.worth * qty;
                        }
                    }
                    return 0;
                }
            }
        }

        [System.Serializable]
        public enum SpawnRate
        {
            very_sparse = -2, sparse = -1, moderate = 0, dense = 1, ultra_dense = 2
        }
    }
}
