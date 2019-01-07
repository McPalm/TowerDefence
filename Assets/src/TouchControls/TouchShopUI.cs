using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Building;

public class TouchShopUI : TouchRadial
{
    public Shop shop;
    Vector3 location;
    public GameObject Dummy;
    
	
	public void OpenAt(Vector3 location)
    {
        location = new Vector3(Mathf.Round(location.x + .5f) - .5f, Mathf.Round(location.y + .5f) - .5f);
        this.location = location;
        OpenRadialAt(location);
        Dummy.gameObject.SetActive(true);
        Dummy.transform.position = location;
    }

    public void Close()
    {
        CloseRadial();
        Dummy.gameObject.SetActive(false);
    }

    public void BuildSniper()
    {
        TowerPlacer.Place(shop.sniper, location, 40);
        Close();
    }

    public void BuildBrute()
    {
        TowerPlacer.Place(shop.brute, location, 40);
        Close();
    }

    public void BuildSplash()
    {
        TowerPlacer.Place(shop.splash, location, 40);
        Close();
    }

    public void BuildBounce()
    {
        TowerPlacer.Place(shop.twilight, location, 40);
        Close();
    }

    public void BuildMine()
    {
        TowerPlacer.Place(shop.mine, location, 40);
        Close();
    }

    public void BuildSupport()
    {
        TowerPlacer.Place(shop.sommy, location, 40);
        Close();
    }

}
