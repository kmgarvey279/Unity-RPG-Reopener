using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Targeting Type")]
public class BattleConditionTargetingType : BattleCondition
{
    [SerializeField] private TargetingType targetingType;

    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (actionSummary != null && actionSummary.Action.TargetingType == targetingType)
        {
            return true;
        }

        return false;
    }
}
