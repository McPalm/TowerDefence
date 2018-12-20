using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack.Projectile
{
    public class LineAttack : MonoBehaviour, IProjectile
    {
        public GameObject prefab;
        public float range;

        public void Shoot(GameObject target, Action<GameObject> action, Action<GameObject> action2)
        {
            StartCoroutine(ShowLine(target, action));
        }

        IEnumerator ShowLine(GameObject target, Action<GameObject> action)
        {
            Vector3 endOfTheLine = (target.transform.position - transform.position).normalized;
            endOfTheLine *= range;
            endOfTheLine += transform.position;

            var fab = Instantiate(prefab);
            fab.transform.position = (transform.position + endOfTheLine) / 2;
            fab.transform.localScale = new Vector3(range, 1f, 1f);
            fab.transform.right = endOfTheLine - transform.position;

            var hits = Physics2D.LinecastAll(transform.position, endOfTheLine);
            foreach (var hit in hits)
            {
                action(hit.transform.gameObject);
            }

            yield return new WaitForSeconds(.1f);
            Destroy(fab);
        }
    }
}
