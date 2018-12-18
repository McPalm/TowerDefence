using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack.Aura
{
    public class HyperSpeed : MonoBehaviour, IHitEffect
    {
        public float power;
        public float procChance = 0f;
        float procCooldown;

        SpriteRenderer Sprot;
        float active = 0f;

        // Use this for initialization
        void Start()
        {
            FindObjectOfType<HyperUI>().Add(this);
            Sprot = GetComponent<SpriteRenderer>();
        }

        void OnDestroy()
        {
            FindObjectOfType<HyperUI>()?.Remove(this);
        }

        internal void Activate(float duration = 8f)
        {
            var buffer = GetComponent<Buffer>();
            buffer.Apply(buffer.range, power, duration);
            active = duration;
        }

        public void OnHit(GameObject o)
        {
            if(procCooldown < 0f && Random.value < procChance)
            {
                Activate(5f);
                procCooldown = 15f;
            }
        }

        private void Update()
        {
            procCooldown -= Time.deltaTime;
            if(active > 0f)
            {
                
                active -= Time.deltaTime;
                Sprot.color = active > 0f ? Pulse() : Color.white;
            }
            
        }

        Color Pulse()
        {
            return Color.Lerp(Color.white, new Color(.2f, .8f, .5f), .5f + .5f * Mathf.Cos(Time.timeSinceLevelLoad * 5f));
        }
    }
}