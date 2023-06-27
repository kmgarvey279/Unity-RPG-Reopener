using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class TurnStartState : BattleState
{
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private BattleEventQueue battleEventQueue;
    //cache wait for seconds
    private WaitForSeconds waitZeroPointTwoFive = new WaitForSeconds(0.25f);
    private WaitForSeconds waitZeroPointFive = new WaitForSeconds(0.5f);
    private bool queueIntervention = false;

    public override void OnEnter()
    {
        Debug.Log("Entering Turn Start State");
        base.OnEnter();
        queueIntervention = false;
        StartCoroutine(StartTurnCo());
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("Intervention"))
        {
            if (!queueIntervention && battleTimeline.CurrentTurn.TargetedCombatantType == CombatantType.Player)
            {
                queueIntervention = true;
            }
            else
            {
                battleTimeline.AddTurn(TurnType.Intervention, battleManager.GetCombatants(CombatantType.Player)[0]);
            }
        }
    }


    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private IEnumerator StartTurnCo()
    {
        yield return waitZeroPointTwoFive;

        //add turn start effects to queue
        yield return StartCoroutine(battleTimeline.CurrentTurn.Actor.OnTurnStartCo());
        //execute any turn start effects
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());
        yield return waitZeroPointTwoFive;

        if(queueIntervention)
        {
            battleTimeline.CurrentTurn.PauseTurn();
            battleTimeline.AddTurn(TurnType.Intervention, battleTimeline.CurrentTurn.Actor);
            stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
        }
        else if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
        {
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else if (battleTimeline.CurrentTurn.Actor is EnemyCombatant)
        {
            stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
        }
    }
}
