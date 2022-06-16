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
        StartCoroutine(StartCo());
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

    private IEnumerator StartCo()
    {
        Debug.Log("Entering start coroutine");
        StartCoroutine(battleManager.SpawnCombatants());
        yield return new WaitUntil(() => battleManager.battleIsLoaded);
        stateMachine.ChangeState((int)BattleStateType.TurnStart);
    }
}

