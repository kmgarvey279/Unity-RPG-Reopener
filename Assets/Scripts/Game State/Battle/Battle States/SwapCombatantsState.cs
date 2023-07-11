using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwapCombatantsState : BattleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("entering swap combatants state");

        battleTimeline.TakeSnapshot();
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("Submit"))
        {
            battleManager.SwapPlayableCombatants((PlayableCombatant)battleTimeline.CurrentTurn.Actor);
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else if (Input.GetButton("Cancel"))
        {
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
