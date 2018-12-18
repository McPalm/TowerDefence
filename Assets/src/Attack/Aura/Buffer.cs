using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Attack.Aura
{
    public class Buffer : MonoBehaviour
    {
        public float speed = 1.15f;
        public float range;
        internal float crit = 0f;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(Pulse());
        }

        IEnumerator Pulse()
        {
            while(true)
            {
                yield return new WaitForSeconds(1f);
                Apply(range, speed, 1.1f);
            }
        }
        
        internal void Apply(float range, float speed, float duration)
        {
            foreach (var item in Buff.AllBuffs)
            {
                if ((item.transform.position - transform.position).sqrMagnitude <= range * range)
                {
                    if (crit > 0f)
                        item.ApplyCrit(duration, crit);
                    item.Apply(duration, speed);
                }
            }
        }
    }
}
