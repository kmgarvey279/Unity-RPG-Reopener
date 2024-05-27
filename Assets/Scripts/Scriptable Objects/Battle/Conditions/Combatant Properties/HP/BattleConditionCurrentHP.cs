using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Combatant/CurrentHP")]
public class BattleConditionCurrentHP : BattleCondition
{
    [SerializeField] private ValueComparisonType valueComparisonType;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        value = Mathf.Clamp(value, 0, 1);
        if (combatantToCheck != null)
        {
            float hpPercent = (float)combatantToCheck.HP / (float)combatantToCheck.MaxHP;
            Debug.Log("Checking HP. Current Value: " + combatantToCheck.HP + " Max value: " + combatantToCheck.MaxHP);
            Debug.Log("Checking HP. combatant percent: " + hpPercent + " desired value: " + valueComparisonType.ToString() + " " + value);
            if (CheckValue(hpPercent, value))
            {
                Debug.Log("Checking HP Result: true");
                return true;
            }
        }
        Debug.Log("Checking HP Result: false");
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
