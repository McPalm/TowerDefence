using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class ConcussionPool : ObjectPool
    {
        static ConcussionPool _instance;

        void Start() => _instance = this;

        static public void Spawn(GameObject target, float duration) => _instance.ISpawn(target, duration);

        void ISpawn(GameObject target, float duration) => StartCoroutine(SpawnRoutine(target, duration));

        IEnumerator SpawnRoutine(GameObject target, float duration)
        {
            var o = Create();
            o.transform.position = target.transform.position;
            var killTime = Time.timeSinceLevelLoad + duration;
            while(Time.timeSinceLevelLoad < killTime && target)
            {
                yield return null;
            }
            Dispose(o);
        }
    }
}
