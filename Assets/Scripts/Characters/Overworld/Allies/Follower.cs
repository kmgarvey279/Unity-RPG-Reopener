using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{   

    private Vector3 moveDirection;
    private float walkSpeed = 3f;
    private float runSpeed = 5f;

    public Animator animator;
    public Rigidbody2D rigidbody;
    public bool isRunning;
    public bool isJumping = false;

    [SerializeField] private Transform pointToFollow;

    [SerializeField] private float followDistance;


    public void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        moveDirection = new Vector3(0, 0);
        transform.position = pointToFollow.position;
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

            // float moveX = Input.GetAxisRaw("Horizontal");
            // float moveY = Input.GetAxisRaw("Vertical");
            // moveDirection = new Vector3(moveX, moveY).normalized;

            // if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
            // {
            //     float tempX = Mathf.Round(moveDirection.x);
            //     float tempY = Mathf.Round(moveDirection.y);
            //     animator.SetFloat("Look X", tempX);
            //     animator.SetFloat("Look Y", tempY);

            //     float angle = Mathf.Atan2(moveDirection.x, -moveDirection.y) * Mathf.Rad2Deg;
            //     followerAxis.rotation = Quaternion.Euler(0, 0, angle);
            // }
        }
    }

    public void FixedUpdate()
    {
        
        float currentSpeed = 0;

        if(isRunning)
        {
            currentSpeed = runSpeed;
        }
        else 
        {
            currentSpeed = walkSpeed;
        }
        //move
        Vector3 moveTemp = new Vector3(0,0);
        if(Vector3.Distance(transform.position, pointToFollow.position) > followDistance)
        {
            moveTemp = Vector3.MoveTowards(transform.position, pointToFollow.position, currentSpeed * Time.deltaTime);
            rigidbody.MovePosition(moveTemp);
        }
        animator.SetFloat("Speed", moveTemp.sqrMagnitude);
        //change direction
        Vector2 direction = (pointToFollow.position - transform.position).normalized;
        if(!Mathf.Approximately(direction.x, 0.0f) || !Mathf.Approximately(direction.y, 0.0f))
        {
            float tempX = Mathf.Round(direction.x);
            float tempY = Mathf.Round(direction.y);
            animator.SetFloat("Look X", tempX);
            animator.SetFloat("Look Y", tempY);
        }
    }
}
