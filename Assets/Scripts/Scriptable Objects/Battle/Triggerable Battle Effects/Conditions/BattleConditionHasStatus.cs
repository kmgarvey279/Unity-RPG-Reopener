using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Triggerable Effects/Conditions/Has Status")]
public class BattleConditionHasStatus : BattleCondition
{
    [SerializeField] private BattleEventType BattleEventType;
    [SerializeField] private StatusEffect statusEffect;
    public override bool CheckCondition(ActionSubevent actionSubevent)
    {
        foreach (StatusEffectInstance statusEffectInstance in actionSubevent.Target.StatusEffectInstances)
        {
            if(statusEffectInstance.StatusEffect == statusEffect)
            {
                return true;
            }
        }
        return false;
    }
}
