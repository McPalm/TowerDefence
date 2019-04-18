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
            int level = GetComponent<Attack.Enemy>().Level;

            float worth = this.worth;
            if (nickle)
                worth += .5f;
            worth *= Mathf.Pow(1.15f, level-1) * 2f * Random.value;
            worth += Random.value;
            if (worth < 1f)
            {
                this.worth = 0;
                nickle = true;
            }
            else
            {
                this.worth = Mathf.RoundToInt(worth);
                nickle = false;
            }

            if (nickle)
            {
                odd = !odd;
                if(!odd)
                    FindObjectOfType<Wallet>().Add(1);
            }
            else
                FindObjectOfType<Wallet>().Add(this.worth);
        }
    }

    
}