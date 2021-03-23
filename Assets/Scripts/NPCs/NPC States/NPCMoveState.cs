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
        moveManager.HandleMoveLogic();
    }

    public override void StateFixedUpdate()
    {
        moveManager.HandleMovePhysics();  
    }


    public override string CheckConditions()
    {
        if(actionManager.currentAction != null)
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
