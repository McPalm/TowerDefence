using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Movement;

namespace Attack
{
    public class Turret : MonoBehaviour
    {
        public float distance = 4;
        public float attackSpeed = 1f;
        private float cooldown;
        public TargetPriority targetPriority;
        public bool LockTarget { set; get; } = true;

        Enemy lastTarget;

        [HideInInspector]
        public bool freezeTower = false;

        Action<GameObject, Action<GameObject>, Action<GameObject>> shoot;
        Action<GameObject> effect;
        Action<GameObject> effect2;

        private void Start()
        {
            cooldown = 1f / attackSpeed;
            FindEffects();
            SetProjectile(GetComponent<Projectile.IProjectile>());
        }

        public void SetProjectile(Projectile.IProjectile projectile)
        {
            
            if (projectile != null)
                shoot = projectile.Shoot;
            else
                shoot = (o, a, a2) => a(o);
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

        private void Update()
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0f)
            {
                var target = FindTarget();
                if (target)
                {
                    shoot(target.gameObject, o => effect(o), o => effect2(o));
                    var buf = GetComponent<IBuff>();
                    var buff = buf != null ? buf.Speed : 1f;
                    if (buff < 1f) buff = 1f;
                    cooldown += 1f / attackSpeed / buff;
                    GetComponent<SpriteRenderer>().flipX = transform.position.x > target.transform.position.x;
                }
                else
                    cooldown = 0f;
            }   
        }

        private Enemy FindTarget()
        {
            if (LockTarget && lastTarget && InDistance(lastTarget))
                return lastTarget;

            var inRange = Enemy.Enemies.Where(InDistance).ToList();
            if (inRange.Count == 0)
                return null;
            switch(targetPriority)
            {
                case TargetPriority.first:
                    inRange.Sort(First);
                    break;
                case TargetPriority.last:
                    inRange.Sort(Last);
                    break;
                case TargetPriority.strongest:
                    inRange.Sort(Strongest);
                    break;
                case TargetPriority.weakest:
                    inRange.Sort(Weakest);
                    break;
                case TargetPriority.random:
                    lastTarget = inRange[UnityEngine.Random.Range(0, inRange.Count)];
                    return lastTarget;
            }
            if(freezeTower)
            {
                Debug.Log("Freexe Tower!");
                foreach (var enemy in inRange)
                {
                    var move = enemy.GetComponent<Mobile>();
                    if(!move.Slowed)
                    {
                        lastTarget = enemy;
                        return enemy;
                    }
                }
            }
            lastTarget = inRange.FirstOrDefault();
            return lastTarget;

        }

        protected virtual bool InDistance(Enemy target)
        {
            return (transform.position - target.transform.position).sqrMagnitude <= distance * distance;
        }

        private int First(Enemy a, Enemy b)
        {
            return Mobile.ComparePosition(a.GetComponent<Mobile>(), b.GetComponent<Mobile>());
        }

        private int Strongest(Enemy a, Enemy b)
        {
            if (a.HP == b.HP)
                return (First(a, b));
            return b.HP - a.HP;
        }

        private int Weakest(Enemy a, Enemy b)
        {
            if (a.HP == b.HP)
                return (First(a, b));
            return a.HP - b.HP;
        }

        private int Last(Enemy a, Enemy b)
        {
            return Mobile.ComparePosition(b.GetComponent<Mobile>(), a.GetComponent<Mobile>());
        }

        [System.Serializable]
        public enum TargetPriority
        {
            first = 0,
            last,
            strongest,
            weakest,
            notSlowed,
            random,
        }
    }
}
