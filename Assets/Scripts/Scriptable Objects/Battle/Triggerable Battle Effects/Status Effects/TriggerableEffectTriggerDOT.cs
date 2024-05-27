using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Status Effects/Trigger DOT")]
public class TriggerableEffectTriggerDOT : TriggerableEffect
{
    [SerializeField] private StatusEffect statusEffect;

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        if (statusEffect == null)
        {
            return;
        }

        int healthEffect = 0;
        StatusEffectInstance statusEffectInstance = target.GetStatusEffectInstance(statusEffect);
        if (statusEffectInstance != null)
        {
            healthEffect = Mathf.CeilToInt(statusEffectInstance.Potency * value);
        }

        if (statusEffect.StatusEffectType == StatusEffectType.Buff)
        {
            target.OnHealed(healthEffect);
        }
        else if (statusEffect.StatusEffectType == StatusEffectType.Debuff)
        {
            target.OnDamaged(healthEffect, false, false);
        }
    }
}
