using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class HealthBar : MonoBehaviour
    {
        public Enemy client;

        Color healthyColor;
        Color poisonColor;
        SpriteRenderer spriteRenderer;


        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            healthyColor = spriteRenderer.color;
            poisonColor = new Color(.3f, .9f, .3f);
        }

        // Use this for initialization
        void Update()
        {
            if(client.HP > 0)
                transform.localScale = new Vector3(client.HealthPercentage, 1f, 1f);
            spriteRenderer.color = client.Poisoned ? poisonColor : healthyColor;
        }
        
    }
}