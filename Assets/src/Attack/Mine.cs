using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Attack
{
    public class Mine : MonoBehaviour
    {
        public int damage = 100;
        public float radius = 1f;
        public int maxTargets = 1;
        bool active = true;
        public float lifetime = 60f;

        public float stunDuration = 0f;

        private void FixedUpdate()
        {
            lifetime -= Time.fixedDeltaTime;
            if (active == false)
                return;
            var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, Vector2.zero);
            if (hit.transform)
            {
                if (radius == 0f)
                {
                    hit.transform.GetComponent<Enemy>().Strike(damage, true);
                    if (stunDuration > 0f)
                        hit.transform.GetComponent<Movement.Mobile>().Stun(stunDuration);
                    Destroy(gameObject);
                }
                else
                {
                    Explode(hit.transform.gameObject);
                }
            }
            if (lifetime < 0f)
            {
                Destroy(gameObject);
            }
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
            Destroy(gameObject);
        }
    }
}