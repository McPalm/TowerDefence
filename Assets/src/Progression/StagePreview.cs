using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Progression
{
    public class StagePreview : MonoBehaviour
    {
        public GameObject Background;
        public GameObject Images;
        public TextMeshProUGUI Text;
        public Image Minimap;
        public GameObject MapMarker;

        public System.Action OnConfirm;

        public void Show(Tile tile, System.Action OnConfirm)
        {
            var enemies = new List<GameObject>();
            foreach (var wave in tile.Army.waves)
            {
                foreach (var unit in wave.units)
                {
                    if (enemies.Contains(unit.enemy) == false)
                        enemies.Add(unit.enemy);
                }
            }
            var images = Images.GetComponentsInChildren<Image>(true);
            for (int i = 0; i < images.Length; i++)
            {
                if (i < enemies.Count)
                {
                    var sprite = enemies[i].GetComponent<SpriteRenderer>();
                    if (sprite)
                    {
                        images[i].gameObject.SetActive(true);
                        images[i].sprite = sprite.sprite;
                    }
                    else
                    {
                        images[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    images[i].gameObject.SetActive(false);
                }
            }
            var info = Map.Instance.ScenedataFor(tile.Stage);
            Minimap.sprite = info.UISprite;
            Text.text = $"Level {tile.Level}";
            Background.SetActive(true);
            this.OnConfirm = OnConfirm;
            MapMarker.transform.position = tile.transform.position;
            MapMarker.SetActive(true);
        }

        public void Hide()
        {
            Background.SetActive(false);
            MapMarker.SetActive(false);
        }

        public void Confirm() => OnConfirm?.Invoke();
    }
}
