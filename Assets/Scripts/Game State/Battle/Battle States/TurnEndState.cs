using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class TurnEndState : BattleState
{
    [SerializeField] private BattleEventQueue battleEventQueue;
    [SerializeField] private SignalSender onInterventionEnd;
    private WaitForSeconds wait025 = new WaitForSeconds(0.25f);
    private WaitForSeconds wait1 = new WaitForSeconds(1f);
    public override void OnEnter()
    {
        Debug.Log("Entering Turn End State");
        base.OnEnter();
        StartCoroutine(EndTurnCo());
    }
    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }
    
    public IEnumerator EndTurnCo()
    {
        //trigger turn end effects
        if (!battleTimeline.CurrentTurn.IsIntervention && battleTimeline.CurrentTurn.Actor.CombatantState != CombatantState.KO)
        {
            //add turn end effects to queue and trigger
            battleTimeline.CurrentTurn.Actor.OnTurnEnd();
            yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());

            //display any timeline changes
            battleTimeline.DisplayTurnOrder();

            //reset positions
            battleManager.ResetCombatantPositions();

            //clear status effects
            yield return StartCoroutine(battleTimeline.CurrentTurn.Actor.ClearExpiredStatusEffectsCo(TurnEventType.OnEnd));

            //resolve health changes
            yield return StartCoroutine(battleManager.ResolveBarChanges());

            //check combatants for ko
            yield return StartCoroutine(battleManager.ResolveKOs());
        }

        if (battleTimeline.CurrentTurn.Actor.CombatantState != CombatantState.KO)
        {
            //unhighlight party panel
            if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
            {
                PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
                playableCombatant.BattlePartyPanel.OnTurnEnd();
            }

            //destroy previous turn panel
            battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
        }

        yield return wait025;

        battleManager.ResetCombatantAnimations();
        battleManager.HideHealthBars();

        //reset menu
        //battleManager.BattleData.SetLastCommandMenuStateType(CommandMenuStateType.Main);

        //switch to change turn state
        stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
    }
}
