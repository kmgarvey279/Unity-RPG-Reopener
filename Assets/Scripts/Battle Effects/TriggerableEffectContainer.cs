using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class TriggerableEffectContainer
//{
    //[field: Header("Trigger Type/Conditions/Rate")]
    //[field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    //[field: SerializeField] public TriggerableEffectType TriggerableEffectType { get; private set; }
    
    //[SerializeField] private List<BattleConditionContainer> triggerConditions = new List<BattleConditionContainer>();
    //[SerializeField][Range(0.01f, 1f)] private float triggerRate;
    //[SerializeField] private bool onlyTriggerOnHit;

    //[field: Header("Targeting")]
    //[field: SerializeField] public bool UseTargetOverride { get; private set; }
    //[field: SerializeField] public TargetingType TargetOverride { get; private set; }
    //[field: SerializeField] public bool PickRandomTarget { get; private set; }

    //[field: Header("UI Info")]
    //[field: SerializeField] public string EffectName { get; private set; }

    //[field: SerializeField] public TriggerableEffect TriggerableEffect { get; private set; }
    //[field: SerializeField] public float Value { get; private set; } = 0;

    //[field: Header("Animations")]
    //[field: SerializeField] public List<ActionAnimationData> ActionAnimations { get; private set; } = new List<ActionAnimationData>();

    //public bool ConditionsCheck(Combatant actor, Combatant target, ActionSummary actionSummary)
    //{
    //    if (actionSummary == null || actionSummary.Action == null)
    //    {
    //        Debug.Log("error: action summary/action missing");
    //        return false;
    //    }
    //    if (onlyTriggerOnHit && !actionSummary.Values[ActionSummaryValue.DidHit])
    //    {
    //        return false;
    //    }
    //    bool conditionsSatisfied = true;
    //    foreach (BattleConditionContainer battleConditionContainer in triggerConditions)
    //    {
    //        Combatant combatantToCheck = actor;
    //        if (battleConditionContainer.BattleEventType == BattleEventType.Targeted)
    //        {
    //            combatantToCheck = target;
    //        }
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

    //public bool TriggerCheck()
    //{
    //    return Roll(triggerRate);
    //}

    //public bool Roll(float chance)
    //{
    //    float roll = Random.Range(0.01f, 1f);
    //    if (roll <= chance)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
//}



