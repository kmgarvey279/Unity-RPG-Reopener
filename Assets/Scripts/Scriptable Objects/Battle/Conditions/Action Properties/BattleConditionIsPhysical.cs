using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/Attack/Is Physical")]
public class BattleConditionIsPhysical : BattleCondition
{
    private List<ElementalProperty> physicalElements = new List<ElementalProperty>()
    {
        ElementalProperty.Slash,
        ElementalProperty.Strike,
        ElementalProperty.Pierce
    };

    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (actionSummary != null && actionSummary.Action is Attack)
        {
            Attack attack = (Attack)actionSummary.Action;
            if (physicalElements.Contains(attack.ElementalProperty))
            {
                return true;
            }
            return false;
        }

        return false;
    }
}
