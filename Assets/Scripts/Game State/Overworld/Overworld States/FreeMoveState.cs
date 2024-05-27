using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class FreeMoveState : OverworldState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void StateUpdate()
    {
        //if (Input.GetButtonDown("Pause"))
        //{
        //    stateMachine.ChangeState((int)OverworldStateType.Paused);
        //}
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
