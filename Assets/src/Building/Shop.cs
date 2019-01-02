using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Score;

namespace Building
{
    public class Shop : MonoBehaviour
    {
        public GameObject twilight;
        public GameObject sniper;
        public GameObject splash;
        public GameObject mine;
        public GameObject brute;
        public GameObject sommy;

        public void Update()
        {
            if (Input.GetButtonDown("Tower1"))
                BuySniper();
            if (Input.GetButtonDown("Tower2"))
                BuyTwiggy();
            if (Input.GetButtonDown("Tower3"))
                BuySplash();
            if (Input.GetButtonDown("Tower4"))
                BuyMine();
            if (Input.GetButtonDown("Tower5"))
                BuyBrute();
            if (Input.GetButtonDown("Tower6"))
                BuySommy();
        }

        public void BuyTwiggy()
        {
            if(Wallet.Instance.Money >= 40)
            {
                FindObjectOfType<TowerPlacer>().Select(twilight, 40);
            }
        }

        public void BuySniper()
        {
            if (Wallet.Instance.Money >= 40)
            {
                FindObjectOfType<TowerPlacer>().Select(sniper, 40);
            }
        }

        public void BuySplash()
        {
            if (Wallet.Instance.Money >= 40)
            {
                FindObjectOfType<TowerPlacer>().Select(splash, 40);
            }
        }

        public void BuyMine()
        {
            if (Wallet.Instance.Money >= 40)
            {
                FindObjectOfType<TowerPlacer>().Select(mine, 40);
            }
        }

        public void BuyBrute()
        {
            if (Wallet.Instance.Money >= 40)
            {
                FindObjectOfType<TowerPlacer>().Select(brute, 40);
            }
        }

        public void BuySommy()
        {
            if (Wallet.Instance.Money >= 40)
            {
                FindObjectOfType<TowerPlacer>().Select(sommy, 40);
            }
        }
        
    }
}
