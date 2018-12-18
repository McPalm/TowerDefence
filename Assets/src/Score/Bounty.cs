using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Score
{
    public class Bounty : MonoBehaviour, Attack.Enemy.IOnKilled
    {
        public int worth = 1;
        public bool nickle = false;

        static bool odd = false;

        static int depth = 0;
        public int TotalWorth
        {
            get
            {
                var spawn = GetComponent<Attack.SpawnOnDeath>();
                if(spawn)
                {
                    var bounty = spawn.GetComponent<Bounty>();
                    if(bounty)
                    {
                        depth++;
                        if (depth < 100)
                            return bounty.TotalWorth * spawn.qty + worth;
                        else
                            Debug.LogError("Bounty.TotalWorth wont go more than 100 spawns deep."); // this is to prevent recursion
                        depth = 0;
                    }
                }
                return worth;
            }
        }

        public void OnKilled()
        {
            if (nickle)
            {
                if (odd)
                    odd = false;
                else
                {
                    FindObjectOfType<Wallet>().Add(1);
                    odd = true;
                }
            }
            else
                FindObjectOfType<Wallet>().Add(worth);
        }
    }

    
}