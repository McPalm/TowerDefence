using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Movement
{
    public class FireMovementAnimator : MonoBehaviour
    {
        float speed;
        Animator animator;
        Mobile mobile;
        DropFromTheSky drop;
        float lastX;

        bool FaceRight
        {
            set
            {
                if (value != right)
                {
                    Debug.Log("Flip");
                    right = value;
                    if(right)
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
                    else
                        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
                }
            }
        }
        bool right = true;

        // Use this for initialization
        void Start()
        {
            speed = 0f;
            animator = GetComponent<Animator>();
            mobile = GetComponent<Mobile>();
            animator.SetFloat("Speed", speed);
        }

        // Update is called once per frame
        void Update()
        {
            if (mobile.enabled)
            {
                if (speed != mobile.RealSpeed / Mathf.Abs(transform.localScale.x))
                {
                    speed = mobile.RealSpeed / Mathf.Abs(transform.localScale.x);
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
}