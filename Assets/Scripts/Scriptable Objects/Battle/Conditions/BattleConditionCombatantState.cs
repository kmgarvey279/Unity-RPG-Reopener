using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant State")]
public class BattleConditionCombatantState : BattleCondition
{
    [SerializeField] private bool checkActor;
    [SerializeField] private CombatantState combatantState;

    public override bool CheckCondition(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        Combatant combatantToCheck = target;
        if (checkActor)
        {
            combatantToCheck = actor;
        }
        if (combatantToCheck.CombatantState == combatantState)
        {
            return true;
        }

        return false;
    }
}
