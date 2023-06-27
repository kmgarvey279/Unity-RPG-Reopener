using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Triggerable Effects/Conditions/Elemental Type")]
public class BattleConditionElementalType : BattleCondition
{
    [SerializeField] private ElementalProperty elementalProperty;
    public override bool CheckCondition(ActionSubevent actionSubevent)
    {
        if(actionSubevent.Action.ElementalProperty == elementalProperty)
        {
            return true;
        }
        return false;
    }
}
