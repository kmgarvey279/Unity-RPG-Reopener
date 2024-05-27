using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using JetBrains.Annotations;

[System.Serializable]
public class TurnStartState : BattleState
{
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private BattleEventQueue battleEventQueue;
    //cache wait for seconds
    private WaitForSeconds wait025 = new WaitForSeconds(0.25f);
    private WaitForSeconds waitZeroPointFive = new WaitForSeconds(0.5f);
    [SerializeField] private Action wait;

    public override void OnEnter()
    {
        Debug.Log("Entering Turn Start State");
        base.OnEnter();
        StartCoroutine(StartTurnCo());
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
        yield return wait025;

        //add turn start effects to queue + trigger
        battleTimeline.CurrentTurn.Actor.OnTurnStart();
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());

        //display any timeline changes
        battleTimeline.DisplayTurnOrder();

        //clear status effects
        yield return StartCoroutine(battleTimeline.CurrentTurn.Actor.ClearExpiredStatusEffectsCo(TurnEventType.OnStart));
        
        //refresh guard
        //battleTimeline.CurrentTurn.Actor.RefreshGuard();
        
        //resolve health changes
        yield return StartCoroutine(battleManager.ResolveBarChanges());

        yield return wait025;

        battleManager.HideHealthBars();

        //if actor cannot act
        if (battleTimeline.CurrentTurn.Actor.CheckBool(CombatantBool.CannotActOnTurn))
        {
            battleTimeline.CurrentTurn.SetAction(wait);
            battleTimeline.CurrentTurn.SetTargets(new List<Combatant>() { battleTimeline.CurrentTurn.Actor });
            stateMachine.ChangeState((int)BattleStateType.Execute);
        }
        //otherwise, act like normal
        else if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
        {
            stateMachine.ChangeState((int)BattleStateType.PlayerTurn);
        }
        else if (battleTimeline.CurrentTurn.Actor is EnemyCombatant)
        {
            stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
        }
    }
}
