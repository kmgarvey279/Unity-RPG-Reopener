using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    private Vector3 moveDirection;
    private bool isRunning;
    private float walkSpeed = 3f;
    private float runSpeed = 5f;
    
    [Header("Sprite/Animation")]
    [SerializeField] private SpriteRenderer actionPrompt;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;

    [Header("Interactable")]
    private Interactable interactable;

    [Header("Game state info")]
    [SerializeField] private RuntimeData runtimeData;
    
    // private class JumpData
    // {
    //     public JumpType jumpType;
    //     public Vector3 start; 
    //     public Vector3 end; 
    //     public float duration = 0.15f;
    //     public float timer = 0f;

    //     public JumpData(JumpType jumpType, Vector3 start, Vector3 end)
    //     {
    //         this.start = start; 
    //         this.end = end;
    //         this.jumpType = jumpType;
    //     }
    // }
    // private JumpData jumpData;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        moveDirection = new Vector3(0, 0);
    }

    private void Update()
    {
        if(!runtimeData.lockInput)
        {
            if(Input.GetButtonDown("Select") && interactable != null && !runtimeData.interactTriggerCooldown)
            {
                if(interactable.faceObject)
                {
                    Vector2 direction = (interactable.transform.position - transform.position).normalized;
                    SetDirection(direction);
                }
                interactable.Interact(); 
            }
            else
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
                    Vector2 tempDirection = new Vector2(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y));
                    SetDirection(tempDirection);
                }
            }
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        animator.SetFloat("Look X", newDirection.x);
        animator.SetFloat("Look Y", newDirection.y);
    }

    public Vector3 LookDirection()
    {
        return new Vector3(animator.GetFloat("Look X"), animator.GetFloat("Look Y"));
    }

    public void FixedUpdate()
    {      
        if(!runtimeData.lockInput)
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
            GetComponent<Rigidbody>().velocity = new Vector3(moveDirection.x * currentSpeed, moveDirection.y * currentSpeed);

            //update animation
            animator.SetFloat("Speed", GetComponent<Rigidbody>().velocity.sqrMagnitude);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }
    }

    // public void Jump(JumpType jumpType, Vector3 jumpDestination)
    // {
    //     if(jumpData == null)
    //     {
    //         disableControl = true;
    //         if(actionPrompt.enabled == true)
    //             actionPrompt.enabled = false;
            
    //         jumpData = new JumpData(jumpType, transform.position, jumpDestination);
    //         rigidbody.velocity = Vector3.zero;

    //         switch ((int)jumpType)
    //         {
    //             case 0:        
    //                 gameObject.layer = gameObject.layer - 1;
    //                 animator.SetBool("Jump", true);
    //                 break;
    //             case 1:
    //                 sprite.sortingOrder = sprite.sortingOrder + 1;
    //                 gameObject.layer = gameObject.layer + 1;
    //                 animator.SetBool("Jump", true);
    //                 break;
    //             case 2:
    //                 animator.SetBool("Jump", true);
    //                 break;
    //             default:
    //                 break;
    //         }
    //         GetComponent<BoxCollider2D>().enabled = false;
    //     }
    // }

    // public void EndJump()
    // {
    //     switch ((int)jumpData.jumpType)
    //     {
    //         case 0:    
    //             sprite.sortingOrder = sprite.sortingOrder - 1;    
    //             animator.SetBool("Jump", false);
    //             break;
    //         case 1:
    //             animator.SetBool("Jump", false);
    //             break;
    //         case 2:
    //             animator.SetBool("Jump", false);
    //             break;
    //         default:
    //             break;
    //     }
    //     jumpData = null;
    //     GetComponent<BoxCollider2D>().enabled = true;
    //     disableControl = false;
    // }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Interactable"))
        {
            interactable = other.gameObject.GetComponent<Interactable>();
            DisplayActionPrompt();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Interactable"))
        {
            if(interactable)
            {
                interactable = null; 
            }
            HideActionPrompt();
        }
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