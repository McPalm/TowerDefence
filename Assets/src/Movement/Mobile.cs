using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Mobile : MonoBehaviour
    {

        [HideInInspector]
        public WaypointMesh mesh;
        [Range(0.2f, 6f)]
        public float speed = 1f;
        public float location = 0f;

        float slowFactor = 1f;
        float slowDuration = 0f;

        float stun = 0f;
        float stunResistance = 0f;
        public bool Stunned { get { return stun > 0f; } }

        internal void Stun(float duration)
        {
            if (stunResistance <= 0f)
            {
                Debug.Log("Full Stun");
                stun = duration;
                stunResistance = 24f;
            }
            else if(stunResistance < 24f)
            {
                stun = duration/2;
                stunResistance += 12f;
                Debug.Log("Diminished Stun");
            }
            else
                Debug.Log("Stun Immune");
        }

        public bool Slowed
        {
            get
            {
                return slowDuration > 0f;
            }
        }

        // Use this for initialization
        void Start()
        {
            if(!mesh)
                mesh = FindObjectOfType<MeshManager>().GetMesh();
        }

        internal void ApplySlow(float speedFactor, float duration)
        {
            if(speedFactor <= slowFactor)
            {
                slowFactor = speedFactor;
                slowDuration = duration;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (slowDuration > 0f)
                slowDuration -= Time.fixedDeltaTime;
            else
                slowFactor = 1f;
            if (Stunned == false)
            {
                location += slowFactor * speed / 60f;
                transform.position = mesh.DistanceToPosition(location);
            }
            else
                stun -= Time.fixedDeltaTime;
            stunResistance -= Time.fixedDeltaTime;
            if(location > mesh.length)
            {
                Destroy(gameObject);
                FindObjectOfType<Score.Lives>().HP -= 1;
            }

        }

        static public int ComparePosition(Mobile a, Mobile b)
        {
            return a.location > b.location ? -1 : 1;
        }

        static public int CompareSlow(Mobile a, Mobile b)
        {
            if (a.slowDuration == 0 && b.slowDuration == 0)
                return ComparePosition(a, b);
            if (a.slowDuration > 0 && b.slowDuration == 0)
                return 1;
            if (a.slowDuration > 0 && b.slowDuration > 0)
                return ComparePosition(a, b);
            return -1;
        }
    }
}