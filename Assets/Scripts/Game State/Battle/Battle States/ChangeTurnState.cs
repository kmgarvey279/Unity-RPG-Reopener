using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTurnState : BattleState
{
    private WaitForSeconds wait05 =  new WaitForSeconds(0.5f);
    public override void OnEnter()
    {
        Debug.Log("Entering Change Turn State");
        base.OnEnter();
        battleManager.ToggleCanQueueInterventions(false);
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
        battleManager.ToggleCanQueueInterventions(true);
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

        if (battleTimeline.CurrentTurn.Actor != null && battleTimeline.CurrentTurn.Actor.CombatantState == CombatantState.KO)
        {
            while (battleTimeline.CurrentTurn.Actor.CombatantState == CombatantState.KO)
            {
                //destroy previous turn panel
                battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
                yield return wait05;

                yield return StartCoroutine(battleTimeline.AdvanceCo());
            }
        }


        //if next turn is an intervention:
        if (battleTimeline.CurrentTurn.IsIntervention)
        {
            stateMachine.ChangeState((int)BattleStateType.InterventionStart);
        }
        //if next turn is a queued cast:
        //else if (battleTimeline.CurrentTurn.TurnType == TurnType.Cast)
        //{
        //    battleTimeline.CurrentTurn.Actor.OnCastStart();
        //    StartQueuedCast();
        //}
        else
        {
            stateMachine.ChangeState((int)BattleStateType.TurnStart);
        }
    }

    //private void StartQueuedCast()
    //{
    //    Turn currentTurn = battleTimeline.CurrentTurn;
    //    //move on to next turn if the cast can't occur
    //    if (currentTurn.Action == null)
    //    {
    //        Debug.Log("error: no action assigned to cast");
    //        battleTimeline.RemoveTurn(currentTurn, false);
    //        stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
    //        return;
    //    }

    //    if (currentTurn.Actor.CombatantType == CombatantType.Enemy)
    //    {
    //        //recheck avalable targets
    //        if (currentTurn.Action.AOEType == AOEType.All)
    //        {
    //            battleTimeline.CurrentTurn.SetTargets(battleManager.GetCombatants(currentTurn.TargetedCombatantType));
    //            stateMachine.ChangeState((int)BattleStateType.Execute);
    //        }
    //    }
    //    else
    //    {
    //        //select a new target if the orginal target is dead
    //        if (currentTurn.ReselectTargets)
    //        {
    //            currentTurn.Targets.Clear();
    //            stateMachine.ChangeState((int)BattleStateType.TargetSelect);
    //        }
    //        //otherwise, recheck targets in aoe range
    //        else
    //        {
    //            currentTurn.SetTargets(gridManager.GetTargets(currentTurn.Targets[0].Tile, currentTurn.Action.AOEType, currentTurn.Action.IsMelee, currentTurn.TargetedCombatantType));
    //            stateMachine.ChangeState((int)BattleStateType.Execute);
    //        }
    //    }
    //}
}
