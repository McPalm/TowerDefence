using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class LivingFlame : Enemy, Enemy.IOnStruck
    {

        public float maxSize = 1f;
        public float minSpeed = 1f;
        public float maxSpeed = 3f;

        public void OnHurt(int damage, float healthPercentage)
        {
            SetSize(healthPercentage);
        }

        new protected void Start()
        {
            base.Start();
            SetSize(1f);
        }

        void SetSize(float healthPercentage)
        {
            float size = .4f + healthPercentage * maxSize;
            if(transform.localScale.x > 0f)
                transform.localScale = new Vector3(size, size, 1f);
            else
                transform.localScale = new Vector3(-size, size, 1f);
            GetComponent<Movement.Mobile>().speed = Mathf.Lerp(minSpeed, maxSpeed, healthPercentage);
        }
        
    }
}