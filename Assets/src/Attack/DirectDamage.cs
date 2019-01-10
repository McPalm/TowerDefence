using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class DirectDamage : MonoBehaviour, IHitEffect
    {
        public int damage = 100;
        public float critChance = 0f;
        public bool armorPiercing = false;
        public int offTargetDamage = 50;

        public AudioClip HitSound;
        [Range(0f, 1f)] public float hitVolume;
        public AudioClip CritSound;
        [Range(0f, 1f)] public float critVolume;

        float CritChance
        {
            get
            {
                return critChance + GetComponent<IBuff>().Crit;
            }
        }

        public void OnHit(GameObject o)
        {

            if (Random.value < CritChance)
            {
                o.GetComponent<Enemy>().Strike(damage * 3, armorPiercing);
                ObjectPooling.CritPool.Spawn(o.transform.position);
                if(CritSound)
                    gameObject.PlaySound(CritSound, critVolume);
            }
            else
            {
                o.GetComponent<Enemy>().Strike(damage, armorPiercing);
                if(HitSound)
                    gameObject.PlaySound(HitSound, hitVolume);
            }
        }
        
        public void OnHit2(GameObject o)
        {
            if (Random.value < CritChance)
            {
                o.GetComponent<Enemy>().Strike(offTargetDamage * 3, armorPiercing);
                ObjectPooling.CritPool.Spawn(o.transform.position);
            }
            else
                o.GetComponent<Enemy>().Strike(offTargetDamage, armorPiercing);
        }
    }
}