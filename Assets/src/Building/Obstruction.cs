using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building {
    public class Obstruction : MonoBehaviour {

        // Use this for initialization
        private void Start()
        {
            FindObjectOfType<BuildGrid>().Add(gameObject);
        }

        private void OnDestroy()
        {
            FindObjectOfType<BuildGrid>()?.Remove(gameObject);
        }
    }
}
