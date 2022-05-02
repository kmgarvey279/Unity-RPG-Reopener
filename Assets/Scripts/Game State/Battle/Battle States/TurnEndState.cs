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
        yield return new WaitForSeconds(0.4f);
        
        battleManager.turnData.combatant.OnTurnEnd();
        if(battleManager.turnData.combatant.hp.GetCurrentValue() <= 0)
        {
            battleManager.KOCombatant(battleManager.turnData.combatant);
        }
        
        yield return new WaitForSeconds(0.4f);

        bool allPartyMembersKO = true;
        foreach(PlayableCombatant playableCombatant in battleManager.PlayableCombatants)
        {
            if(playableCombatant.ko == false)
            {
                allPartyMembersKO = false;
            }
        }
        if(battleManager.EnemyCombatants.Count <= 0 || allPartyMembersKO == true)
        {
            stateMachine.ChangeState((int)BattleStateType.BattleEnd);
        }
        else
        {
            stateMachine.ChangeState((int)BattleStateType.TurnStart);
        }
    }
}
