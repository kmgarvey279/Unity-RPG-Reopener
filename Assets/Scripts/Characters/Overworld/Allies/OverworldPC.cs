using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPC : MonoBehaviour
{
    public Vector3 moveDirection;
    public float walkSpeed = 3f;
    public float runSpeed = 5f;

    public Transform followerPosition;
    public Transform followerAxis;

    public Animator animator;
    public bool isRunning;
    public bool isJumping = false;

    public void Start()
    {
        animator = GetComponent<Animator>();
        moveDirection = new Vector3(0, 0);
    }

    public void Update()
    {
        if(!isJumping)
        {
            if(Input.GetButton("Run"))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(moveX, moveY).normalized;

            if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
            {
                float tempX = Mathf.Round(moveDirection.x);
                float tempY = Mathf.Round(moveDirection.y);
                animator.SetFloat("Look X", tempX);
                animator.SetFloat("Look Y", tempY);

                float angle = Mathf.Atan2(moveDirection.x, -moveDirection.y) * Mathf.Rad2Deg;
                followerAxis.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
