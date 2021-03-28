using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class NPCMoveState : NPCState
{

 public override void OnEnter()
    {
        nextState = "";
    }

    public override void StateUpdate()
    {
        character.moveManager.HandleMoveLogic();
    }

    public override void StateFixedUpdate()
    {
        character.moveManager.HandleMovePhysics();  
    }


    public override string CheckConditions()
    {
        if(character.actionManager.currentAction != null)
        {
            return "ActionState";
        }
        else
        {
            return null;
        }
    }

    public override void OnExit()
    {

    }
}
