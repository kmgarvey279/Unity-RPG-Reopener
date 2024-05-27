using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action Summary/Damage Inflicted/Bool")]
public class BattleConditionDidInflictDamage : BattleCondition
{
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (actionSummary != null && actionSummary.CumHealthEffect > 0)
        {
            return true;
        }
        return false;
    }
}
