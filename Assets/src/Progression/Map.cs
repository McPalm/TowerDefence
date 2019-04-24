using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Progression
{
    public class Map : MonoBehaviour
    {
        static public Map Instance { get; private set; }

        Dictionary<Vector2Int, Tile> Tiles = new Dictionary<Vector2Int, Tile>();
        public List<ProgressionTileData> Scenes;
        public ProgressionTileData ScenedataFor(int i)
        {
            i = i < 0 ? -i : i;
            return Scenes[i % Scenes.Count];
        }

        public WaveManagement.ArmyGenerator Generator;

        void Awake()
        {
            Instance = this;
        }
        
        void Start()
        {
            var rando = GetComponentInChildren<RandomTile>();
            var rng = new System.Random(0);
            foreach (var tile in GetComponentsInChildren<Tile>())
            {
                if (Tiles.ContainsKey(tile.Position))
                    Debug.LogError($"Tiles already contains a tile for {tile.Position}", tile);
                else
                    Tiles.Add(tile.Position, tile);
            }
            for (int x = -50; x < 51; x++)
            {
                for (int y = -50; y < 51; y++)
                {
                    if(false == Tiles.ContainsKey(new Vector2Int(x, y)))
                    {
                        var tile = Instantiate(rando);
                        tile.transform.position = new Vector3(x +.5f, y +.5f, 0);
                        Tiles.Add(tile.Position, tile);
                        tile.Seed = rng.Next();
                    }
                }
            }
            foreach (var tile in Tiles.Values)
            {
                var data = ScenedataFor(tile.Stage);
                tile.GetComponent<SpriteRenderer>().sprite = data.MapSprite;
            }
        }
        public Tile Get(Vector2 position) => Get(Vector2Int.FloorToInt(position));

        public Tile Get(Vector2Int position)
        {
            if (Tiles.ContainsKey(position))
                return Tiles[position];
            return null;
        }
    }
}