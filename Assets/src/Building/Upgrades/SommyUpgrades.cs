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
                if (level == 1)
                    return new UpgradeFormat[0];
                if (boostRank + speedRank + critRank == 15)
                    return UltimateUpgrade();

                var list = new List<UpgradeFormat>();
                list.Add(RangeUpgrade());
                list.Add(SpeedUpgrade());
                list.Add(CritUpgrade());
                return list.ToArray();
            }
        }

        void Start() => SunkCost = 40;

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
        int boostRank;
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
                maxRank = speedRank == 5,
            };
        }

        UpgradeFormat RangeUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Active Boost",
                cost = 10 * (boostRank + 1),
                Upgrade = () =>
                {
                    boostRank++;
                    // GetComponent<Turret>().distance = 2.5f + RankRank(boostRank) * .25f;
                    HyperSpeed boost;
                    if (boostRank == 1)
                        boost = gameObject.AddComponent<HyperSpeed>();
                    else
                        boost = GetComponent<HyperSpeed>();
                    boost.power = 1.6f + RankRank(boostRank) * .2f;
                    boost.audioClip = BoostSound;
                },
                maxRank = boostRank == 5,
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
                },
                maxRank = critRank == 5,
            };
        }

        UpgradeFormat[] UltimateUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "+1 Buff Radius",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var buffer = GetComponent<Buffer>();
                        buffer.range = 2.5f;

                        level++;
                        MarkFinal();
                    }
                },
                new UpgradeFormat()
                {
                    name = "Proc speed boost",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<HyperSpeed>().procChance = .15f;
                        GetComponent<Turret>().FindEffects();
                        level++;
                        MarkFinal();
                    },
                }
            };
        }
    }
}