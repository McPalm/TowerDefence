
using WaveManagement;
using UnityEngine;

namespace Progression
{
    class RandomTile : Tile
    {
        override public int Level => Mathf.CeilToInt(transform.position.magnitude * .7f);
        public int Seed { get; set; }
        int Complexity => 3 + Level / 3;
        int Waves => 2 + Level;

        public override Army Army => Map.Instance.Generator.GetArmy(Level, Complexity, Waves, Seed);

        public override int Stage => Rando(Position);


        static int Rando(Vector2Int v2)
        {
            return (int)(Mathf.PerlinNoise(v2.x * .5f + 50 + .5f, v2.y * .5f + 50 + .5f) * 5f);
        }

    }
}
