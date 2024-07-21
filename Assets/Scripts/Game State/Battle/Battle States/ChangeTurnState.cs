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
        yield return StartCoroutine(battleTimeline.AdvanceCo());

        //check if all combatants are dead 
        if (battleManager.GetCombatants(CombatantType.Player).Count < 1)
        {
            stateMachine.ChangeState((int)BattleStateType.GameOver);
            yield break;
        }
        if (battleManager.GetCombatants(CombatantType.Enemy).Count < 1)
        {
            stateMachine.ChangeState((int)BattleStateType.BattleEnd);
            yield break;
        }

        //if (battleTimeline.CurrentTurn != null && battleTimeline.CurrentTurn.Actor != null && battleTimeline.CurrentTurn.Actor.CombatantState == CombatantState.KO)
        //{
        //    //    while (battleTimeline.CurrentTurn.Actor.CombatantState == CombatantState.KO)
        //    //    {
        //    //        //destroy previous turn panel
        //    //        battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
        //    //        yield return wait05;

        //    //        yield return StartCoroutine(battleTimeline.AdvanceCo());
        //    //    }
        //    yield return wait05;
        //}


        //if next turn is an intervention:
        if (battleTimeline.CurrentTurn.IsIntervention)
        {
            stateMachine.ChangeState((int)BattleStateType.InterventionStart);

        }
        else
        {
            stateMachine.ChangeState((int)BattleStateType.TurnStart);
        }
    }
}
