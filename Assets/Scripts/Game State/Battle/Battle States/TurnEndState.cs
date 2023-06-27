using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class TurnEndState : BattleState
{
    [SerializeField] private BattleEventQueue battleEventQueue;
    [SerializeField] private SignalSender onInterventionEnd;
    private WaitForSeconds waitZeroPointTwoFive = new WaitForSeconds(0.25f);
    public override void OnEnter()
    {
        Debug.Log("Entering Turn End State");
        base.OnEnter();
        StartCoroutine(EndTurnCo());
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("Intervention"))
        {
            battleTimeline.AddTurn(TurnType.Intervention, battleManager.GetCombatants(CombatantType.Player)[0]);
        }
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
        yield return waitZeroPointTwoFive;

        //battleManager.ClearTimelineTargetPreview();

        if (battleTimeline.CurrentTurn.TurnType != TurnType.Intervention && !battleTimeline.CurrentTurn.IsDead)
        {
            //add turn start effects to queue
            yield return StartCoroutine(battleTimeline.CurrentTurn.Actor.OnTurnEndCo());

            //execute any turn end effects
            yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());
            yield return waitZeroPointTwoFive;

            //check combatants for ko
            for (int i = battleManager.Combatants.Count - 1; i >= 0; i--)
            {
                //resolve health change
                battleManager.Combatants[i].ResolveHealthChange();
                if (battleManager.Combatants[i].HP.CurrentValue <= 0)
                {
                    battleManager.KOCombatant(battleManager.Combatants[i]);
                }
            }
            yield return waitZeroPointTwoFive;
        }
        //end battle if all of one side is dead
        if (battleManager.GetCombatants(CombatantType.Enemy).Count <= 0 || battleManager.GetCombatants(CombatantType.Player).Count <= 0)
        {
            stateMachine.ChangeState((int)BattleStateType.BattleEnd);
        }
        //continue intervention if possible
        else if (battleTimeline.CurrentTurn.TurnType == TurnType.Intervention && battleManager.InterventionPoints >= 25)
        {
            battleTimeline.ResetInterventionPanel();
            stateMachine.ChangeState((int)BattleStateType.InterventionStart);
        }
        else
        {
            //unhighlight party panel
            if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
            {
                PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
                playableCombatant.BattlePartyPanel.Highlight(false);
            }
            //destroy previous turn panel
            battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
            //switch to change turn state
            stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
        }
    }
}
