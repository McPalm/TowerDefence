using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveManagement;

namespace Progression
{
    public class EventTile : Tile
    {
        public override Army Army => army;
        public override int Level => _level;
        public override int Stage => stage;

        public int _level;
        public int stage;
        public Army army;
        public GameObject Treasure;

        public Unlock Unlocks;

        public override bool Controlled
        {
            get
            {
                return !Treasure.activeSelf;
            }
            set
            {
                Treasure.SetActive(!Controlled);
            }
        }

        
    }
}
