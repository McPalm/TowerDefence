using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class AimedTurret : MonoBehaviour
    {
        public float speed = 5f;
        public float spread = 0f;
        public float piercing = 0;


        float cooldown = 1f;
        public float projectileSpeed = 15f;

        static float perlin = 0f;
        float myPerlin = 0f;

        public GameObject bullet;
        public AudioClip SoundEffect;

        bool downtime = false;

        System.Action<GameObject> effect;
        System.Action<GameObject> effect2;

        private void Start()
        {
            myPerlin = perlin;
            perlin += 10f;
            FindEffects();

            var wm = FindObjectOfType<WaveManagement.WaveManager>();
            wm.OnStartWave.AddListener(() => downtime = false);
            wm.OnStartDowntime.AddListener(() => downtime = true);
        }

        public void FindEffects()
        {
            effect = null;
            effect2 = null;
            foreach (var item in GetComponents<IHitEffect>())
            {
                effect += item.OnHit;
                effect2 += item.OnHit2;
            }
        }

        Vector2 AimAt
        {
            get
            {
                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(!downtime)
                AutoFire();
        }

        void AutoFire()
        {
            cooldown -= Time.deltaTime;
            if(cooldown < 0f)
            {
                Fire();
                var boff = GetComponent<Aura.Buff>().Speed;
                if (boff < 1f) boff = 1f;
                cooldown = 1f / speed / boff;
            }
        }

        void Fire()
        {
            if (SoundEffect)
                this.PlaySound(SoundEffect, .16f, 2.3f + Random.value * .38f);
            var target = AimAt;
            if(spread > 0f)
            {
                var rng = new Vector2((.5f - Mathf.PerlinNoise(Time.realtimeSinceStartup * 2f, myPerlin)) * spread, (.5f - Mathf.PerlinNoise(Time.realtimeSinceStartup * 2f, myPerlin + 5f)) * spread);
                rng *= ((Vector2)transform.position - target).magnitude;
                target += rng;
            }
            StartCoroutine(BulletLerp(target));
        }

        IEnumerator BulletLerp(Vector2 target)
        {
            var struck = new HashSet<GameObject>();
            var effect = this.effect;

            Vector2 source = transform.position;
            var bullet = Instantiate(this.bullet);
            bullet.transform.position = transform.position;
            bullet.transform.right = target - source;

            Vector2 direction = (target - source).normalized * projectileSpeed * .02f;
            bullet.AddComponent<KillAfterSeconds>().seconds = 2f;

            for (int i = 0; i < 120; i++) // two seconds lifetime
            {
                var hits = Physics2D.LinecastAll(bullet.transform.position, (Vector2)bullet.transform.position + direction);
                bullet.transform.position += (Vector3)direction;
                foreach (var hit in hits)
                {
                    if (struck.Contains(hit.transform.gameObject))
                        continue;
                    effect(hit.transform.gameObject);
                    effect = effect2;
                    struck.Add(hit.transform.gameObject);
                    if (struck.Count > piercing)
                    {
                        i = 99999999;
                        break;
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            Destroy(bullet);
        }
    }
}
