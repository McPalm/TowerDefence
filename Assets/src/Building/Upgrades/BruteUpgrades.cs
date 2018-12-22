using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;

namespace Building.Upgrades
{
    public class BruteUpgrades : AUpgrade
    {
        int level = 0;
        int critRank;
        int speedRank;
        int damageRank;
        public GameObject ShockEffect;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if(level == 0)
                {
                    if(critRank == 5 && speedRank == 5 && damageRank == 5)
                        return FinalUpgrade();

                    var upgrades = new List<UpgradeFormat>();
                    if (critRank < 5)
                        upgrades.Add(CritUpgrade());
                    if (speedRank < 5)
                        upgrades.Add(SpeedUpgrade());
                    if (damageRank < 5)
                        upgrades.Add(DamageUpgrade());
                    return upgrades.ToArray();

                }
                return new UpgradeFormat[0];
            }
        }

        void Start() => SunkCost = 40;

        UpgradeFormat CritUpgrade()
        {
            return new UpgradeFormat()
            {
                name = $"+5% Crit Chance ({critRank}/5)",
                cost = 10 * (critRank + 1),
                Upgrade = () =>
                {
                    critRank++;
                    GetComponent<DirectDamage>().critChance = RankRank(critRank) * .05f;
                },
            };
        }

        UpgradeFormat SpeedUpgrade()
        {
            return new UpgradeFormat()
            {
                name = $"Attack Speed",
                cost = 10 * (speedRank + 1),
                Upgrade = () =>
                {
                    speedRank++;
                    GetComponent<Turret>().attackSpeed = 1f + speedRank * .2f;
                },
            };
        }

        UpgradeFormat[] SpeedAndCritRank()
        {
            return new UpgradeFormat[]
            {
                CritUpgrade(), SpeedUpgrade(),
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
                    GetComponent<DirectDamage>().damage = 100 + RankRank(damageRank) * 20;
                }
            };
        }

        UpgradeFormat[] FinalUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "x3 Damage",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<DirectDamage>().damage *= 3;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "Shockwave",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var shock = gameObject.AddComponent<Shockwave>();
                        shock.damage = 200;
                        shock.size = .9f;
                        shock.prefab = ShockEffect;
                        GetComponent<Turret>().FindEffects();
                        level++;
                    },
                },
            };
        }
    }
}