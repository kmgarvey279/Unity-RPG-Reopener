using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant/IsBoss")]
public class BattleConditionIsBoss : BattleCondition
{
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (combatantToCheck != null && combatantToCheck is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)combatantToCheck;
            if (enemyCombatant.IsBoss)
            {
                return true;
            }
        }
        return false;
    }
}
