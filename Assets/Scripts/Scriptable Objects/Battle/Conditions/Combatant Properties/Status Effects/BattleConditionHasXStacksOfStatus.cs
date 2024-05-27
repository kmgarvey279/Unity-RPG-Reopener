using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueComparisonType
{
    Equals,
    LessThan,
    LessThanOrEqualTo,
    GreaterThan,
    GreaterThanOrEqualTo
}

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Status Effects/Has X Stacks")]
public class BattleConditionHasXStacksOfStatus : BattleCondition
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private ValueComparisonType valueComparisonType;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        int intValue = Mathf.FloorToInt(value);

        foreach (StatusEffectInstance statusEffectInstance in combatantToCheck.StatusEffectInstances)
        {
            if (statusEffectInstance.StatusEffect == statusEffect && CheckStacks(intValue, statusEffectInstance.Stacks.CurrentValue))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckStacks(int valueA, int valueB)
    {
        switch (valueComparisonType)
        {
            case ValueComparisonType.Equals:
                return valueA == valueB;
            case ValueComparisonType.LessThan:
                return valueA < valueB;
            case ValueComparisonType.LessThanOrEqualTo:
                return valueA <= valueB;
            case ValueComparisonType.GreaterThan:
                return valueA > valueB;
            case ValueComparisonType.GreaterThanOrEqualTo:
                return valueA >= valueB;
            default:
                return false;
        }
    }
}
