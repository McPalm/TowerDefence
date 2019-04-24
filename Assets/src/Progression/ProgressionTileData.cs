using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "Map Tile Data", menuName = "Map Tile Data", order = 0)]
    public class ProgressionTileData : ScriptableObject
    {
        public Sprite MapSprite;
        public Sprite UISprite;
        public string SceneName;
    }
}
