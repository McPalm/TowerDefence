using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Building.Upgrades
{
    public class UpgradeUI : MonoBehaviour
    {
        public RangeMarker rangeMarker;
        public Image displayImage;
        public Text description;

        public Button[] upgradeButtons;

        AUpgrade selected;

        void Start()
        {
            FindObjectOfType<Selection.SelectTower>().OnSelect.AddListener(Select);
        }

        void Select(GameObject o)
        {
            var app = o.GetComponent<AUpgrade>();
            if (app)
                Show(app);
        }

        public void Sell()
        {
            if (selected)
            {
                selected.Sell();
                Deselect();
            }
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                Deselect();
        }

        void Deselect()
        {
            rangeMarker.gameObject.SetActive(false);
            foreach (var item in upgradeButtons)
                item.gameObject.SetActive(false);
            displayImage.gameObject.SetActive(false);
            selected = null;
            description.text = "";
        }

        void Show(AUpgrade target)
        {
            selected = target;
            displayImage.gameObject.SetActive(true);
            displayImage.sprite = target.GetComponent<SpriteRenderer>().sprite;


            ShowRadius();
            ShowUpgrades();
        }

        void ShowRadius()
        {
            var turret = selected.GetComponent<Attack.Turret>();
            var miner = selected.GetComponent<Attack.MineLayer>();
            var radius = .5f;
            if (turret)
                radius = turret.distance;
            else if (miner)
                radius = miner.range;
            rangeMarker.Show(selected.transform.position, radius);
        }

        void ShowUpgrades()
        {
            var upgrades = selected.AvailableUpgrades;
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                if (i < upgrades.Length)
                {
                    upgradeButtons[i].gameObject.SetActive(true);
                    upgradeButtons[i].GetComponentInChildren<Text>().text = $"{upgrades[i].name}\n₿ {upgrades[i].cost}";
                    upgradeButtons[i].onClick.RemoveAllListeners();
                    var capture = i;
                    upgradeButtons[i].onClick.AddListener(() => {
                        selected.DoUpgrade(capture);
                        ShowUpgrades();
                        ShowRadius();
                    });

                }
                else
                {
                    upgradeButtons[i].gameObject.SetActive(false);
                }
            }
            description.text = selected.Description;
        }
    }
}
