using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;
using Attack.Projectile;

namespace Building.Upgrades
{
    public class BouncingTowerUpgrade : AUpgrade
    {
        int level = 0;
        int bounceRank;
        int rangeRank;
        int critRank = 0;
        int damageRank;

        void Start() => SunkCost = 30;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 1)
                    return new UpgradeFormat[0];
                if (bounceRank + critRank + damageRank + rangeRank == 15)
                    return FinalUpgrade();

                var upgrades = new List<UpgradeFormat>();

                if (bounceRank < 5)
                    upgrades.Add(BounceUpgrade());
                if (damageRank < 5)
                    upgrades.Add(DamageUpgrade());
                if (rangeRank < 5)
                    upgrades.Add(RangeUpgrade());
                return upgrades.ToArray();

            }
        }

        UpgradeFormat BounceUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Bounce " + (bounceRank + 1),
                cost = 10 * (bounceRank + 1),
                Upgrade = () =>
                {
                    bounceRank++;
                    GetComponent<Bouncing>().targets = RankRank(bounceRank) + 3;
                    GetComponent<Bouncing>().bounceSpeed = 12f + RankRank(bounceRank);
                },
            };
        }

        UpgradeFormat CritUpgrade()
        {
            return new UpgradeFormat()
            {
                name = $"Crit",
                cost = 10 * (critRank + 1),
                Upgrade = () =>
                {
                    critRank++;
                    GetComponent<DirectDamage>().critChance = critRank * .05f;
                },
            };
        }

        UpgradeFormat DamageUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Damage",
                cost = 10 * (damageRank + 1),
                Upgrade = () =>
                {
                    damageRank++;
                    var damage = GetComponent<DirectDamage>();
                    damage.damage = 50 + RankRank(damageRank) * 10;
                    damage.offTargetDamage = 50 + RankRank(damageRank) * 10;
                }
            };
        }

        UpgradeFormat[] FinalUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "x3 Speed",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<Turret>().attackSpeed *= 3f;
                        var bounce = GetComponent<Bouncing>();
                        bounce.speed *= 1.5f;
                        bounce.bounceSpeed *= 1.5f;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "Stunning",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var stun = gameObject.AddComponent<Stun>();
                        stun.frequency = 5;
                        stun.duration = 4f;
                        GetComponent<Turret>().FindEffects();
                        level++;
                    },
                },
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
    }
}
