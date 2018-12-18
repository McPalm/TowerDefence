using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Score
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField]
        int money;

        public int Money { get { return money; }
            private set
            {
                money = value;
                OnWalletChange.Invoke(money);
            }
        }
        public int TotalWorth { get; private set; }
        public void Add(int money)
        {
            Money += money;
            TotalWorth += money;
        }
        public void Spend(int money) => Money -= money;
        public void Refund(int money)
        {
            money = money * 80 / 100;
            Money += money;
        }

        public MoneyEvent OnWalletChange;

        static public Wallet Instance;

        void Start()
        {
            Instance = this;
            TotalWorth = money;
        }

        [System.Serializable]
        public class MoneyEvent : UnityEvent<int> { }
    }
}
