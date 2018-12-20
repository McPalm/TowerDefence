using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class SplashDamage : MonoBehaviour, IHitEffect
    {
        public int DirectDamage = 100;
        public int _SplashDamage = 100;
        public float radius;

        public void OnHit(GameObject o)
        {
            
            var hits = Physics2D.CircleCastAll(o.transform.position, radius, Vector2.zero);
            foreach (var hit in hits)
            {
                if(hit.transform.gameObject != o)
                    hit.transform.GetComponent<Enemy>().Strike(_SplashDamage);
            }
            o.GetComponent<Enemy>().Strike(DirectDamage);
        }
        public void OnHit2(GameObject o) => OnHit(o);
    }
}
