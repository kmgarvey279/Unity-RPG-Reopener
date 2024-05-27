using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Status Effects/Remove Status Effect")]
public class TriggerableEffectRemoveStatus : TriggerableEffect
{
    [field: SerializeField] public StatusEffect StatusEffectToRemove { get; private set; } 

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        target.RemoveStatusEffect(StatusEffectToRemove);
    }
}
