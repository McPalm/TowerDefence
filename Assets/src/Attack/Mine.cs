using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Attack
{
    public class Mine : MonoBehaviour
    {
        public int damage = 100;
        public float radius = 1f;
        bool active = true;
        public float lifetime = 60f;

        public float slowFactor = 1f;
        public float slowDuration = 0f;

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
                    if (slowDuration > 0f)
                        hit.transform.GetComponent<Movement.Mobile>().ApplySlow(slowFactor, slowDuration);
                    Destroy(gameObject);
                }
                else
                {
                    Explode();
                }
            }
            if(lifetime < 0f)
            {
                Destroy(gameObject);
            }
        }

        IEnumerator ExplodeIn(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Explode();
        }

        public void Explode()
        {
            var hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
            foreach (var hit in hits)
            {
                hit.transform.GetComponent<Enemy>().Strike(damage, true);
                if (slowDuration > 0f)
                    hit.transform.GetComponent<Movement.Mobile>().ApplySlow(slowFactor, slowDuration);
            }
            Destroy(gameObject);
        }
    }
}