using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;

namespace Building.Upgrades
{
    public class SplashUpgrades : AUpgrade
    {
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
                if (durationRank + powerRank + rangeRank + splashRank == 15)
                    return UltimateUpgrade();

                var upgrades = new List<UpgradeFormat>();

                if (powerRank < 5)
                    upgrades.Add(PowerUpgrade());
                if (frostTower && durationRank < 5)
                    upgrades.Add(DurationUpgrade());
                if (!frostTower && splashRank < 5)
                    upgrades.Add(SplashUpgrade());
                if (rangeRank < 5)
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
                    $"Dmg: {damage}%\n" +
                    $"Slow: {(int)(100f - slow.speedFactor * 100f)}% for {slow.duration}s";
                return $"Spd: {speed}\n" +
                    $"Dmg: {damage}";

            }
        }

        // Use this for initialization
        void Start() => SunkCost = 35;

        UpgradeFormat[] Level1()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Poison Milk",
                    cost = 10,
                    Upgrade = () =>
                    {
                        var poison = gameObject.AddComponent<PoisonEffect>();
                        poison.damage = 100;
                        poison.duration = 5;
                        GetComponent<Turret>().FindEffects();
                        frostTower = false;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "Frosty Milk",
                    cost = 10,
                    Upgrade = () =>
                    {
                        var slow = gameObject.AddComponent<SlowEffect>();
                        slow.speedFactor = .75f;
                        slow.duration = 2f;
                        GetComponent<Turret>().FindEffects();
                        frostTower = true;
                        level++;
                    },
                }
            };
        }

        int powerRank;
        int durationRank;
        int rangeRank;
        int splashRank;

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
                        slow.speedFactor = .75f - .04f * powerRank;
                    }
                    else
                    {
                        var poison = GetComponent<PoisonEffect>();
                        poison.damage = 100 + powerRank * 34;
                        poison.duration = 5f + powerRank * .5f;
                    }
                }
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
                    GetComponent<Turret>().distance = 2.5f + rangeRank * .2f;
                }
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
                }
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
                }
            };
        }

        UpgradeFormat[] UltimateUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Splash Vial",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var slow = GetComponent<SlowEffect>();
                        if (slow)
                        {
                            slow.radius = 1.5f;
                        }
                        else
                        {
                            GetComponent<PoisonEffect>().radius = 3f;
                        }
                        level++;
                    }
                },
                new UpgradeFormat()
                {
                    name = "Express Delivery",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<Turret>().attackSpeed *= 2f;
                        GetComponent<Turret>().distance = 5.5f;

                        var slow = GetComponent<SlowEffect>();
                        if (slow)
                        {
                            slow.speedFactor = .5f;
                        }
                        else
                        {
                            GetComponent<PoisonEffect>().duration = 3f;
                            GetComponent<PoisonEffect>().damage = 400;
                        }

                        level++;
                    }
                }
            };
        }


        UpgradeFormat[] Poison2()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Splash Vial",
                    cost = 160,
                    Upgrade = () =>
                    {
                        GetComponent<PoisonEffect>().radius = .5f;
                        splash = true;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "x3 Poison Damage",
                    cost = 100,
                    Upgrade = () =>
                    {
                        GetComponent<PoisonEffect>().damage *= 3;
                        level++;
                    },
                }
            };
        }

        UpgradeFormat[] Frost2()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Splash Vial",
                    cost = 480,
                    Upgrade = () =>
                    {
                        GetComponent<SlowEffect>().radius = .5f;
                        splash = true;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "Better Slow",
                    cost = 350,
                    Upgrade = () =>
                    {
                        GetComponent<SlowEffect>().speedFactor = .5f;
                        GetComponent<SlowEffect>().duration = 8f;
                        level++;
                    },
                }
            };
        }

        UpgradeFormat[] Splash3()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Larger Area",
                    cost = 1000,
                    Upgrade = () =>
                    {
                        var slow = GetComponent<SlowEffect>();
                        var poison = GetComponent<PoisonEffect>();
                        if(slow)
                            slow.radius += .5f;
                        if(poison)
                            poison.radius += .5f;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "Double Duration",
                    cost = 1000,
                    Upgrade = () =>
                    {
                        var slow = GetComponent<SlowEffect>();
                        var poison = GetComponent<PoisonEffect>();
                        if(slow)
                            slow.duration += 2f;
                        if(poison)
                        {
                            poison.duration *=2;
                            poison.damage *= 2;
                        }
                        level++;
                    },
                }
            };
        }

        UpgradeFormat[] Direct3()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "x2 Speed and Range",
                    cost = 1000,
                    Upgrade = () =>
                    {
                        GetComponent<Turret>().distance *= 2f;
                        GetComponent<Turret>().attackSpeed *= 2f;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "Double Duration",
                    cost = 1000,
                    Upgrade = () =>
                    {
                        var slow = GetComponent<SlowEffect>();
                        var poison = GetComponent<PoisonEffect>();
                        if(slow)
                            slow.duration += 2f;
                        if(poison)
                        {
                            poison.duration *=2;
                            poison.damage *= 2;
                        }
                        level++;
                    },
                }
            };
        }
    }
}

