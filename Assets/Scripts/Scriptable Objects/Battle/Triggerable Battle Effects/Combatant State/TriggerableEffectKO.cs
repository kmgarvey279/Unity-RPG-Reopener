using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffectBarrier", menuName = "Triggerable Effects/Health Effect/KO")]
public class TriggerableEffectKO : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        target.OnOneShot();
    }
}
