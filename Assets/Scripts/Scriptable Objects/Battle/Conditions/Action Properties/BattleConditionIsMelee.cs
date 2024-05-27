using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/Attack/Is Melee")]
public class BattleConditionIsMelee : BattleCondition
{
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (actionSummary != null && actionSummary.Action.IsMelee)
        {
            return true;
        }

        return false;
    }
}
