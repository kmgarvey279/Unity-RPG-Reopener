using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffectBarrier", menuName = "Triggerable Effects/Barriers/Percentage")]
public class TriggerableEffectBarrierPercentage : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        //clamp % value
        value = Mathf.Clamp01(value);

        int barrierPower = Mathf.CeilToInt(value * target.MaxHP);
        target.OnApplyBarrier(Mathf.Clamp(barrierPower, 1, 9999));
    }
}
