using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventModifierProtect : ActionEventModifier
{
    public float minHPPercentage;
    public override ActionEvent ApplyModifiers(ActionEvent actionEvent, Combatant combatant)
    {
        if(actionEvent.action.isMelee 
            && actionEvent.targets.Count == 1 
            && actionEvent.targets[0] != combatant
            && actionEvent.targets[0].hp.GetCurrentValue() <= actionEvent.targets[0].hp.GetValue() / minHPPercentage)
        {
            if(Roll(chance))
            {
                actionEvent.overrideTarget = combatant;
            }
        }
        return base.ApplyModifiers(actionEvent, combatant);
    }
}
