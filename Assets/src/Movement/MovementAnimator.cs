using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementAnimator : MonoBehaviour {

        float speed;
        Animator animator;
        Mobile mobile;
        float lastX;

        bool FaceRight
        {
            set
            {
                if(value != right)
                {
                    right = value;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1f);
                }
            }
        }
        bool right = true;

        // Use this for initialization
        void Start()
        {
            speed = 1f;
            animator = GetComponent<Animator>();
            mobile = GetComponent<Mobile>();
            animator.SetFloat("Speed", speed);
        }

        // Update is called once per frame
        void Update()
        {
            if(speed != mobile.RealSpeed)
            {
                speed = mobile.RealSpeed;
                animator.SetFloat("Speed", speed);
            }
            var x = transform.position.x;
            if (lastX < x)
                FaceRight = true;
            else if (lastX > x)
                FaceRight = false;
            lastX = x;
        }
    }
}