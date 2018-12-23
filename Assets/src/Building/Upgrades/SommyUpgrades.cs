using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;
using Attack.Aura;

namespace Building.Upgrades
{
    public class SommyUpgrades : AUpgrade
    {
        int level = 0;
        public AudioClip BoostSound;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 0)
                    return Level1();
                if (level == 2)
                    return new UpgradeFormat[0];
                if (rangeRank + speedRank + critRank == 15)
                    return UltimateUpgrade();

                var list = new List<UpgradeFormat>();
                if (rangeRank < 5)
                    list.Add(RangeUpgrade());
                if (speedRank < 5)
                    list.Add(SpeedUpgrade());
                if (critRank < 5)
                    list.Add(CritUpgrade());
                return list.ToArray();
            }
        }

        void Start() => SunkCost = 25;

        UpgradeFormat[] Level1()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Active Speed Boost",
                    cost = 60,
                    Upgrade = () =>
                    {
                        var boost = gameObject.AddComponent<HyperSpeed>();
                        boost.power = 2.1f;
                        boost.audioClip = BoostSound;
                        level++;
                    },
                }
            };
        }

        public override string Attributes
        {
            get
            {
                var buffer = GetComponent<Buffer>();

                float speed = buffer.speed - 1f + .005f;
                float crit = buffer.crit + .005f;

                string buffs = $"\nSpeed Buff {(int)(speed * 100f)}%";
                if(crit > .005f)
                    buffs += $"\nCrit Buff {(int)(crit * 100f)}%";
                return base.Attributes + buffs;
            }
        }

        int speedRank;
        int rangeRank;
        int critRank;

        UpgradeFormat SpeedUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Speed Buff",
                cost = 10 * (speedRank + 1),
                Upgrade = () =>
                {
                    speedRank++;
                    GetComponent<Turret>().attackSpeed = 1.5f + speedRank * .1f;
                    GetComponent<Buffer>().speed = 1.10f + speedRank * .04f;
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
                    if (rangeRank == 1)
                        GetComponent<Buffer>().range = 1.5f;
                    if (rangeRank == 3)
                        GetComponent<Buffer>().range = 2.1f;
                    if (rangeRank == 5)
                        GetComponent<Buffer>().range = 2.5f;
                    GetComponent<Turret>().distance = 2.5f + rangeRank * .2f;
                },
            };
        }

        UpgradeFormat CritUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Crit Buff",
                cost = 10 * (critRank + 1),
                Upgrade = () =>
                {
                    critRank++;
                    var buffer = GetComponent<Buffer>();
                    buffer.crit = .02f * critRank;
                }
            };
        }

        UpgradeFormat[] UltimateUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Better Buffs",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var buffer = GetComponent<Buffer>();
                        buffer.crit = .2f;
                        buffer.speed = 1.44f;

                        level++;
                    }
                },
                new UpgradeFormat()
                {
                    name = "Proc speed bost",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<HyperSpeed>().procChance = .15f;
                        GetComponent<Turret>().FindEffects();
                        level++;
                    },
                }
            };
        }
    }
}