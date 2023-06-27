using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Triggerable Effects/Conditions/Action Type")]
public class BattleConditionActionType : BattleCondition
{
    [SerializeField] private ActionType actionType;
    public override bool CheckCondition(ActionSubevent actionSubevent)
    {
        if(actionSubevent.Action.ActionType == actionType)
        {
            return true;
        }
        return false;
    }
}
