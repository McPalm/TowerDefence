using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;

namespace Building.Upgrades
{
    public class MineUpgrades : AUpgrade
    {
        int level = 0;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 1)
                    return new UpgradeFormat[0];
                if (damageRank < 5 && rateRank < 5)
                    return new UpgradeFormat[] { DamageUpgrade(), RateUpgrade() };
                if (damageRank < 5)
                    return new UpgradeFormat[] { DamageUpgrade() };
                if (rateRank < 5)
                    return new UpgradeFormat[] { RateUpgrade() };
                return FinalUpgrade();
                    
            }
        }

        void Start() => SunkCost = 40;

        public override string Attributes
        {
            get
            {
                var mines = GetComponent<MineLayer>();
                return $"Damage: {mines.damage}\nMines: {mines.minesPerWave}";
            }
        }


        int damageRank = 0;
        UpgradeFormat DamageUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Damage",
                cost = 10 * (damageRank + 1),
                Upgrade = () =>
                {
                    damageRank++;
                    GetComponent<MineLayer>().damage = 300  + damageRank * 60;
                },
            };
        }

        int rateRank = 0;
        UpgradeFormat RateUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "More mines",
                cost = 10 * (rateRank + 1),
                Upgrade = () =>
                {
                    rateRank++;
                    GetComponent<MineLayer>().minesPerWave = 6 + rateRank * 2;
                    GetComponent<MineLayer>().range = 2.5f + rateRank * .2f;
                    GetComponent<MineLayer>().RefreshSpawnPoints();
                },
            };
        }

        UpgradeFormat[] FinalUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat
                {
                name = "Snaring",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var layer = GetComponent<MineLayer>();
                        layer.slowDuration = 6f;
                        layer.slowFactor = .0f;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                name = "x3 Mines",
                cost = 500,
                Upgrade = () =>
                    {
                        GetComponent<MineLayer>().minesPerWave *= 3;
                        level++;
                    },
                }
            };
        }
    }
}
