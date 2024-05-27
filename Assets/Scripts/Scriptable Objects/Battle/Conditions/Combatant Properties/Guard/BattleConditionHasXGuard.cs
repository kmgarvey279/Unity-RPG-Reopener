using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant/Enemy/Has X Guard")]
public class BattleConditionHasXGuard : BattleCondition
{
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        int intValue = Mathf.FloorToInt(value);

        if (combatantToCheck is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)combatantToCheck;
            if (enemyCombatant.Guard == intValue)
            {
                return true;
            }
        }
        return false;
    }
}
