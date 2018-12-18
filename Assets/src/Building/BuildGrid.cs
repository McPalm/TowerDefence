using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public class BuildGrid : MonoBehaviour
    {
        Dictionary<Vector2Int, GameObject> obstructions;

        private void Awake()
        {
            obstructions = new Dictionary<Vector2Int, GameObject>();
        }


        private Transform min, max;
        private void Start()
        {
            var borders = FindObjectOfType<Borders>();
            min = borders.min;
            max = borders.max;
        }

        internal void Remove(GameObject gameObject)
        {
            obstructions.Remove(V2For(gameObject.transform.position));
        }

        public bool Add(GameObject o)
        {
            var v2 = V2For(o.transform.position);
            if (obstructions.ContainsKey(v2))
                return false;
            obstructions.Add(v2, o);
            return true;
        }

        public bool SpaceAvailable(Vector3 location) => !obstructions.ContainsKey(V2For(location)) && InsideField(location);
        public bool InsideField(Vector3 location) => location.x >= min.position.x && location.y >= min.position.y && location.x <= max.position.x && location.y <= max.position.y;
        public GameObject ObjectAt(Vector3 location)
        {
            GameObject o = null;
            obstructions.TryGetValue(V2For(location), out o);
            return o;
        }


        Vector2Int V2For(Vector3 v3) => new Vector2Int((int)v3.x, (int)v3.y);
    }
}
