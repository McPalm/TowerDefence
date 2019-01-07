using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRadial : MonoBehaviour
{
    public GameObject RadialRoot;
    public float padding;


    public void OpenRadialAt(Vector2 worldPosition)
    {
        RadialRoot.SetActive(true);
        RadialRoot.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        if (RadialRoot.transform.position.x < padding)
            RadialRoot.transform.position = new Vector3(padding, RadialRoot.transform.position.y);
        if (RadialRoot.transform.position.x > Screen.width - padding)
            RadialRoot.transform.position = new Vector3(Screen.width - padding, RadialRoot.transform.position.y);
        if (RadialRoot.transform.position.y < padding)
            RadialRoot.transform.position = new Vector3(RadialRoot.transform.position.x, padding);
        if (RadialRoot.transform.position.y > Screen.height - padding)
            RadialRoot.transform.position = new Vector3(RadialRoot.transform.position.x, Screen.height - padding);
    }

    protected void CloseRadial()=>  StartCoroutine(CloseRoutine());

    IEnumerator CloseRoutine()
    {
        yield return null;
        RadialRoot.SetActive(false);
    }
}
