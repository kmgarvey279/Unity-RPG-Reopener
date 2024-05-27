using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionModifierType
{
    Damage,
    Healing,
    HitRate,
    CritRate,
    EffectTriggerRate,
    MPCost,
    TargetWeight,
    Break
}

public enum ValueModifierType
{
    Addend,
    Multiplier
}

//only checked during actions, won't effect other events like turn regens/dots
[System.Serializable]
public class ActionModifier
{
    [field: Header("Event Type")]
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    [field: Header("Modifier")]
    [field: SerializeField] public ActionModifierType ActionModifierType { get; private set; }
    [SerializeField] private ValueModifierType valueModifierType;
    [SerializeField] private float modifierValue;
    [field: Header("Conditions")]
    [SerializeField] private List<BattleConditionContainer> conditionList = new List<BattleConditionContainer>();

    public float ApplyModifier(float baseValue, Combatant combatantA, Combatant combatantB, ActionSummary actionSummary)
    {
        if (!ConditionsCheck(combatantA, combatantB, actionSummary))
        {
            return baseValue;
        }
        if (valueModifierType == ValueModifierType.Addend)
        {
            baseValue += modifierValue;
        }
        else
        {
            baseValue *= modifierValue;
        }
        return baseValue;
    }

    public bool ConditionsCheck(Combatant combatantA, Combatant combatantB, ActionSummary actionSummary)
    {
        bool conditionsSatisfied = true;
        foreach (BattleConditionContainer battleConditionContainer in conditionList)
        {
            //get combatant to check
            Combatant combatantToCheck = combatantA;
            //if (battleConditionContainer.BattleEventType == BattleEventType.Targeted)
            //{
            if (battleConditionContainer.BattleConditionCombatant == BattleConditionCombatant.CombatantB)
            {
                    combatantToCheck = combatantB;
            }

            //check conditionals
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
}
