using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class BattleStartState : BattleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(StartBattleCo());
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

    private IEnumerator StartBattleCo()
    {
        yield return StartCoroutine(battleManager.StartBattleCo());
        stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
    }
}

