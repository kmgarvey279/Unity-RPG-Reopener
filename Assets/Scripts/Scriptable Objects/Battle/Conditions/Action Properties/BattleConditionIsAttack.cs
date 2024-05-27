using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/Attack/Is Attack")]
public class BattleConditionIsAttack : BattleCondition
{
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Action is Attack)
        {
            return true;
        }
        return false;
    }
}
