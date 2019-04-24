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
                    var list = new List<ArmyCell>(Enemies
                        .Where(cell => cell.complexity == 0)
                        .Where(cell => cell.minimumLevel <= level));
                    options.Add(list[rng.Next(list.Count)]);
                }
                else if(i == complexity - 1)
                {
                    var list = new List<ArmyCell>(Enemies
                        .Where(cell => cell.complexity > 0)
                        .Where(cell => cell.minimumLevel <= level + waves / 3 && cell.minimumLevel > 1));
                    if (list.Count > 0)
                        options.Add(list[rng.Next(list.Count)]);
                    else
                        options.Add(Enemies
                            .Where(cell => cell.complexity > 0)
                            .Where(cell => cell.minimumLevel <= level)
                            .First());
                }
                else
                {
                    var list = new List<ArmyCell>(Enemies
                        .Where(cell => cell.complexity < 2)
                        .Where(cell => cell.minimumLevel <= level));
                    options.Add(list[rng.Next(list.Count)]);
                }
            }
                
            var army = CreateInstance<Army>();
            army.waves = new List<Army.Wave>();
            for(int i = 0; i < waves; i++)
            {
                int waveLevel = level + i / 2;
                var max = i < waves / 2 ? options.Count - 1 : options.Count;
                var wave = new Army.Wave();
                if (i == waves - 1)
                {
                    wave.units.Add(options[options.Count - 1].GetUnit(waveLevel));
                    wave.units.Add(options[0].GetUnit(level));
                }
                else if (i < complexity - 1)
                    wave.units.Add(options[i].GetUnit(waveLevel));
                else
                    wave.units.Add(options[rng.Next(max)].GetUnit(waveLevel, rng.Next()));
                army.waves.Add(wave);
            }
            return army;
        }
    }
}
