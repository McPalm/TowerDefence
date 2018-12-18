using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

namespace Attack
{
    public class HealthPhase : MonoBehaviour, Enemy.IOnStruck
    {
        [Range(.1f, 1f)]
        public float threshold = .5f;
        public float speed = 0f;
        public bool armor;
        public Sprite sprite;


        bool active = false;
        

        public void OnHurt(int damage, float healthPercentage)
        {
            if (!active && healthPercentage <= threshold)
            {
                if (speed > 0f)
                    GetComponent<Mobile>().speed = speed;
                if (sprite != null)
                    GetComponent<SpriteRenderer>().sprite = sprite;
                GetComponent<Enemy>().armor = armor;
                active = true;
            }
        }
    }
}
