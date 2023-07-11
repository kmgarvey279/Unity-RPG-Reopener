using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Modify Status Stacks")]
public class TriggerableBattleEffectModifyStacks : TriggerableBattleEffect
{
    [SerializeField] private StatusEffect statusEffectToModify;
    [SerializeField] private int amount;
    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        base.ApplyEffect(actor, target, actionSummary);

        if (statusEffectToModify != null && statusEffectToModify.StatusCounterType == StatusCounterType.Stacks)
        {
            target.ModifyStatusStacks(statusEffectToModify, amount);
        }
    }
}
