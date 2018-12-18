using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack.Projectile
{
    static public class HitscanUtility
    {
        static public GameObject At(Vector2 position, float radius)
        {
            var hits = Physics2D.CircleCastAll(position, radius, Vector2.zero);
            if (hits.Length > 0)
                return hits[0].transform.gameObject;
            return null;
        }
    }
}