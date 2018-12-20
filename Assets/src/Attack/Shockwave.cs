using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class Shockwave : MonoBehaviour, IHitEffect
    {
        public int damage;
        float chance = .3f;
        public float size;
        public GameObject prefab;

        public void OnHit(GameObject o)
        {
            if (Random.value < chance)
            {
                StartCoroutine(Dothething(o));
                chance -= .4f;
            }
            else
                chance += .4f;
        }
        public void OnHit2(GameObject o) => OnHit(o);

        IEnumerator Dothething(GameObject target)
        {
            var struckSet = new HashSet<GameObject>();

            Vector2 delta = (target.transform.position - transform.position).normalized;
            delta *= size;
            Vector2 targetPosition = target.transform.position;

            for (int i = 0; i < 3; i++)
            {
                var hits = Physics2D.CircleCastAll(targetPosition, size, Vector2.zero);
                foreach (var hit in hits)
                {
                    if (struckSet.Contains(hit.transform.gameObject))
                        continue;
                    hit.transform.GetComponent<Enemy>().Strike(damage);
                    struckSet.Add(hit.transform.gameObject);
                }

                var fab = Instantiate(prefab);
                fab.transform.position = targetPosition;

                targetPosition += delta;

                yield return new WaitForSeconds(.1f);
            }
        }
    }
}