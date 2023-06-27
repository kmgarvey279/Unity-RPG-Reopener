using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTurnState : BattleState
{
    public override void OnEnter()
    {
        Debug.Log("Entering Change Turn State");
        base.OnEnter();
        StartCoroutine(AdvanceTimelineCo());
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

    public IEnumerator AdvanceTimelineCo()
    {
        //check if all combatants are dead 
        if (battleTimeline.TurnQueue.Count < 0)
        {
            stateMachine.ChangeState((int)BattleStateType.BattleEnd);
            yield break;
        }

        yield return StartCoroutine(battleTimeline.AdvanceCo());

        //if next turn is an intervention:
        if (battleTimeline.CurrentTurn.TurnType == TurnType.Intervention)
        {
            StartIntervention();
        }
        //if next turn was "paused" due to an intervention, use saved turn data
        else if (battleTimeline.CurrentTurn.TurnType == TurnType.Paused)
        {
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        //if next turn is a queued cast:
        else if (battleTimeline.CurrentTurn.TurnType == TurnType.Cast)
        {
            StartQueuedCast();
        }
        else
        {
            stateMachine.ChangeState((int)BattleStateType.TurnStart);
        }
    }

    private void StartIntervention()
    {
        stateMachine.ChangeState((int)BattleStateType.InterventionStart);
    }

    private void StartQueuedCast()
    {
        Turn currentTurn = battleTimeline.CurrentTurn;
        //move on to next turn if the cast can't occur
        if (currentTurn.Action == null)
        {
            Debug.Log("error: no action assigned to cast");
            battleTimeline.RemoveTurn(currentTurn, false);
            stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
            return;
        }

        if (currentTurn.Actor.CombatantType == CombatantType.Enemy)
        {
            //recheck avalable targets
            if (currentTurn.Action.AOEType == AOEType.All)
            {
                battleTimeline.UpdateTurnTargets(currentTurn, battleManager.GetCombatants(currentTurn.TargetedCombatantType));
                stateMachine.ChangeState((int)BattleStateType.Execute);
            }
        }
        else
        {
            //select a new target if the orginal target is dead
            if (currentTurn.ReselectTargets)
            {
                currentTurn.Targets.Clear();
                stateMachine.ChangeState((int)BattleStateType.TargetSelect);
            }
            //otherwise, recheck targets in aoe range
            else
            {
                battleTimeline.UpdateTurnTargets(currentTurn, gridManager.GetTargets(currentTurn.Targets[0].Tile, currentTurn.Action.AOEType, currentTurn.Action.IsMelee, currentTurn.TargetedCombatantType));
                stateMachine.ChangeState((int)BattleStateType.Execute);
            }
        }
        //if (currentTurn.Targets.Count == 0)
        //{
        //    Debug.Log("error: no targets for cast");
        //    battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
        //    StartCoroutine(AdvanceTimelineCo());
        //    return;
        //}
    }
}
