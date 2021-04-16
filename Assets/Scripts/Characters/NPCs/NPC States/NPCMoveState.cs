using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class NPCMoveState : NPCState
{
    public Vector3 destination;
    public Vector3 moveDirection;
    public bool moving;

    public override void OnEnter()
    {
        // moving = false;
        // moveDirection = new Vector3(0,0,0);
        nextState = "";
        // SetDestination();
    }

    public override void StateUpdate()
    {
        // CalculateMovement();
    }

    public override void StateFixedUpdate()
    {
        // ExecuteMovement();
    }


    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {

    }

    public virtual void SetDestination()
    {

    }

    public virtual void DestinationCheck()
    {

    }

    public void CalculateMovement()
    {
        if(moving)
        {
            moveDirection = (destination - transform.position).normalized;
            DestinationCheck();  
        }
    }

    public void ExecuteMovement()
    { 
        if(moving)
        {
            //update direction
            character.ChangeLookDirection(moveDirection);

            //move
            character.GetComponent<Rigidbody>().velocity = new Vector3(moveDirection.x * character.characterInfo.moveSpeed, moveDirection.y * character.characterInfo.moveSpeed);

            //update animation
            float tempX = Mathf.Round(character.lookDirection.x);
            float tempY = Mathf.Round(character.lookDirection.y);
            animator.SetFloat("Look X", tempX);
            animator.SetFloat("Look Y", tempY);
        }
        animator.SetFloat("Speed", moveDirection.sqrMagnitude);
    }
}
