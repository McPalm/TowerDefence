using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class HealthSprites : MonoBehaviour, Enemy.IOnStruck
    {
        
        public GameObject[] sprites;

        public void OnHurt(int damage, float healthPercentage)
        {
            float relative = healthPercentage * sprites.Length;

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].SetActive(i < relative);
            }
        }
    }
}
