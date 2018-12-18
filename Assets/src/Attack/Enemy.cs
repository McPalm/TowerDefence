using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class Enemy : MonoBehaviour
    {
        private static HashSet<Enemy> enemies;


        public int hits = 2;
        public bool armor;

        int hp;
        int maxhp;

        internal void Poison(int totalDamage, int ticks)
        {
            int tickDamage = totalDamage / ticks;
            StartCoroutine(PoisonTick(tickDamage, ticks));
        }

        internal void Heal(int heal)
        {
            hp += heal;
            if (hp > maxhp) hp = maxhp;
            var r = GetComponent<SpriteRenderer>();
            if (r)
                r.color = new Color(1f, HealthPercentage, HealthPercentage);
        }

        public IEnumerator PoisonTick(int tickDamage, float ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                yield return new WaitForSeconds(1f);
                Strike(tickDamage, true);
            }
        }

        public float HealthPercentage
        {
            get
            {
                return (float)hp / (float)maxhp;
            }
        }

        public int HP
        {
            get
            {
                return hp;
            }
        }

        internal static HashSet<Enemy> Enemies
        {
            get
            {
                if (enemies == null)
                    enemies = new HashSet<Enemy>();
                return enemies;
            }
        }

        // Use this for initialization
        protected void Start()
        {
            hp = hits * 100;
            maxhp = hp;
        }

        public void Boost(float factor)
        {
            if(factor > 1f)
            {
                var spawner = GetComponent<SpawnOnDeath>();
                if(spawner)
                {
                    spawner.boost = factor;
                }
                var speedFactor = .8f + .2f * factor;
                GetComponent<Movement.Mobile>().speed *= speedFactor;
            }
        }

        public void Strike(int damage, bool ArmourPiercing = false)
        {
            if(armor && ArmourPiercing == false) damage /= 2;
            if (damage <= 0) return;
            hp -= damage;
            var r = GetComponent<SpriteRenderer>();
            if(r)
                r.color = new Color(1f, HealthPercentage, HealthPercentage);


            foreach (var item in GetComponents<IOnStruck>())
                item.OnHurt(damage, HealthPercentage);
            if (hp <= 0)
            {
                foreach (var item in GetComponents<IOnKilled>())
                    item.OnKilled();
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            Enemies.Add(this);
        }

        private void OnDisable()
        {
            Enemies.Remove(this);
        }

        public interface IOnStruck
        {
            void OnHurt(int damage, float healthPercentage);
        }

        public interface IOnKilled
        {
            void OnKilled();
        }
    }
}
