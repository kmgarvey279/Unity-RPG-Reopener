using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class TurnEndState : BattleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(EndTurnCo());
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

    public IEnumerator EndTurnCo()
    {     
        yield return new WaitForSeconds(0.5f);
        //check combatant status after end of turn effects
        battleManager.turnData.combatant.OnTurnEnd();
        if(battleManager.turnData.combatant.hp.GetCurrentValue() <= 0)
        {
            battleManager.KOCombatant(battleManager.turnData.combatant);
        }
        yield return new WaitForSeconds(0.5f);         
        if(battleManager.GetCombatants(CombatantType.Enemy).Count <= 0 || battleManager.GetCombatants(CombatantType.Player).Count <= 0)
        {
            stateMachine.ChangeState((int)BattleStateType.BattleEnd);
        }
        else
        {
            stateMachine.ChangeState((int)BattleStateType.TurnStart);
        }
    }
}
