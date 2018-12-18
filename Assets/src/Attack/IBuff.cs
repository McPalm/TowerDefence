using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public interface IBuff
    {
        float Speed { get; }
        float Crit { get; }
    }
}
