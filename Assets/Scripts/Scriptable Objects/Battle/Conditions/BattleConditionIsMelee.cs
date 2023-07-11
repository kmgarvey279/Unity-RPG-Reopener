using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Is Melee")]
public class BattleConditionIsMelee : BattleCondition
{
    public override bool CheckCondition(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        if (actionSummary != null && actionSummary.Action.IsMelee)
        {
            return true;
        }

        return false;
    }
}
