using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;

namespace Building.Upgrades
{
    public class SplashUpgrades : AUpgrade
    {
        public GameObject IceProjectile;
        public GameObject PoisonProjectile;

        int level = 0;
        bool frostTower;
        bool splash;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 0)
                    return Level1();
                if (level == 2)
                    return new UpgradeFormat[0];
                if (durationRank + powerRank + rangeRank + splashRank + speedRank == 15)
                    return UltimateUpgrade();

                var upgrades = new List<UpgradeFormat>();

                upgrades.Add(PowerUpgrade());
                if (frostTower)
                    upgrades.Add(SpeedUpgrade());
                if (!frostTower)
                    upgrades.Add(SplashUpgrade());
                upgrades.Add(RangeUpgrade());
                return upgrades.ToArray();
            }
        }

        public override string Attributes
        {
            get
            {
                float speed = 1f;
                float damage = 100;
                var poison = GetComponent<PoisonEffect>();
                var slow = GetComponent<SlowEffect>();
                var turret = GetComponent<Attack.Turret>();
                if (turret)
                    speed = turret.attackSpeed;
                var dd = GetComponent<Attack.DirectDamage>();
                if (dd)
                    damage = dd.damage;
                var sd = GetComponent<Attack.SplashDamage>();
                if (sd)
                    damage = sd.DirectDamage;
                var mine = GetComponent<Attack.MineLayer>();
                if (mine)
                {
                    damage = mine.damage;
                }

                if (poison)
                    return $"Spd: {speed}\n" +
                    $"Dmg: {damage} + {poison.damage} for {poison.duration}s";
                else if(slow)
                    return $"Spd: {speed}\n" +
                    $"Dmg: {damage}\n" +
                    $"Slow: {(int)(100f - slow.speedFactor * 100f)}% for {slow.duration}s";
                return $"Spd: {speed}\n" +
                    $"Dmg: {damage}";

            }
        }

        // Use this for initialization
        void Start()
        {
            GetComponent<Turret>().LockTarget = false;
            SunkCost = 40;
        }

        UpgradeFormat[] Level1()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Venom Splash",
                    cost = 10,
                    Upgrade = () =>
                    {
                        var poison = gameObject.AddComponent<PoisonEffect>();
                        poison.damage = 100;
                        poison.duration = 5;
                        var turret = GetComponent<Turret>();
                        turret.FindEffects();
                        turret.PoisonTower = true;
                        frostTower = false;
                        GetComponent<Attack.Projectile.Lerp>().prefab = PoisonProjectile;
                        level++;
                        var animator = GetComponent<Animator>();
                        animator.SetBool("Masked", true);
                        animator.SetTrigger("Upgrade");
                    },
                },
                new UpgradeFormat()
                {
                    name = "Frost Potion",
                    cost = 10,
                    Upgrade = () =>
                    {
                        var slow = gameObject.AddComponent<SlowEffect>();
                        slow.speedFactor = .7f;
                        slow.duration = 2f;
                        var turret = GetComponent<Turret>();
                        turret.FindEffects();
                        turret.FreezeTower = true;
                        frostTower = true;
                        GetComponent<Attack.Projectile.Lerp>().prefab = IceProjectile;
                        level++;
                    },
                }
            };
        }

        int powerRank;
        int durationRank;
        int rangeRank;
        int splashRank;
        int speedRank;

        UpgradeFormat PowerUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Stronger",
                cost = 10 * (powerRank + 1),
                Upgrade = () =>
                {
                    powerRank++;
                    var slow = GetComponent<SlowEffect>();
                    if(slow)
                    {
                        slow.speedFactor = .7f - .02f * powerRank;
                        slow.duration = 2f + .5f * RankRank(powerRank);
                    }
                    else
                    {
                        var poison = GetComponent<PoisonEffect>();
                        poison.damage = 100 + RankRank(powerRank) * 25;
                        poison.duration = 5f + powerRank * .5f;
                    }
                },
                maxRank = powerRank == 5,
            };
        }

        UpgradeFormat RangeUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Range",
                cost = 10 * (rangeRank + 1),
                Upgrade = () =>
                {
                    rangeRank++;
                    GetComponent<Turret>().distance = 2.5f + RankRank(rangeRank) * .2f;
                },
                maxRank = rangeRank == 5,
            };
        }

        UpgradeFormat SplashUpgrade()
        {
            return new UpgradeFormat
            {
                name = "Splash",
                cost = 10 * (splashRank + 1),
                Upgrade = () =>
                {
                    splashRank++;
                    var poison = GetComponent<PoisonEffect>();
                    poison.radius = .4f + splashRank * .25f;
                },
                maxRank = splashRank == 5,
            };
        }

        UpgradeFormat DurationUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Duration",
                cost = 10 * (durationRank + 1),
                Upgrade = () =>
                {
                    durationRank++;
                    var slow = GetComponent<SlowEffect>();
                    if (slow)
                    {
                        slow.duration = 4f + durationRank;
                    }
                },
                maxRank = durationRank == 5,
            };
        }

        
        UpgradeFormat SpeedUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Speed",
                cost = 10 * (speedRank + 1),
                Upgrade = () =>
                {
                    speedRank++;
                    var turret = GetComponent<Turret>();
                    turret.attackSpeed = .65f + RankRank(speedRank) * .07f;
                },
                maxRank = speedRank == 5,
            };
        }

        UpgradeFormat[] UltimateUpgrade()
        {
            var list = new List<UpgradeFormat>();
            var slow = GetComponent<SlowEffect>();
            var poison = GetComponent<PoisonEffect>();

            if (poison)
                list.Add(new UpgradeFormat()
                {
                    name = "Rapid Working Poison",
                    cost = 500,
                    Upgrade = () =>
                    {
                        poison.duration = 2.5f;
                        level++;
                        MarkFinal();
                    },
                });
            if (slow)
                list.Add(new UpgradeFormat()
                {
                    name = "Splash Vial",
                    cost = 500,
                    Upgrade = () =>
                    {
                        slow.radius = 1.5f;
                        level++;
                        MarkFinal();
                    }
                });
            
                list.Add(new UpgradeFormat()
                {
                    name = "Express Delivery",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var turret = GetComponent<Turret>();
                        turret.attackSpeed *= 2f;
                        turret.distance = 5.5f;
                        var lerp = GetComponent<Attack.Projectile.Lerp>();
                        lerp.speed *= 2f;

                        if (slow)
                        {
                            slow.speedFactor = .5f;
                            slow.radius = .1f;
                        }
                        else
                        {
                            poison.radius += 1f;
                            poison.damage += 30;
                            turret.distance += 1f;
                            
                        }
                        GetComponent<DirectDamage>().damage += 50;
                        level++;
                        MarkFinal();
                    }
                });



            return list.ToArray();
        }
    }
}

