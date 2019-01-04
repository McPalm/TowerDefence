using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class VolumeControl : MonoBehaviour, IPointerClickHandler
    {

        static public float volume = 1f;
        public Slider slider;
        public GameObject ShowWhenMuted;

        float storedVolume = .5f;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(volume > 0f)
            {
                storedVolume = volume;
                volume = 0f;
            }
            else
            {
                volume = storedVolume;
            }
            slider.value = volume;
            ShowWhenMuted.SetActive(volume == 0f);
        }

        private void Start()
        {
            slider.maxValue = 1f;
            slider.minValue = 0f;
            slider.value = volume;
            slider.onValueChanged.AddListener(f =>
            {
                volume = f;
                ShowWhenMuted.SetActive(volume == 0f);
            });
            ShowWhenMuted.SetActive(volume == 0f);
        }

    }
}
