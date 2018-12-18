using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class Sniper : Turret
    {
        protected override bool InDistance(Enemy target)
        {
            return true;
        }
    }
}
