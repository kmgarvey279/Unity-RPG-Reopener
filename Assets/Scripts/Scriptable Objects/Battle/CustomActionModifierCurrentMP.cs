using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Custom Action Modifier", menuName = "Action/Custom Action Modifier/Current MP")]
public class CustomActionModifierCurrentMP : CustomActionModifier
{
    [Header("MP")]
    [SerializeField] private float damageMultiplierMax;
    public override float ApplyModifier(float baseValue, Combatant combatantA, Combatant combatantB, ActionSummary actionSummary)
    {
        //get combatant to check
        Combatant combatantToCheck = combatantA;
        if (battleConditionCombatant == BattleConditionCombatant.CombatantB)
        {
            combatantToCheck = combatantB;
        }

        float percentMPConsumed = (float)combatantToCheck.MP / (float)combatantToCheck.MaxMP;
        Debug.Log("custom mp modifier %:" + percentMPConsumed);
        float powerMultiplier = 1f + (damageMultiplierMax * percentMPConsumed);
        Debug.Log("custom mp modifier power multiplier:" + powerMultiplier);

        baseValue *= powerMultiplier;
        return baseValue;
    }
}
