using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Status Effects/Remove All Status Effects")]
public class TriggerableEffectRemoveAllStatus : TriggerableEffect
{
    [field: SerializeField] public StatusEffectType StatusEffectType { get; private set; }

     public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        target.RemoveAllStatusEffects(StatusEffectType);
    }
}
