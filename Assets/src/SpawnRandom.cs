using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : MonoBehaviour {

    public GameObject[] prefabs;
    public int qty = 1;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < qty; i++)
        {
            var o = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
            o.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}
