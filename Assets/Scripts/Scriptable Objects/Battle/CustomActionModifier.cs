using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomActionModifier : ScriptableObject
{
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    [field: SerializeField] public ActionModifierType ActionModifierType { get; private set; }
    [SerializeField] protected BattleConditionCombatant battleConditionCombatant;

    public virtual float ApplyModifier(float baseValue, Combatant combatantA, Combatant combatantB, ActionSummary actionSummary)
    {
        return baseValue;
    }
}
