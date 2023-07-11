using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action Type")]
public class BattleConditionActionType : BattleCondition
{
    [SerializeField] private ActionType actionType;
    public override bool CheckCondition(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Action.ActionType == actionType)
        {
            return true;
        }
        return false;
    }
}
