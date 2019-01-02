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
        public Button SellButton;

        AUpgrade selected;

        void Start()
        {
            var select = FindObjectOfType<Selection.SelectTower>();
            select.OnSelect.AddListener(Select);
            select.OnDeselect.AddListener(Deselect);
            Deselect();
        }

        void Select(GameObject o)
        {
            var app = o.GetComponent<AUpgrade>();
            if (app)
                Show(app);
            else
                Deselect();
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
            if (Input.GetButtonDown("Upgrade1"))
            {
                if (upgradeButtons[0].IsActive() && upgradeButtons[0].interactable)
                    upgradeButtons[0].onClick.Invoke();
            }
            if (Input.GetButtonDown("Upgrade2"))
            {
                if (upgradeButtons[1].IsActive() && upgradeButtons[1].interactable)
                    upgradeButtons[1].onClick.Invoke();
            }
            if (Input.GetButtonDown("Upgrade3"))
            {
                if (upgradeButtons[2].IsActive() && upgradeButtons[2].interactable)
                    upgradeButtons[2].onClick.Invoke();
            }


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
            SellButton.gameObject.SetActive(false);
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
                    var button = upgradeButtons[i];
                    button.gameObject.SetActive(true);
                    var max = upgrades[i].maxRank;
                    if(max)
                        button.GetComponentInChildren<Text>().text = $"{upgrades[i].name}\n MAX";
                    else
                        button.GetComponentInChildren<Text>().text = $"{upgrades[i].name}\n₿ {upgrades[i].cost}";
                    button.onClick.RemoveAllListeners();
                    button.interactable = !max;
                    var capture = i;
                    button.onClick.AddListener(() => {
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
            SellButton.gameObject.SetActive(true);
            SellButton.GetComponentInChildren<Text>().text = $"Sell ({selected.SunkCost * 80 / 100})";
        }
    }
}
