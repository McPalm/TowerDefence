using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class Stun : MonoBehaviour, IHitEffect
    {
        public float duration;
        public int frequency;

        public void OnHit(GameObject o)
        {
            if(Roll())
            {
                o.GetComponent<Movement.Mobile>().Stun(duration);
            }
        }
        public void OnHit2(GameObject o) => OnHit(o);

        int tick = 3;

        bool Roll()
        {
            tick--;
            if(tick == 0)
            {
                tick = Random.Range(1, frequency * 2);
                return true;
            }
            return false;
        }
    }
}