using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Mobile : MonoBehaviour
    {
        public WaypointMesh mesh { set; get; }
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
                stun = duration;
                stunResistance = duration * 3f;
                ObjectPooling.ConcussionPool.Spawn(transform.position, duration);
            }
            else if(stunResistance < duration * 6f * UnityEngine.Random.value)
            {
                stun = duration;
                stunResistance += duration * 3f;
                ObjectPooling.ConcussionPool.Spawn(transform.position, duration);
            }
        }

        public bool Slowed
        {
            get
            {
                return slowDuration > 0f;
            }
        }

        public float RealSpeed
        {
            get
            {
                if (Stunned)
                    return 0f;
                if (Slowed)
                    return speed * slowFactor;
                return speed;
            }
        }

        // Use this for initialization
        void Start()
        {
            if (!mesh)
                mesh = FindObjectOfType<MeshManager>().GetMesh();
            if (DifficultySelector.Difficulty == Difficulty.easy)
                speed *= .7f;
            if (DifficultySelector.Difficulty == Difficulty.medium)
                speed *= .85f;
        }

        internal void ApplySlow(float speedFactor, float duration)
        {
            if(speedFactor <= slowFactor)
            {
                var sprite = GetComponent<SpriteRenderer>();
                if (sprite)
                    sprite.color = new Color(0.2f, .2f, 1f);
                slowFactor = speedFactor;
                slowDuration = duration;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (slowDuration > 0f)
            {
                slowDuration -= Time.fixedDeltaTime;
                if (slowDuration <= 0f)
                {
                    var sprite = GetComponent<SpriteRenderer>();
                    if (sprite)
                        sprite.color = Color.white;
                }
            }
            else
            {
                slowFactor = 1f;
            }
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