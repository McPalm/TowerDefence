using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class VolumeControl : MonoBehaviour
    {

        static public float volume = 1f;

        public Slider slider;


        private void Start()
        {
            slider.maxValue = 1f;
            slider.minValue = 0f;
            slider.value = volume;
            slider.onValueChanged.AddListener(f => volume = f);
        }

    }
}
