using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Building.Upgrades
{
    abstract public class AUpgrade : MonoBehaviour
    {
        abstract internal UpgradeFormat[] AvailableUpgrades { get; }
        public int SunkCost { set; get; }

        public string Description
        {
            get
            {
                return summary + "\n" + Attributes;
            }
        }

        protected string summary = "";
        
        static protected int RankRank(int rank)
        {
            /* I should not name methods 3 AM */
            if (rank < 3)
                return rank;
            return rank * 2 - 2;
        }

        public virtual string Attributes
        {
            get
            {
                float speed = 1f;
                float damage = 100;
                float crit = 0f;

                var turret = GetComponent<Attack.Turret>();
                if(turret)
                    speed = turret.attackSpeed;
                var dd = GetComponent<Attack.DirectDamage>();
                if (dd)
                {
                    damage = dd.damage;
                    crit = dd.critChance;
                }
                var sd = GetComponent<Attack.SplashDamage>();
                if (sd)
                    damage = sd.DirectDamage;
                var mine = GetComponent<Attack.MineLayer>();
                if(mine)
                {
                    damage = mine.damage;
                }

                if (crit == 0f)
                    return $"Spd: {speed}\n" +
                    $"Dmg: {damage}";
                else
                    return $"Spd: {speed}\n" +
                    $"Dmg: {damage}\n" +
                    $"Crit: {(int)(crit * 100f)} x3";
            }
        }

        internal void DoUpgrade(int i)
        {
            var u = AvailableUpgrades[i];
            if (Pay(u.cost))
            {
                u.Upgrade();
            }
        }

        protected bool Pay(int price)
        {
            if (Score.Wallet.Instance.Money >= price)
            {
                Score.Wallet.Instance.Spend(price);
                SunkCost += price;
                return true;
            }
            return false;
        }

        public void Sell()
        {
            Score.Wallet.Instance.Refund(SunkCost);
            Destroy(gameObject);
        }

        internal struct UpgradeFormat
        {
            internal string name;
            internal int cost;
            internal System.Action Upgrade;
            internal bool maxRank;
        }
    }
}
