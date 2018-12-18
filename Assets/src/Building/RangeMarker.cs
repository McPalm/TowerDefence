using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMarker : MonoBehaviour
{
    public void Show(Vector3 coordinates, float radius)
    {
        gameObject.SetActive(true);
        transform.position = coordinates;
        transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
