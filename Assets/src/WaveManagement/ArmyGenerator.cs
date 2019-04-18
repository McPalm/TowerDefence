using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WaveManagement
{
    [CreateAssetMenu(fileName ="Army Generator", menuName ="Army Generator", order=1)]
    public class ArmyGenerator : ScriptableObject
    {
        public List<ArmyCell> Enemies;
        

        public Army GetArmy(int level, int complexity, int waves, int seed)
        {
            var rng = new System.Random(seed);

            var options = new List<ArmyCell>();
            for(int i = 0; i < complexity; i++)
            {
                if(i == 0)
                {
                    var list = new List<ArmyCell>(Enemies.Where(cell => cell.complexity == 0));
                    options.Add(list[rng.Next(list.Count)]);
                }
                else
                {
                    var list = new List<ArmyCell>(Enemies.Where(cell => cell.complexity < 2));
                    options.Add(list[rng.Next(list.Count)]);
                }
            }
                
            var army = CreateInstance<Army>();
            army.waves = new List<Army.Wave>();
            for(int i = 0; i < waves; i++)
            {
                var max = i+1 < complexity ? i+1 : complexity;
                var wave = new Army.Wave();
                
                wave.units.Add(options[rng.Next(max)].GetUnit(level + i / 3));
                army.waves.Add(wave);
            }
            return army;
        }
    }
}
