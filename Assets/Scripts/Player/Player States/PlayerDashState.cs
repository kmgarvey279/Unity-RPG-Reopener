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
    [Header("Afterimage Effect")]
    public Afterimage afterimage;
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
        if(animator.GetFloat("Speed") <= 0.01)
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
        animator.SetTrigger("End Action");
        afterimage.createAfterimage = false;
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
            animator.SetTrigger("Dashing");  
        }
        else 
        {
            animator.SetTrigger("Backstepping");
            speedTemp = -speedTemp;  
        }
        playerRB.velocity = new Vector3(animator.GetFloat("Look X") * dashSpeed,
            animator.GetFloat("Look Y") * dashSpeed, 0); 
        afterimage.createAfterimage = true;
    }
}

