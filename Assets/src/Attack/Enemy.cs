using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class Enemy : MonoBehaviour
    {
        private static HashSet<Enemy> enemies;

        int level = 1;
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                CalculateMaxHP();
            }
        }
        public int hits = 2;
        public bool armor;

        int hploss;
        int maxhp;
        public int HP => maxhp - hploss;
        public float HealthPercentage => (float)HP / (float)maxhp;

        int poisonLeft = 0;

        Coroutine poisonRoutine;
        bool dead = false;

        public bool Poisoned => poisonLeft > 0;

        internal void Poison(int totalDamage, float duration)
        {
            if (poisonLeft < totalDamage)
            {
                if(poisonRoutine != null) StopCoroutine(poisonRoutine);
                poisonRoutine = StartCoroutine(PoisonTick(totalDamage, duration));
            }
        }

        internal void Heal(int heal)
        {
            hploss -= heal;
            hploss = hploss < 0 ? 0 : hploss;
        }

        public IEnumerator PoisonTick(int totalDamage, float duration)
        {
            int ticks = (int)(duration * 10);
            int tickDamage = totalDamage / ticks;
            int leftover = totalDamage - ticks * tickDamage;
            poisonLeft = totalDamage;

            for (int tick = 0; tick < ticks; tick++)
            {
                yield return new WaitForSeconds(.1f);
                int tikk = tickDamage;
                if(leftover > 0)
                {
                    leftover--;
                    tikk++;
                }
                Strike(tikk, true);
                poisonLeft -= tikk;
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
            CalculateMaxHP();
        }

        void CalculateMaxHP() => maxhp = (int)(hits * Mathf.Pow(1.2f, level-1) * 100);

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
            if (dead == false)
            {
                if (armor && ArmourPiercing == false) damage /= 2;
                if (damage <= 0) return;
                hploss += damage;

                foreach (var item in GetComponents<IOnStruck>())
                    item.OnHurt(damage, HealthPercentage);
                if (hploss >= maxhp)
                {
                    foreach (var item in GetComponents<IOnKilled>())
                        item.OnKilled();
                    Destroy(gameObject);
                    dead = true;
                }
            }
        }

        private void OnEnable() =>Enemies.Add(this);
        private void OnDisable() => Enemies.Remove(this);

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
