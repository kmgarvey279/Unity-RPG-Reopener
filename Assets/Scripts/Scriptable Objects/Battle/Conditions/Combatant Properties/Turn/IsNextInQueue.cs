using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant/Turn/Next")]
public class IsNextInQueue : BattleCondition
{
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (combatantToCheck != null)
        {
            int nextTurnIndex = combatantToCheck.GetNextTurnIndex(false);

            if (nextTurnIndex == 1) 
            {
                return true;
            }
        }
        return false;
    }
}
