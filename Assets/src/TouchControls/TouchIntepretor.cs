using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Building;
using Building.Upgrades;

public class TouchIntepretor : MonoBehaviour
{
    BuildGrid grid;

    public GameObject[] Overlays;

    // Use this for initialization
    void Start () {
        var touch = GetComponent<Touch>();
        touch.OnTapLocation.AddListener(OnTap);
        grid = FindObjectOfType<BuildGrid>();
	}
	
   

    void OnTap(Vector2 worldLocation)
    {
        foreach (var item in Overlays)
        {
            if(item.activeSelf)
            {
                item.SetActive(false);
                return;
            }
        }
        var o = grid.ObjectAt(worldLocation);
        if(o)
        {
            var tower = o.GetComponent<AUpgrade>();
            if (tower)
            {
                GetComponent<TouchUpgradeUI>().ShowAt(tower);
                GetComponent<TouchShopUI>().Close();
            }
            else
            {
                EmptyTap(worldLocation);
            }
        }
        else if(grid.SpaceAvailable(worldLocation))
        {
            GetComponent<TouchShopUI>().OpenAt(worldLocation);
            GetComponent<TouchUpgradeUI>().Close();
        }
        else
        {
            EmptyTap(worldLocation);
        }
    }

    void EmptyTap(Vector2 worldLocation)
    {
        GetComponent<TouchShopUI>().Close();
        GetComponent<TouchUpgradeUI>().Close();
    }
}
