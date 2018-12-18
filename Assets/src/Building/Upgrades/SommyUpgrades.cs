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

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 0)
                    return Level1();
                if (level == 2)
                    return new UpgradeFormat[0];
                if (speedRank < 5 && rangeRank < 5)
                    return new UpgradeFormat[] { SpeedUpgrade(), RangeUpgrade() };
                if (speedRank < 5)
                    return new UpgradeFormat[] { SpeedUpgrade() };
                if (rangeRank < 5)
                    return new UpgradeFormat[] { RangeUpgrade() };
                return UltimateUpgrade();
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
                        level++;
                    },
                }
            };
        }

        int speedRank;
        int rangeRank;

        UpgradeFormat SpeedUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Attack Speed",
                cost = 10 * (speedRank + 1),
                Upgrade = () =>
                {
                    speedRank++;
                    GetComponent<Turret>().attackSpeed = 1.5f + speedRank * .1f;
                    GetComponent<Buffer>().speed = 1.15f + rangeRank * .05f;
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
                    GetComponent<Buffer>().range = 1.5f + rangeRank * .2f;
                    GetComponent<Turret>().distance = 2.5f + rangeRank * .2f;
                },
            };
        }

        UpgradeFormat[] UltimateUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Also Buff Crit",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<Buffer>().crit = .2f;
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

        UpgradeFormat[] Level2()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Base buff at 40%",
                    cost = 140,
                    Upgrade = () =>
                    {
                        GetComponent<Buffer>().speed = 1.4f;
                        GetComponent<Turret>().attackSpeed *= 1.2f;
                        level++;
                    }
                },
                new UpgradeFormat()
                {
                    name = "Bigger Area",
                    cost = 210,
                    Upgrade = () =>
                    {
                        GetComponent<Buffer>().range = 2.5f;
                        GetComponent<Turret>().attackSpeed *= 1.2f;
                        GetComponent<Turret>().distance += 1f;
                        level++;
                    },
                }
            };
        }

        UpgradeFormat[] Level3()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Grant 20% crit",
                    cost = 1000,
                    Upgrade = () =>
                    {
                        GetComponent<Buffer>().crit = .2f;
                        GetComponent<Turret>().attackSpeed *= 1.2f;
                        level++;
                    },
                }
            };
        }

        UpgradeFormat[] Level4()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Chance on hit to activate boost",
                    cost = 2500,
                    Upgrade = () =>
                    {
                        GetComponent<HyperSpeed>().procChance = .15f;
                        GetComponent<Turret>().attackSpeed *= 1.2f;
                        GetComponent<Turret>().FindEffects();
                        level++;
                    }
                }
            };
        }
    }
}