using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Remove Status Effect")]
public class TriggerableBattleEffectRemoveStatus : TriggerableBattleEffect
{
    [field: SerializeField] public StatusEffectType StatusEffectType { get; private set; }
    [field: SerializeField] public List<StatusEffect> StatusEffectsToRemove { get; private set; } = new List<StatusEffect>();
    [field: SerializeField] public bool RemoveAll { get; private set; } = false;

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        base.ApplyEffect(actor, target, actionSummary);

        if(RemoveAll) 
        {
            target.RemoveAllStatusEffects(StatusEffectType);
        }
        else
        {
            foreach (StatusEffect statusToRemove in StatusEffectsToRemove)
            {
                target.RemoveStatusEffect(statusToRemove);
            }
        }
    }
}
