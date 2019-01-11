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

        void Start() => SunkCost = 40;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 1)
                    return new UpgradeFormat[0];
                if (bounceRank + critRank + damageRank + rangeRank == 15)
                    return FinalUpgrade();

                var upgrades = new List<UpgradeFormat>();
                upgrades.Add(BounceUpgrade());
                upgrades.Add(DamageUpgrade());
                upgrades.Add(RangeUpgrade());
                return upgrades.ToArray();

            }
        }

        UpgradeFormat BounceUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Bounce",
                cost = 10 * (bounceRank + 1),
                Upgrade = () =>
                {
                    bounceRank++;
                    GetComponent<Bouncing>().targets = RankRank(bounceRank) + 3;
                    GetComponent<Bouncing>().bounceSpeed = 12f + RankRank(bounceRank);
                },
                maxRank = bounceRank == 5,
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
                maxRank = critRank == 5,
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
                },
                maxRank = damageRank == 5,
            };
        }

        UpgradeFormat[] FinalUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "x2 Speed",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<Turret>().attackSpeed *= 2f;
                        var bounce = GetComponent<Bouncing>();
                        bounce.speed *= 1.5f;
                        bounce.bounceSpeed *= 1.5f;
                        level++;
                        MarkFinal();
                    },
                },
                new UpgradeFormat()
                {
                    name = "+2 Range",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var turrent = GetComponent<Turret>();
                        turrent.distance += 2f;
                        level++;
                        MarkFinal();
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
                },
                maxRank = rangeRank == 5,
            };
        }
    }
}
