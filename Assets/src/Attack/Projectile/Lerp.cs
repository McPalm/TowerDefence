using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack.Projectile
{
    public class Lerp : MonoBehaviour, IProjectile
    {
        public GameObject prefab;
        public float speed = 20f;
        public AudioClip fireSound;
        [Range(0f,1f)]
        public float volume = -5f;

        public void Shoot(GameObject target, Action<GameObject> action, Action<GameObject> action2)
        {
            StartCoroutine(Shoot(transform.position, target, speed, action));
        }

        IEnumerator Shoot(Vector3 source, GameObject target, float speed, Action<GameObject> action)
        {
            if (fireSound)
                this.PlaySound(fireSound);
            Vector3 end = target.transform.position;
            float distance = (source - end).magnitude;
            var projectile = Instantiate(prefab);
            projectile.AddComponent<KillAfterSeconds>().seconds = 6f;
            var progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime * speed / distance;
                projectile.transform.position = Vector3.Lerp(source, end, progress);
                yield return null;
                if (!target)
                {
                    var e = HitscanUtility.At(projectile.transform.position, 0.1f);
                    if (e)
                    {
                        action(e);
                        progress = 2f;
                    }
                }
            }
            
            if(target)
                action(target);
                
            Destroy(projectile);
        }
    }
}