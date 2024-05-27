using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/Attack/Elemental Type")]
public class BattleConditionElementalType : BattleCondition
{
    [SerializeField] private ElementalProperty elementalProperty;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        if (actionSummary != null && actionSummary.Action is Attack)
        {
            Attack attack = (Attack)actionSummary.Action;
            if (attack.ElementalProperty == elementalProperty)
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
