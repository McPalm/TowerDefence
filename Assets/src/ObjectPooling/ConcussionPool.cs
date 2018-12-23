using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class ConcussionPool : ObjectPool
    {
        static ConcussionPool _instance;

        void Start() => _instance = this;

        static public void Spawn(Vector3 position, float duration) => _instance.ISpawn(position, duration);

        void ISpawn(Vector3 position, float duration) => StartCoroutine(SpawnRoutine(position, duration));

        IEnumerator SpawnRoutine(Vector3 position, float duration)
        {
            var o = Create();
            o.transform.position = position;
            yield return new WaitForSeconds(duration);
            Dispose(o);
        }
    }
}
