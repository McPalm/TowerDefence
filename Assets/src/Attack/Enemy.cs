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

        int poisonLeft = 0;

        Coroutine poisonRoutine;

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
            hp += heal;
            if (hp > maxhp) hp = maxhp;
        }

        public IEnumerator PoisonTick(int totalDamage, float duration)
        {
            var sprite = GetComponent<SpriteRenderer>();
            if(sprite)
                sprite.color = new Color(0.2f, 1f, .1f);
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
            if (sprite)
                sprite.color = Color.white;
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
            switch(DifficultySelector.Difficulty)
            {
                case Difficulty.easy:
                    hp = hits * 75;
                    break;
                case Difficulty.medium:
                    hp = hits * 90;
                    break;
                case Difficulty.hard:
                default:
                    hp = hits * 100;
                    break;
            }
            
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
            if(armor && ArmourPiercing == false) damage /= 3;
            if (damage <= 0) return;
            hp -= damage;

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
