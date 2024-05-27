using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffectBarrier", menuName = "Triggerable Effects/Barriers/Stat")]
public class TriggerableEffectBarrier : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        //get power
        int barrierPower = Mathf.Clamp(Mathf.FloorToInt(value), 1, 9999);

        //apply
        target.OnApplyBarrier(barrierPower);
    }
}