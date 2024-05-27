
using System.Collections.Generic;
using UnityEngine;

public enum BattleEventType
{
    Acting,
    Targeted
}

//also determines order of execution
public enum TriggerableEffectType
{
    Trigger,
    Buff,
    Debuff,
    Counter,
    Turn,
    Preemptive
}

public class BattleEventTarget
{
    public BattleEventTarget(Combatant combatant, float value)
    {
        Combatant = combatant;
        ValueOverride = value;
    }

    public Combatant Combatant { get; private set; }
    public float ValueOverride { get; private set; }
}

[System.Serializable]
public class TriggerableEffectContainer
{
    //public TriggerableEffectContainer(TriggerableEffect triggerableEffect, float value)
    //{
    //    TriggerableEffect = triggerableEffect;
    //    Value = value;
    //}
    public void TriggerEffect(Combatant actor, Combatant target, float valueOverride)
    {
        if (TriggerableEffect == null)
        {
            Debug.Log("Triggerable Effect not set");
        }
        if (actor == null)
        {
            Debug.Log("Actor not set on Triggerable Effect: " + TriggerableEffect.name);
        }
        if (target == null)
        {
            Debug.Log("Target not set on Triggerable Effect: " + TriggerableEffect.name);
        }

        float valueToUse = Value;
        if (useValueOverride)
        {
            valueToUse = valueOverride * valueOverrideMultiplier;
        }
        TriggerableEffect.ApplyEffect(actor, target, valueToUse);
    }

    [field: SerializeField] public TriggerableEffect TriggerableEffect { get; private set; }
    [field: SerializeField] public float Value { get; private set; } = 0;
    //[field: Header("Use External Value? ex: status potency")]
    [field: SerializeField] private bool useValueOverride;
    [field: SerializeField] private float valueOverrideMultiplier = 1;
}


[CreateAssetMenu(fileName = "NewBattleEventTrigger", menuName = "BattleEventTrigger/Standard")]
public class BattleEventTrigger : ScriptableObject
{
    [field: Header("Trigger Type/Conditions/Rate")]
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    [field: SerializeField] public TriggerableEffectType TriggerableEffectType { get; private set; }

    [SerializeField] private List<BattleConditionContainer> triggerConditions = new List<BattleConditionContainer>();
    [SerializeField][Range(0.01f, 1f)] private float triggerRate;
    [SerializeField] private bool onlyTriggerOnHit;

    [field: Header("Targeting")]
    [field: SerializeField] public bool UseTargetOverride { get; private set; }
    [field: SerializeField] public TargetingType TargetOverride { get; private set; }
    [field: SerializeField] public bool PickRandomTarget { get; private set; }

    [field: Header("UI Info")]
    [field: SerializeField] public string EventName { get; private set; }

    [field: Header("Effects to Trigger")]
    [field: SerializeField] public List<TriggerableEffectContainer> TriggerableEffectContainers { get; private set; } = new List<TriggerableEffectContainer>();

    [field: Header("Animations")]
    [field: SerializeField] public List<ActionAnimationData> ActionAnimations { get; private set; } = new List<ActionAnimationData>();

    public bool ConditionsCheck(Combatant combatantA, Combatant combatantB, ActionSummary actionSummary)
    {
        if (actionSummary == null || actionSummary.Action == null)
        {
            Debug.Log("error: action summary/action missing");
            return false;
        }

        if (onlyTriggerOnHit && !actionSummary.Values[ActionSummaryValue.DidHit])
        {
            return false;
        }

        bool conditionsSatisfied = true;
        foreach (BattleConditionContainer battleConditionContainer in triggerConditions)
        {
            //get combatant to check
            Combatant combatantToCheck = combatantA;
            if (battleConditionContainer.BattleConditionCombatant == BattleConditionCombatant.CombatantB)
            {
                combatantToCheck = combatantB;
            }

            if (battleConditionContainer.Conditional == Conditional.If)
            {
                conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
            }
            else if (battleConditionContainer.Conditional == Conditional.And)
            {
                if (!conditionsSatisfied)
                {
                    continue;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
                }
            }
            else if (battleConditionContainer.Conditional == Conditional.Or)
            {
                if (conditionsSatisfied)
                {
                    return true;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
                }
            }
        }
        return conditionsSatisfied;
    }

    public bool TriggerCheck()
    {
        return Roll(triggerRate);
    }

    public bool Roll(float chance)
    {
        float roll = Random.Range(0.01f, 1f);
        if (roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

