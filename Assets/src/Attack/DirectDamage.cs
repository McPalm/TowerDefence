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
            }
            else
                o.GetComponent<Enemy>().Strike(damage, armorPiercing);
        }
        
        public void OnHit2(GameObject o)
        {
            Debug.Log("offtarget?");
            if (Random.value < CritChance)
            {
                o.GetComponent<Enemy>().Strike(offTargetDamage * 3, armorPiercing);
            }
            else
                o.GetComponent<Enemy>().Strike(offTargetDamage, armorPiercing);
        }
    }
}