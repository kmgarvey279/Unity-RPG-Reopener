using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionModifierType
{
    Damage,
    Healing,
    Defense,
    HitRate,
    CritRate,
    EffectTriggerRate,
    MPCost,
    TimeCost
}

[System.Serializable]
public class ActionModifier
{
    [SerializeField] private float multiplier;
    [SerializeField] private float hitMultiplier;
    [SerializeField] private List<BattleConditionContainer> conditionList = new List<BattleConditionContainer>();
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    [field: SerializeField] public ActionModifierType ActionModifierType { get; private set; }

    public float GetModifier(ActionSubevent actionSubevent)
    {
        foreach (BattleConditionContainer conditionContainer in conditionList)
        {
            if (!conditionContainer.BattleCondition.CheckCondition(actionSubevent))
            {
                return 0;
            }
        }
        float hitBonus = hitMultiplier * actionSubevent.HitTally;
        return multiplier + hitBonus;
    }
}
