using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class TurnStartState : BattleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering Turn Start");
        StartCoroutine(StartTurnCo());
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

    private IEnumerator StartTurnCo()
    {   
        battleManager.AdvanceTimeline();
        yield return new WaitForSeconds(0.3f);
        battleManager.turnData.combatant.OnTurnStart();
        yield return new WaitForSeconds(0.3f);
        //get next state
        if(battleManager.turnData.combatant is PlayableCombatant)
        {
            Debug.Log("Player Turn Start");
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else if(battleManager.turnData.combatant is EnemyCombatant)
        {
            Debug.Log("Enemy Turn Start");
            stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
        }
    }
}
