using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant/Bool")]
public class BattleConditionCombatantBool : BattleCondition
{
    [field: SerializeField] public CombatantBool combatantBool;

    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        if (combatantToCheck != null && combatantToCheck.CheckBool(combatantBool))
        {
            return true;
        }
        return false;
    }
}
