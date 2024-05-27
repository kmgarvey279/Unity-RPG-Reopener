using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant/Type")]
public class BattleConditionCombatantType : BattleCondition
{
    [SerializeField] private CombatantType combatantType;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (combatantToCheck != null && combatantToCheck.CombatantType == combatantType)
        {
            return true;
        }
        return false;
    }
}
