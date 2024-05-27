using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Turn Modifiers/Apply Modifier")]
public class TriggerableEffectTurnModifier : TriggerableEffect
{
    [SerializeField] private bool applyToNextTurnOnly;
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        value = Mathf.Clamp(value, -1f, 1f);
        target.ApplyTurnModifier(value, applyToNextTurnOnly);
    }
}
