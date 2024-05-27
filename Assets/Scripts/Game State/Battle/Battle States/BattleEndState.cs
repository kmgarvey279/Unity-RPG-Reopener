using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class BattleEndState : BattleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        battleManager.ToggleCanQueueInterventions(false);
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
    }
}
