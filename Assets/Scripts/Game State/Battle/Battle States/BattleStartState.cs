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
        Debug.Log("Entering Battle Start");
        battleManager.SpawnCombatants();
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
        Debug.Log("Entering start coroutine");
        yield return new WaitForSeconds(1f);
        stateMachine.ChangeState((int)BattleStateType.TurnStart);
    }
}

