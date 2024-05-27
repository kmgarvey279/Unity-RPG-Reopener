using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Status Effects/Modify Status Duration/All")]
public class TriggerableEffectModifyStatusDurationAll : TriggerableEffect
{
    [SerializeField] private StatusEffectType statusEffectType;
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        int intValue = Mathf.FloorToInt(value);
        target.ModifyAllStatusDurations(statusEffectType, intValue);
    }
}
