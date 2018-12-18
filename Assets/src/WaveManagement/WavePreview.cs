using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaveManagement
{
    public class WavePreview : MonoBehaviour
    {
        public Text text;

        public void Start()
        {
            var manager = FindObjectOfType<WaveManager>();
            manager.OnStartDowntime.AddListener(() => ShowWave(manager.NextWave));
            ShowWave(manager.NextWave);
            manager.OnStartWave.AddListener(Hide);
        }
        

        public void ShowWave(Army.Wave wave)
        {
            text.gameObject.SetActive(true);
            var format = "Next";
            foreach (var item in wave.units)
            {
                format += "\n" + item.qty + "x " +  item.enemy.name;
            }
            text.text = format;
        }

        public void Hide()
        {
            text.gameObject.SetActive(false);
        }
    }
}
