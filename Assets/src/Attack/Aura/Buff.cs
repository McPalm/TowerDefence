using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack.Aura
{
    /// <summary>
    /// More like buffee
    /// </summary>
    public class Buff : MonoBehaviour, IBuff
    {
        float duration;
        float critDuration;
        public float Speed { get; private set; }
        public float Crit { get; private set; }
        public GameObject[] icons;

        static HashSet<Buff> _allBuffs;
        static internal HashSet<Buff> AllBuffs
        {
            get
            {
                if (_allBuffs == null)
                    _allBuffs = new HashSet<Buff>();
                return _allBuffs;
            }
        }

        void Awake()
        {
            AllBuffs.Add(this);
            Speed = 1f;
        }

        void OnDestroy()
        {
            AllBuffs.Remove(this);
        }

        void Update()
        {
            if (duration > 0f)
                duration -= Time.deltaTime;
            else
            {
                Speed = 1f;
                foreach (var icon in icons)
                {
                    icon.SetActive(false);
                }
            }
            if (critDuration > 0f)
                critDuration -= Time.deltaTime;
            else
                Crit = 0f;
        }

        internal void Apply(float duration, float speed)
        {
            if (speed >= Speed)
            {
                Speed = speed;

                this.duration = duration;
                if (speed > 1.1f)
                    icons[0].SetActive(true);
                if (speed > 1.35f)
                    icons[1].SetActive(true);
                if (speed > 2f)
                    icons[2].SetActive(true);
            }
        }

        internal void ApplyCrit(float duration, float crit)
        {
            if(crit >= Crit)
            {
                Crit = crit;
                critDuration = duration;
            }
        }
    }
}
