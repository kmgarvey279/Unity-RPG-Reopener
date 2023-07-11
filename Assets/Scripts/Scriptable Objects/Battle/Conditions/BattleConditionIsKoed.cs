using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Is KOed")]
public class BattleConditionIsKoed : BattleCondition
{
    [SerializeField] private bool checkActor;

    public override bool CheckCondition(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        Combatant combatantToCheck = target;
        if (checkActor)
        {
            combatantToCheck = actor;
        }
        if (target.IsKOed)
        {
            return true;
        }

        return false;
    }
}
