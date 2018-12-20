using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class PoisonEffect : MonoBehaviour, IHitEffect
    {
        public float duration;
        public int damage;
        public float radius = 0f;

        public void OnHit(GameObject o)
        {
            if(radius == 0f)
                o.GetComponent<Enemy>().Poison(damage, duration);
            else
            {
                var hits = Physics2D.CircleCastAll(o.transform.position, radius, Vector2.zero);
                foreach (var hit in hits)
                {
                    hit.transform.GetComponent<Enemy>().Poison(damage, duration);
                }
            }
        }
        public void OnHit2(GameObject o) => OnHit(o);
    }
}
