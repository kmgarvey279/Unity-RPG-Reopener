using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant State")]
public class BattleConditionCombatantState : BattleCondition
{
    [SerializeField] private CombatantState combatantState;

    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (combatantToCheck.CombatantState == combatantState)
        {
            return true;
        }
        return false;
    }
}
