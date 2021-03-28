using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class NPCActionState : NPCState
{
    private string animatorTrigger;  

    public override void OnEnter()
    {
        nextState = "";
        character.actionManager.currentAction.TakeAction();
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {
        character.animator.SetTrigger("End Action");
        character.actionManager.FinishAction();
    }


    public void OnAnimationComplete()
    {
        nextState = "MoveState";
    }

}
