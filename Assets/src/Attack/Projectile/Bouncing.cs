using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Attack.Projectile
{
    public class Bouncing : MonoBehaviour, IProjectile
    {
        public GameObject prefab;
        public float speed = 20f;
        public float bounceSpeed = 10f;
        public float bounceRange = 1f;
        public int targets = 3;
        public bool greedyBounce = false;
        public bool returnToSender = false;

        public AudioClip twang;
        public AudioClip smack;
        [Range(0f, 1f)]
        public float twangVolume = .5f;
        [Range(0f, 1f)]
        public float smackVolume = .5f;

        public void Shoot(GameObject target, Action<GameObject> action, Action<GameObject> action2)
        {
            StartCoroutine(Shoot(transform.position, target, speed, action, action2));
        }

        IEnumerator Shoot(Vector3 source, GameObject target, float speed, Action<GameObject> action, Action<GameObject> action2)
        {
            HashSet<GameObject> alreadyStruck = new HashSet<GameObject>();
            var projectile = Instantiate(prefab);
            projectile.transform.position = source;
            projectile.AddComponent<KillAfterSeconds>().seconds = 6f;
            var tosser = source; // wont be replaced
            float pitch = .65f + UnityEngine.Random.value * .2f;
            float lastTwang = 0f;
             
            // funciton for finding more targets, dependednt on already struck
            Func <GameObject> nextTarget;
            if (greedyBounce)
            {
                
                nextTarget = () =>
                {
                    GameObject o = null;
                    float bestRange = 0f;
                    float sqrDisFromTosser = (projectile.transform.position - tosser).sqrMagnitude;
                    foreach (var item in Enemy.Enemies)
                    {
                        if (alreadyStruck.Contains(item.gameObject)) continue;
                        var range = (item.transform.position - projectile.transform.position).sqrMagnitude;
                        var tosserFactor = (item.transform.position - tosser).sqrMagnitude < sqrDisFromTosser ? .5f : 1f;
                        if (range <= bounceRange * bounceRange && range * UnityEngine.Random.value * tosserFactor > bestRange)
                        {
                            
                            bestRange = range * tosserFactor;
                            o = item.gameObject;
                        }
                    };
                    return o;
                };
            }
            else
            {
                nextTarget = () =>
                {
                    GameObject o = null;
                    float bestRange = bounceRange * bounceRange;
                    foreach (var item in Enemy.Enemies)
                    {
                        if (alreadyStruck.Contains(item.gameObject)) continue;
                        var range = (item.transform.position - projectile.transform.position).sqrMagnitude;
                        if (range <= bestRange)
                        {
                            bestRange = range;
                            o = item.gameObject;
                        }
                    };
                    return o;
                };
            }
            

            for (int bounce = 0; bounce < targets; bounce++)
            {
                if(bounce > 0)
                    target = nextTarget();
                if (target == null)
                    break;

                source = projectile.transform.position;
                Vector3 end = target.transform.position;
                float distance = (source - end).magnitude;
                var progress = 0f;
                var localSpeed = bounce == 0 ? speed : bounceSpeed;
                projectile.transform.right = end - source;

                while (progress < 1f)
                {
                    progress += Time.deltaTime * localSpeed / distance;
                    projectile.transform.position = Vector3.Lerp(source, end, progress);
                    yield return null;
                    if (!target)
                    {
                        var e = HitscanUtility.At(projectile.transform.position, 0.1f);
                        if (e)
                        {
                            if (bounce % 2 == 0)
                            {
                                e.PlaySound(twang, twangVolume, pitch);
                                lastTwang = Time.realtimeSinceStartup;
                                pitch *= 1.1f;
                            }
                            e.PlaySound(smack, smackVolume * .6f);
                            
                            action(e);
                            alreadyStruck.Add(e);
                            progress = 2f;
                            action = action2;
                        }
                    }
                }

                if (target)
                {
                    if (bounce % 2 == 0)
                    {
                        target.PlaySound(twang, twangVolume, pitch);
                        lastTwang = Time.realtimeSinceStartup;
                        pitch *= 1.1f;
                    }
                    target.PlaySound(smack, smackVolume);
                    alreadyStruck.Add(target);
                    action(target);
                    action = action2;
                }

                
            }

            if(returnToSender)
            {
                source = projectile.transform.position;
                float distance = (source - tosser).magnitude;
                var progress = 0f;
                projectile.transform.right = tosser - source;

                while (progress < 1f)
                {
                    progress += Time.deltaTime * speed / distance;
                    projectile.transform.position = Vector3.Lerp(source, tosser, progress);
                    yield return null;
                }
            }
                    

            Destroy(projectile);
        }
    }
}
