using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class OnHitSound : MonoBehaviour, IHitEffect
    {
        public AudioClip[] HitSound;
        public AudioClip[] SecondaryHitSound;
        [Range(0f,1f)]
        public float volume = 1f;

        public void OnHit(GameObject o)
        {
            if (HitSound.Length > 0)
            {
                if (o)
                    o.PlaySound(HitSound[Random.Range(0, HitSound.Length)], volume);
                else
                    gameObject.PlaySound(HitSound[Random.Range(0, HitSound.Length)], volume);
            }
        }

        public void OnHit2(GameObject o)
        {
            if (SecondaryHitSound.Length > 0)
            {
                if (o)
                    o.PlaySound(SecondaryHitSound[Random.Range(0, SecondaryHitSound.Length)], volume);
                else
                    gameObject.PlaySound(SecondaryHitSound[Random.Range(0, SecondaryHitSound.Length)], volume);
            }
        }
    }
}