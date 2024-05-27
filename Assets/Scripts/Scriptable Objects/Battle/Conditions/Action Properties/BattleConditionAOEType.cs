using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/AOE Type")]
public class BattleConditionAOEType : BattleCondition
{
    [SerializeField] private AOEType aoeType;

    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (actionSummary != null && actionSummary.Action.AOEType == aoeType)
        {
            return true;
        }

        return false;
    }
}
