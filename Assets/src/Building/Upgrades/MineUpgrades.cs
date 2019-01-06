using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;

namespace Building.Upgrades
{
    public class MineUpgrades : AUpgrade
    {
        int level = 0;
        public GameObject TheBigOne;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 1)
                    return new UpgradeFormat[0];
                if (damageRank + rateRank + explosionRank == 15)
                    return FinalUpgrade();

                var list = new List<UpgradeFormat>();

                list.Add(DamageUpgrade());
                list.Add(RateUpgrade());
                list.Add(ExplosionUpgrade());

                return list.ToArray();
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
        int rateRank = 0;
        int explosionRank = 0;

        UpgradeFormat DamageUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Damage",
                cost = 10 * (damageRank + 1),
                Upgrade = () =>
                {
                    damageRank++;
                    GetComponent<MineLayer>().damage = 200 + RankRank(damageRank) * 50;
                },
                maxRank = damageRank == 5,
            };
        }

        UpgradeFormat ExplosionUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Area",
                cost = 10 * (explosionRank + 1),
                Upgrade = () =>
                {
                    explosionRank++;
                    var layer = GetComponent<MineLayer>();
                    layer.explosionRadius = 1f + explosionRank * .3f;
                    layer.maxTargets = 2 + RankRank(explosionRank) / 2;
                },
                maxRank = explosionRank == 5,
            };
        }

        UpgradeFormat RateUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "More mines",
                cost = 10 * (rateRank + 1),
                Upgrade = () =>
                {
                    rateRank++;
                    GetComponent<MineLayer>().minesPerWave = 3 + rateRank;
                },
                maxRank = rateRank == 5,
            };
        }

        UpgradeFormat[] FinalUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat
                {
                name = "Concussive Blast",
                    cost = 500,
                    Upgrade = () =>
                    {
                        var layer = GetComponent<MineLayer>();
                        layer.stunDuration = 4f;
                        level++;
                        MarkFinal();
                    },
                },
            };
        }
    }
}
