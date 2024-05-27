using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleConditionCombatant
{
    CombatantA,
    CombatantB
}

public enum Conditional
{
    If,
    And, 
    Or
}

[System.Serializable]
public class BattleConditionContainer
{
    [field: SerializeField] public Conditional Conditional { get; protected set; }
    //[field: SerializeField] public BattleEventType BattleEventType { get; protected set; }
    [field: SerializeField] public BattleConditionCombatant BattleConditionCombatant { get; protected set; }
    [field: SerializeField] public bool isNot;
    [field: SerializeField] private BattleCondition battleCondition;
    [field: SerializeField] private float value = 0;

    public bool CheckCondition(Combatant combatantToCheck, ActionSummary actionSummary = null)
    {
        if (battleCondition == null)
        {
            return false;
        }
        bool conditionMet = battleCondition.CheckCondition(combatantToCheck, value, actionSummary);
        if (isNot)
            conditionMet = !conditionMet;
        return conditionMet;
    }
}
