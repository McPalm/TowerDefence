using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour {

    private void OnDrawGizmosSelected()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x * 2f) / 2f, Mathf.Round(transform.position.y * 2f) / 2f, transform.position.z);
        transform.localScale = new Vector3(Mathf.Round(transform.localScale.x), Mathf.Round(transform.localScale.y), transform.localScale.z);
    }
}
