using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerShieldState : PlayerState
{
    private bool isParrying;
    public float parryDuration;
    private float parryTimer;

    public override void OnEnter()
    {
        nextState = "";
        character.rigidbody.velocity = Vector2.zero;
        character.animator.SetBool("Defending", true);
        parryTimer = parryDuration;
        isParrying = true;
    }

    public override void StateUpdate()
    {
        HandleInputs();
        if(isParrying)
        {
            if(parryTimer <= 0)
            {
                isParrying = false;
                character.animator.SetTrigger("End Parry");
            }
            else
            {
                parryTimer -= Time.deltaTime;
            }
        }
    }

    public override string CheckConditions()
    {   
        return nextState;
    }

    public override void OnExit()
    {
        character.animator.SetBool("Defending", false);
    }

    void HandleInputs()
    {
        if(Input.GetButtonUp("Guard")) 
        {
            nextState = "MoveState";
        }
    }
}

