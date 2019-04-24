using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveManagement;

namespace Progression
{
    abstract public class Tile : MonoBehaviour
    {
        abstract public Army Army { get; }
        abstract public int Level { get; }
        abstract public int Stage { get; }

        public Vector2Int Position => Vector2Int.FloorToInt(transform.position);
        virtual public bool Controlled { get; set; } = false;
    }
}
