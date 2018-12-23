using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;

namespace Building.Upgrades
{
    public class SniperUpgrades : AUpgrade
    {
        int level = 0;
        bool aimedTurret = false;
        public GameObject turretParticle;

        public AudioClip TurretAudio;
        public AudioClip TurretHitAudio;

        internal override UpgradeFormat[] AvailableUpgrades
        {
            get
            {
                if (level == 0)
                    return EarlyChoice();
                if (level == 1 && aimedTurret)
                    return TurretUpgrades();
                if (level == 1 && aimedTurret == false)
                    return SniperStuffs();

                return new UpgradeFormat[0]; // empty default;
            }
        }

        void Start() => SunkCost = 40;

        UpgradeFormat[] EarlyChoice()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "Sniper",
                    cost = 20,
                    Upgrade = () =>
                    {
                        GetComponent<Turret>().distance = 4.5f;
                        GetComponent<DirectDamage>().damage = 200;
                        GetComponent<DirectDamage>().armorPiercing = true;
                        level++;
                    },
                },
                new UpgradeFormat()
                {
                    name = "Turret",
                    cost = 50,
                    Upgrade = () =>
                    {
                        Destroy(GetComponent<Turret>());
                        GetComponent<DirectDamage>().damage = 20;
                        GetComponent<DirectDamage>().offTargetDamage = 10;
                        var turret = gameObject.AddComponent<AimedTurret>();
                        turret.speed = 3f;
                        turret.spread = .8f;
                        turret.projectileSpeed = 15f;
                        turret.bullet = turretParticle;

                        turret.SoundEffect = TurretAudio;

                        var audio = GetComponent<OnHitSound>();
                        audio.HitSound[0] = TurretHitAudio;
                        audio.volume *= .15f;
                        audio.SecondaryHitSound = new AudioClip[] {TurretHitAudio};
                        audio.secondaryVolume = audio.volume * .66f;

                        aimedTurret = true;
                        level++;
                    },
                }
            };
        }

        int speedRank = 0;
        UpgradeFormat AimedSpeedUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Fire Rate",
                cost = 20 * (speedRank + 1),
                Upgrade = () =>
                {
                    speedRank++;
                    GetComponent<AimedTurret>().speed = 3f + speedRank;
                },
            };
        }

        int accuracyRank = 0;
        UpgradeFormat AimedAccuracyUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Accuracy & Damage",
                cost = 20 * (accuracyRank + 1),
                Upgrade = () =>
                {
                    accuracyRank++;
                    GetComponent<AimedTurret>().spread = .7f - RankRank(accuracyRank) * .0875f;
                    GetComponent<AimedTurret>().projectileSpeed = 15f + accuracyRank * 2.5f;
                    GetComponent<DirectDamage>().damage = 20 + RankRank(accuracyRank) * 5;
                    GetComponent<DirectDamage>().offTargetDamage = 10 + accuracyRank * 3 + RankRank(damageRank) * 2;
                },
            };
        }

        UpgradeFormat AimedPiercingUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Piercing",
                cost = 20 * (damageRank + 1),
                Upgrade = () =>
                {
                    damageRank++;
                    GetComponent<AimedTurret>().piercing = 1f + damageRank * .5f;
                    GetComponent<DirectDamage>().offTargetDamage = 10 + accuracyRank * 3 + RankRank(damageRank) * 2;
                },
            };
        }

        UpgradeFormat[] TurretUpgrades()
        {
            if (speedRank == 5 && accuracyRank == 5 && damageRank == 5)
            {
                return new UpgradeFormat[0];
            }
            var upgrades = new List<UpgradeFormat>();
            if (speedRank < 5)
                upgrades.Add(AimedSpeedUpgrade());
            if (accuracyRank < 5)
                upgrades.Add(AimedAccuracyUpgrade());
            if (damageRank < 5)
                upgrades.Add(AimedPiercingUpgrade());
            return upgrades.ToArray();
        }

        UpgradeFormat[] SniperStuffs()
        {
            if (rangeRank == 5 && damageRank == 5 && speedRank == 5)
                return UltimateSniperUpgrade();

            List<UpgradeFormat> upgrades = new List<UpgradeFormat>();

            if (damageRank < 5)
                upgrades.Add(DamageUpgrade());
            if (rangeRank < 5)
                upgrades.Add(RangeUpgrade());
            if (speedRank < 5)
                upgrades.Add(SpeedUpgrade());
            return upgrades.ToArray();
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
                    GetComponent<Turret>().attackSpeed = .3f + speedRank * .05f;
                },
            };
        }


        int rangeRank = 0;
        UpgradeFormat RangeUpgrade()
        {
            return new UpgradeFormat()
            {
                name = "Range",
                cost = 10 * (rangeRank + 1),
                Upgrade = () =>
                {
                    rangeRank++;
                    GetComponent<Turret>().distance = 4.5f + rangeRank * .6f + ((rangeRank == 5) ? 1f : 0f);
                },
            };
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
                    GetComponent<DirectDamage>().damage = 200 + RankRank(damageRank) * 50;
                },
            };
        }

        UpgradeFormat[] UltimateSniperUpgrade()
        {
            return new UpgradeFormat[]
            {
                new UpgradeFormat()
                {
                    name = "x2 Attack Speed",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<Turret>().attackSpeed *= 2f;
                        level++;
                    },

                },
                new UpgradeFormat()
                {
                    name = "Devastating Accuracy",
                    cost = 500,
                    Upgrade = () =>
                    {
                        GetComponent<DirectDamage>().damage = 1200;
                        GetComponent<DirectDamage>().critChance = .2f;
                        GetComponent<Turret>().distance = 20f;
                        level++;
                    },
                }
            };
        }
    }
}