using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class CutsceneState : OverworldState
{
    public override void OnEnter()
    {
        base.OnEnter();
        runtimeData.lockInput = true;
    }

    public override void StateUpdate()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        runtimeData.lockInput = false;
    }

    public void OnUnlockInput()
    {
        stateMachine.ChangeState((int)OverworldStateType.FreeMove);
    }
}
