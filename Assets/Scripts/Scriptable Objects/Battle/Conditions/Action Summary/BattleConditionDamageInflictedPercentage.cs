using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Action Summary/Damage Inflicted/Percentage")]
public class BattleConditionDamageInflictedPercentage : BattleCondition
{
    [SerializeField] private ValueComparisonType valueComparisonType;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        value = Mathf.Clamp(value, 0, 1);
        if (actionSummary != null)
        {
            float hpPercent = (float)actionSummary.CumHealthEffect / combatantToCheck.MaxHP;
            if (CheckValue(hpPercent, value))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckValue(float valueA, float valueB)
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
