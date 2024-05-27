using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/Is This Action")]
public class BattleConditionIsThisAction : BattleCondition
{
    [SerializeField] private Action action;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Action == action)
        {
            return true;
        }
        return false;
    }
}
