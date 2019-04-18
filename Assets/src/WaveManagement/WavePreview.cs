using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaveManagement
{
    public class WavePreview : MonoBehaviour
    {
        public Image[] images;

        public void Start()
        {
            var manager = FindObjectOfType<WaveManager>();
            manager.OnStartDowntime.AddListener(() => ShowWave(manager.NextWave));
            ShowWave(manager.NextWave);
            manager.OnStartWave.AddListener(Hide);
        }
        

        public void ShowWave(Army.Wave wave)
        {
            int hpMult = 100;
            if (DifficultySelector.Difficulty == Difficulty.easy)
                hpMult = 70;
            if (DifficultySelector.Difficulty == Difficulty.medium)
                hpMult = 80;

            Hide();
            for (int i = 0; i < wave.units.Count; i++)
            {
                
                var enemy = wave.units[i].enemy.GetComponent<Attack.Enemy>();
                if(enemy == false)
                {
                    var rando = wave.units[i].enemy.GetComponent<SpawnRandom>();
                    if (rando)
                        enemy = rando.prefabs[0].GetComponent<Attack.Enemy>();
                }
                images[i].sprite = enemy.GetComponentInChildren<SpriteRenderer>(true).sprite;
                images[i].gameObject.SetActive(true);
                var text = images[i].GetComponentInChildren<Text>(true);
                if (text)
                {
                    var tooltip = $"<b>{wave.units[i].qty}x lvl {wave.units[i].level} {enemy.name}</b> ({Attack.Enemy.HPFor(enemy.hits, wave.units[i].level)} hp)";
                    if (enemy.GetComponent<Attack.Enemy>().armor)
                        tooltip += "\nArmor: \u00BD Physical Damage.";
                    var regen = enemy.GetComponent<Attack.Regeration>();
                    if (regen)
                        tooltip += $"\nRegeneration: {regen.hpPerSecond} hp/seconds.";
                    text.text = tooltip;
                }
            }
        }

        public void Hide()
        {
            foreach (var image in images)
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}
