using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Intervention/Cancel All")]
public class TriggerableEffectCancelInterventions : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        base.ApplyEffect(actor, target, value);

        if (target is PlayableCombatant)
        {
            //PlayableCombatant playableCombatant = (PlayableCombatant)target;
            //playableCombatant.CancelAllInterventions();
        }
    }
}
