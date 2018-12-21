using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class Stun : MonoBehaviour, IHitEffect
    {
        public float duration;
        public int frequency;
        bool lastRoll = false;

        public void OnHit(GameObject o)
        {
            if(Roll())
            {
                o.GetComponent<Movement.Mobile>().Stun(duration);
            }
        }

        public void OnHit2(GameObject o)
        {
            if (lastRoll)
                o.GetComponent<Movement.Mobile>().Stun(duration);
        }

        int tick = 0;

        bool Roll()
        {
            tick--;
            if(tick <= 0)
            {
                tick = Random.Range(1, frequency * 2);
                lastRoll = true;
                return true;
            }
            lastRoll = false;
            return false;
        }
    }
}