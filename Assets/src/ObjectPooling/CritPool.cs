using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class CritPool : ObjectPool
    {
        static CritPool _instance;

        // Use this for initialization
        void Start()
        {
            _instance = this;
        }

        static public void Spawn(Vector3 position) => _instance.ISpawn(position);

        void ISpawn(Vector3 position) => StartCoroutine(SpawnRoutine(position));

        IEnumerator SpawnRoutine(Vector3 position)
        {
            var fab = Create();
            fab.transform.position = position;
            yield return new WaitForSeconds(.15f);
            yield return new WaitForSecondsRealtime(.15f);
            Dispose(fab);
        }
    }
}
