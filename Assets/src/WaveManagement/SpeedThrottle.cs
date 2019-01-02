using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveManagement
{
    public class SpeedThrottle : MonoBehaviour
    {

        public float speedMultiplier = 2f;
        public bool SpeedUp
        {
            set
            {
                Time.timeScale = value ? speedMultiplier : 1f;
            }
            get
            {
                return Time.timeScale > 1f;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Turbo"))
                SpeedUp = true;
            else if (Input.GetButtonUp("Turbo"))
                SpeedUp = false;

        }
    }
}
