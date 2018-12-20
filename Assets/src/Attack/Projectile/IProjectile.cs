using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack.Projectile
{
    public interface IProjectile
    {
        void Shoot(GameObject target, System.Action<GameObject> effect, System.Action<GameObject> effect2);
    }
}
