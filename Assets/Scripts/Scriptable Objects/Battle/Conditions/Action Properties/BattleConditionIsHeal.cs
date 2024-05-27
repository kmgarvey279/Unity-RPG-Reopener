using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/Is Heal")]
public class BattleConditionIsHeal : BattleCondition
{
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Action is Heal)
        {
            return true;
        }
        return false;
    }
}
