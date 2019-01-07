using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Building.Upgrades;

public class TouchUpgradeUI : TouchRadial
{
    public Button[] buttons;
    public Text sellText;
    public Text details;
    public RangeMarker rangeMarker;
    AUpgrade target;

    public void ShowAt(AUpgrade target)
    {
        this.target = target;
        OpenRadialAt(target.transform.position);
        RefreshButtons();
    }

    public void Close()
    {
        sellCount = 0;
        target = null;
        details.text = "";
        rangeMarker.Hide();
        CloseRadial();
    }

    int sellCount = 0;
    public void Sell()
    {
        sellCount++;
        sellText.text = "Confirm";
        if (sellCount == 2)
        {
            target.Sell();
            Close();
        }
    }

    void RefreshButtons()
    {
        var upgrades = target.AvailableUpgrades;
        sellText.text = $"Sell\n₿ {target.SunkCost * 80 / 100}";
        details.text = target.Description;
        var turret = target.GetComponent<Attack.Turret>();
        if (turret)
            rangeMarker.Show(target.transform.position, turret.distance);
        else
        {
            var mine = target.GetComponent<Attack.MineLayer>();
            if (mine)
                rangeMarker.Show(target.transform.position, mine.range);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < upgrades.Length)
            {
                var capture = i;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() =>
                {
                    target.DoUpgrade(capture);
                    RefreshButtons();
                });
                buttons[i].gameObject.SetActive(true);
                var max = upgrades[i].maxRank;
                if (max)
                    buttons[i].GetComponentInChildren<Text>().text = $"{upgrades[i].name}\n MAX";
                else
                    buttons[i].GetComponentInChildren<Text>().text = $"{upgrades[i].name}\n₿ {upgrades[i].cost}";

                buttons[i].interactable = !max;
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

}
