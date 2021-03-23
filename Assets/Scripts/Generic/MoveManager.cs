using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public Vector3 lookDirection;
    public Vector3 moveDirection;
    public float moveSpeed;
    [HideInInspector]
    public Rigidbody2D myRB;
    [HideInInspector]
    public Animator myAnimator;
    // Start is called before the first frame update
    public virtual void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        lookDirection = new Vector3(0,-1,0); 
        moveDirection = new Vector3(0,0,0);
    }

    public virtual void HandleMoveLogic()
    {    
    }

    public void HandleMovePhysics()
    {
        float speedTemp = moveSpeed;
        if(lookDirection.x != moveDirection.x && lookDirection.y != moveDirection.y)
        {
            speedTemp = speedTemp / 2f;
        }
        else if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
        {
            lookDirection = moveDirection;
        }
        myRB.velocity = new Vector3(moveDirection.x * speedTemp, moveDirection.y * speedTemp);
        UpdateAnimator();
    }

    public void UpdateAnimator()    
    {     
        myAnimator.SetFloat("Look X", Mathf.Round(lookDirection.x));
        myAnimator.SetFloat("Look Y", Mathf.Round(lookDirection.y));
        myAnimator.SetFloat("Speed", moveDirection.sqrMagnitude);
    }
}
