using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class HealthBar : MonoBehaviour
    {

        public Enemy client;

        // Use this for initialization
        void Update()
        {
            transform.localScale = new Vector3(client.HealthPercentage, 1f, 1f);
        }
        
    }
}