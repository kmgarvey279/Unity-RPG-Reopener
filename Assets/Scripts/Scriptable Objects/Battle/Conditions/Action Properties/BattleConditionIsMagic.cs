using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action/Attack/Is Magic")]
public class BattleConditionIsMagic : BattleCondition
{
    private List<ElementalProperty> magicElements = new List<ElementalProperty>() 
    {
        ElementalProperty.Fire,
        ElementalProperty.Ice,
        ElementalProperty.Electric,
        ElementalProperty.Dark,
        ElementalProperty.None
    };
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        if (actionSummary != null && actionSummary.Action is Attack)
        {
            Attack attack = (Attack)actionSummary.Action;
            if (magicElements.Contains(attack.ElementalProperty))
            {
                return true;
            }
            return false;
        }

        return false;
    }
}
