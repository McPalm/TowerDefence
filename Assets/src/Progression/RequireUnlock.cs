using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progression
{
    public class RequireUnlock : MonoBehaviour
    {
        public Unlock Unlock;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(SaveData.Current.LevelOf(Unlock) > 0);
        }
    }
}
