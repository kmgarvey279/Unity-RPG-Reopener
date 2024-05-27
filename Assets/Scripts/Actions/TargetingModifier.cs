using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetingModifier
{
    //[field: Header("Modifier")]
    //[field: SerializeField] public AOEType aoeOverride { get; private set; }
    //[field: Header("Conditions")]
    //[SerializeField] private List<BattleConditionContainer> conditionList = new List<BattleConditionContainer>();

    //public AOEType GetAOEType(AOEType defaultAOEType, Combatant actor, Combatant target, Action action)
    //{
    //    ActionSummary actionSummary = new ActionSummary(action);

    //    if (!ConditionsCheck(actor, target, actionSummary))
    //    {
    //        return defaultAOEType;
    //    }
    //    return aoeOverride;
    //}

    //public bool ConditionsCheck(Combatant actor, Combatant target, ActionSummary actionSummary)
    //{
    //    bool conditionsSatisfied = true;
    //    foreach (BattleConditionContainer battleConditionContainer in conditionList)
    //    {
    //        //get combatant to check
    //        Combatant combatantToCheck = actor;
    //        if (battleConditionContainer.BattleEventType == BattleEventType.Targeted)
    //        {
    //            combatantToCheck = target;
    //        }

    //        //check conditionals
    //        if (battleConditionContainer.Conditional == Conditional.If)
    //        {
    //            conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
    //        }
    //        else if (battleConditionContainer.Conditional == Conditional.And)
    //        {
    //            if (!conditionsSatisfied)
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
    //            }
    //        }
    //        else if (battleConditionContainer.Conditional == Conditional.Or)
    //        {
    //            if (conditionsSatisfied)
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
    //            }
    //        }
    //    }
    //    return conditionsSatisfied;
    //}
}
