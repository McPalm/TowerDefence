using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Attack
{
    public class Mine : MonoBehaviour
    {
        public int damage = 100;
        public float radius = 1f;
        public int maxTargets = 1;
        public float lifetime = 60f;
        public float stunDuration = 0f;

        public MineEvent OnDestroy;


        private void Start()
        {
            StartCoroutine(Routine());
        }

        IEnumerator Routine()
        {
            float killTime = Time.timeSinceLevelLoad + lifetime;
            while(Time.timeSinceLevelLoad < killTime)
            {
                yield return new WaitForFixedUpdate();
                var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, Vector2.zero);
                if (hit.transform)
                {
                    var animator = GetComponent<Animator>();
                    
                    if (animator)
                        animator.SetTrigger("Explode");

                    if (radius == 0f)
                    {
                        hit.transform.GetComponent<Enemy>().Strike(damage, true);
                        if (stunDuration > 0f)
                            hit.transform.GetComponent<Movement.Mobile>().Stun(stunDuration);
                        break;
                    }
                    else
                    {
                        Explode(hit.transform.gameObject);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(.5f);
            OnDestroy.Invoke(this);
            Destroy(gameObject);
        }

        public void Explode(GameObject target)
        {
            var struck = new HashSet<GameObject>();
            struck.Add(target);
            var hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject != target)
                    struck.Add(hit.transform.gameObject);
                if (struck.Count >= maxTargets)
                    break;
            }
            foreach (var item in struck)
            {
                item.GetComponent<Enemy>().Strike(damage, true);
                if (stunDuration > 0f)
                    if(item == target)
                        item.GetComponent<Movement.Mobile>().Stun(stunDuration);
                    else
                        item.GetComponent<Movement.Mobile>().Stun(stunDuration/2f);
            }
            
        }
        [System.Serializable]
        public class MineEvent : UnityEvent<Mine> { }
    }
}