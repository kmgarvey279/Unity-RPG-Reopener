using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Intervention/Points")]
public class TriggerableEffectInterventionPoints: TriggerableEffect
{
    [SerializeField] private SignalSenderInt onChangeInterventionPoints;

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        int intValue = Mathf.Clamp(Mathf.FloorToInt(value), 1, 100);
        onChangeInterventionPoints.Raise(intValue);
    }
}
