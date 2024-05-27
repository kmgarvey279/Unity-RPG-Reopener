using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Status Effects/Modify Status Stacks")]
public class TriggerableEffectModifyStacks : TriggerableEffect
{
    [SerializeField] private StatusEffect statusEffectToModify;

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        int intValue = Mathf.FloorToInt(value);
        if (statusEffectToModify != null)
        {
            target.ModifyStatusStacks(statusEffectToModify, intValue);
        }
    }
}
