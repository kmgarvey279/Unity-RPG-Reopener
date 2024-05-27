using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Status Effects/Apply Status Effect")]
public class TriggerableEffectApplyStatus : TriggerableEffect
{ 
    [field: SerializeField] public StatusEffect StatusEffect { get; private set; }

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        if (StatusEffect == null)
        {
            return;
        }

        int statusPotency = Mathf.Clamp(Mathf.FloorToInt(value), 1, 9999);
        target.AddStatusEffect(StatusEffect, statusPotency);
    }
}
