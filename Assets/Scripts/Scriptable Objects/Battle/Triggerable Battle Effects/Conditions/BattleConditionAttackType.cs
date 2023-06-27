using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Triggerable Effects/Conditions/Attack Type")]
public class BattleConditionAttackType : BattleCondition
{
    [SerializeField] private AttackType attackType;
    public override bool CheckCondition(ActionSubevent actionSubevent)
    {
        if(actionSubevent.Action.AttackType == attackType)
        {
            return true;
        }
        return false;
    }
}
