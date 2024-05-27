using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Revive")]
public class TriggerableEffectRevive : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        value = Mathf.Clamp01(value);
        target.OnRevive(value);
    }
}
