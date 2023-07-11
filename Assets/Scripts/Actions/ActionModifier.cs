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
    TimeCost,
    CritPower,
    BlockPower,
    BasePower
}

[System.Serializable]
public class ActionModifier
{
    [SerializeField] private float multiplier;
    [SerializeField] private List<BattleConditionContainer> conditionList = new List<BattleConditionContainer>();
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    [field: SerializeField] public ActionModifierType ActionModifierType { get; private set; }

    public float GetModifier(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        foreach (BattleConditionContainer conditionContainer in conditionList)
        {
            if (!conditionContainer.BattleCondition.CheckCondition(actor, target, actionSummary))
            {
                return 0;
            }
        }
        return multiplier;
    }
}
