using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Attack Type")]
public class BattleConditionAttackType : BattleCondition
{
    [SerializeField] private AttackType attackType;
    public override bool CheckCondition(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Action.AttackType == attackType)
        {
            return true;
        }
        return false;
    }
}
