using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerDashState : PlayerState
{
    private enum DashType
    {
        dash,
        backstep
    }
    private DashType dashType;
    [Header("Dash Speed/Duration")]
    public float dashSpeed;
    public float dashDuration;
    private float dashTimer;
    [Header("Dash Cooldown")]
    private bool cooldown = false;
    public float cooldownDuration;
    private float cooldownTimer;
    [Header("Dash Attack")]
    public float dashAttackDuration;
    // public string OnPlayerShootState = "ShootState";
    // public string OnPlayerShieldState = "ShieldState";
    private bool moveFlag = false;
    private string onPlayerMoveState = "MoveState";

    public override void OnEnter()
    {
        nextState = "";
        cooldownTimer = cooldownDuration;
        dashTimer = dashDuration;
        if(character.animator.GetFloat("Speed") <= 0.01)
        {
            dashType = DashType.backstep; 
        } 
        else
        {
            dashType = DashType.dash;
        }
        Dash();
    }

    public override void StateUpdate()
    {
        HandleInputs();
        if(dashTimer <= 0)
        {
            // cooldown = true;
            dashTimer = dashDuration;
            nextState = "MoveState";
        } 
        else 
        {
            dashTimer -= Time.deltaTime;
        } 
    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {
        character.animator.SetTrigger("End Action");
        character.afterimage.createAfterimage = false;
    }

    void HandleInputs()
    {
        // if(Input.GetKeyDown(KeyCode.F))
        // {
        //     dashAttackTransition = true;
        // }
    }

    private void Dash()
    {   
        float speedTemp = dashSpeed;
        if(dashType == DashType.dash)
        {
            character.animator.SetTrigger("Dashing");  
        }
        else 
        {
            character.animator.SetTrigger("Backstepping");
            speedTemp = -speedTemp;  
        }
        character.rigidbody.velocity = new Vector3(character.animator.GetFloat("Look X") * dashSpeed,
            character.animator.GetFloat("Look Y") * dashSpeed, 0); 
        character.afterimage.createAfterimage = true;
    }
}

