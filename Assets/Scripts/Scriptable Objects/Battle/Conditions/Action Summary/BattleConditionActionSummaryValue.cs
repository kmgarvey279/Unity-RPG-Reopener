using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action Summary/Value")]
public class BattleConditionActionSummaryValue : BattleCondition
{
    [field: SerializeField] public ActionSummaryValue ActionSummaryValue { get; private set; }

    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Values[ActionSummaryValue])
        {
            return true;
        }
        return false;
    }
}
