using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class SlowEffect : MonoBehaviour, IHitEffect
    {
        public float speedFactor;
        public float duration;
        public float radius;

        public void OnHit(GameObject o)
        {
            if(radius == 0f)
                o.GetComponent<Movement.Mobile>().ApplySlow(speedFactor, duration);
            else
            {
                var hits = Physics2D.CircleCastAll(o.transform.position, radius, Vector2.zero);
                foreach (var hit in hits)
                {
                    hit.transform.GetComponent<Movement.Mobile>().ApplySlow(speedFactor, duration);
                }
            }
        }

        public void OnHit2(GameObject o) => OnHit(o);
    }
}
