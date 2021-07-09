using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody2D rigidbody;
    private Vector3 moveDirection;
    private bool isRunning;
    private float walkSpeed = 3f;
    private float runSpeed = 5f;
    [Header("Sprite/Animation")]
    [SerializeField] private SpriteRenderer actionPrompt;
    private Animator animator;
    public SpriteRenderer sprite;

    private bool disableControl = false;
    
    private class JumpData
    {
        public JumpType jumpType;
        public Vector3 start; 
        public Vector3 end; 
        public float duration = 0.15f;
        public float timer = 0f;

        public JumpData(JumpType jumpType, Vector3 start, Vector3 end)
        {
            this.start = start; 
            this.end = end;
            this.jumpType = jumpType;
        }
    }
    private JumpData jumpData;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        moveDirection = new Vector3(0, 0);
    }

    private void Update()
    {
        if(jumpData != null)
        {
            if(Vector3.Distance(transform.position, jumpData.end) < 0.001f)
            {
                EndJump();
            }
            else
            {

                jumpData.timer = Time.deltaTime/jumpData.duration;
                transform.position = Vector3.Lerp(transform.position, jumpData.end, jumpData.timer);
            }
        }
        else if(!disableControl)
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

                // float angle = Mathf.Atan2(moveDirection.x, -moveDirection.y) * Mathf.Rad2Deg;
                // followerAxis.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    public Vector3 LookDirection()
    {
        return new Vector3(animator.GetFloat("Look X"), animator.GetFloat("Look Y"));
    }

    public void FixedUpdate()
    {      
        if(!disableControl)
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
            rigidbody.velocity = new Vector3(moveDirection.x * currentSpeed, moveDirection.y * currentSpeed);

            //update animation
            animator.SetFloat("Speed", rigidbody.velocity.sqrMagnitude);
        }
    }

    public void Jump(JumpType jumpType, Vector3 jumpDestination)
    {
        if(jumpData == null)
        {
            disableControl = true;
            if(actionPrompt.enabled == true)
                actionPrompt.enabled = false;
            
            jumpData = new JumpData(jumpType, transform.position, jumpDestination);
            rigidbody.velocity = Vector3.zero;

            switch ((int)jumpType)
            {
                case 0:        
                    gameObject.layer = gameObject.layer - 1;
                    animator.SetBool("Jump", true);
                    break;
                case 1:
                    sprite.sortingOrder = sprite.sortingOrder + 1;
                    gameObject.layer = gameObject.layer + 1;
                    animator.SetBool("Jump", true);
                    break;
                case 2:
                    animator.SetBool("Jump", true);
                    break;
                default:
                    break;
            }
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void EndJump()
    {
        switch ((int)jumpData.jumpType)
        {
            case 0:    
                sprite.sortingOrder = sprite.sortingOrder - 1;    
                animator.SetBool("Jump", false);
                break;
            case 1:
                animator.SetBool("Jump", false);
                break;
            case 2:
                animator.SetBool("Jump", false);
                break;
            default:
                break;
        }
        jumpData = null;
        GetComponent<BoxCollider2D>().enabled = true;
        disableControl = false;
    }

    public void ToggleControl(bool isDisabled)
    {
        disableControl = isDisabled;

        if(isDisabled && actionPrompt.enabled == true)
            actionPrompt.enabled = false;
    }

    public void DisplayActionPrompt()
    {
        if(actionPrompt.enabled == false)
            actionPrompt.enabled = true;
    }

    public void HideActionPrompt()
    {
        if(actionPrompt.enabled == true)
            actionPrompt.enabled = false;
    }
}