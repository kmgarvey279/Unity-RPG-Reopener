using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerMoveState : PlayerState
{
    public override void OnEnter()
    {
        nextState = "";
    }

    public override void StateUpdate()
    {
        HandleInput();
        playerMoveManager.HandleMoveLogic();
    }

    public override void StateFixedUpdate()
    {
         playerMoveManager.HandleMovePhysics();  
    }


    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {

    }

    private void HandleInput()
    {
        if(Input.GetButtonDown("Dash"))
        {
            nextState = "DashState";
        } else if (Input.GetButtonDown("Light Attack")) {
            nextState = "AttackState";
        } else if (Input.GetButtonDown("Shoot")) {
            nextState = "ShootState";
        } else if (Input.GetButtonDown("Guard")) {
           nextState = "ShieldState";
        }
    }

}
