using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Elemental Type")]
public class BattleConditionElementalType : BattleCondition
{
    [SerializeField] private ElementalProperty elementalProperty;
    public override bool CheckCondition(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Action.ElementalProperty == elementalProperty)
        {
            return true;
        }
        return false;
    }
}
