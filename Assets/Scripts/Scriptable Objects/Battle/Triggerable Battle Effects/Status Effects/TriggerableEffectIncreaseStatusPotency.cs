using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Status Effects/Increase Potency")]
public class TriggerableEffectIncreaseStatusPotency : TriggerableEffect
{
    [field: SerializeField] private StatusEffect statusEffect;

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        if (statusEffect == null)
        {
            return;
        }

        int valueToUse = Mathf.Abs(Mathf.FloorToInt(value));
        Debug.Log("adding " + valueToUse.ToString() + " potency to " + statusEffect.EffectName);
        target.IncreaseStatusPotency(statusEffect, valueToUse);
    }
}
