using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class AllyControlState : StateMachine.State
{
    public PlayableCharacter character;

    public Vector3 moveDirection;
    public Vector3 lookDirection;
    public Transform leadAxis;

    public override void OnEnter()
    {
        character = GetComponentInParent<PlayableCharacter>();
        nextState = "";

        character.aiPath.canMove = false;
        
        lookDirection = character.lookDirection;
        moveDirection = new Vector3(0,0,0);
    }

    public override void StateUpdate()
    {
        HandleInput();
        CalculateMovement(); 
    }

    public override void StateFixedUpdate()
    { 
        ExecuteMovement();
    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {

    }

    public void CalculateMovement()
    {
        //check first axis (move)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, moveY).normalized;
        //check second axis (look/aim)
        float lookX = Input.GetAxisRaw("Horizontal Look");
        float lookY = Input.GetAxisRaw("Vertical Look");
        if(!Mathf.Approximately(lookX, 0.0f) || !Mathf.Approximately(lookY, 0.0f))
        {
            lookDirection = new Vector3(lookX, lookY).normalized; 
        }    
        else if(!Mathf.Approximately(moveX, 0.0f) || !Mathf.Approximately(moveY, 0.0f))
        {
            lookDirection = new Vector3(moveX, moveY).normalized; 
        }   
    }

    public void ExecuteMovement()
    {
        //rotate
        character.ChangeLookDirection(lookDirection);

        float angle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
        leadAxis.rotation = Quaternion.Euler(0, 0, angle);
        
        //move
        float speedTemp = character.characterInfo.moveSpeed;
        if(character.lookDirection.x != moveDirection.x && character.lookDirection.y != moveDirection.y)
        {
            speedTemp = speedTemp / 2f;
        }
        character.rigidbody.velocity = new Vector3(moveDirection.x * speedTemp, moveDirection.y * speedTemp);

        //update animation
        float tempX = Mathf.Round(character.lookDirection.x);
        float tempY = Mathf.Round(character.lookDirection.y);
        character.animator.SetFloat("Look X", tempX);
        character.animator.SetFloat("Look Y", tempY);
        character.animator.SetFloat("Speed", moveDirection.sqrMagnitude);
    }

    private void HandleInput()
    {
        // if(Input.GetButtonDown("Dash"))
        // {
        //     nextState = "DashState";
        // } else if (Input.GetButtonDown("Light Attack")) {
        //     nextState = "AttackState";
        // } else if (Input.GetButtonDown("Shoot")) {
        //     nextState = "ShootState";
        // } else if (Input.GetButtonDown("Guard")) {
        //     nextState = "ShieldState";
        // }
    }

    public void EnterBattleState()
    {
        nextState = "BattleState";
    }

}
