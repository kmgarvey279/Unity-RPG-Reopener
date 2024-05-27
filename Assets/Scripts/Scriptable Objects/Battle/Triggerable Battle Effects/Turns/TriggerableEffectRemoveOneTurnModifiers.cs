using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Turn Modifiers/Remove One Turn Modifiers")]
public class TriggerableEffectRemoveOneTurnModifiers : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        target.RemoveOneTurnModifiers();
    }
}
