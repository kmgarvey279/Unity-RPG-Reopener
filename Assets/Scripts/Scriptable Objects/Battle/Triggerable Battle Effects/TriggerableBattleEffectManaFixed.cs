using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Mana Effect/Fixed")]
public class TriggerableBattleEffectManaFixed : TriggerableBattleEffect
{
    [SerializeField] private bool useOverride;
    [SerializeField] private int manaEffect;

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        base.ApplyEffect(actor, target, actionSummary);
        if (target.CombatantType != CombatantType.Player)
        {
            return;
        }
        PlayableCombatant playableCombatant = target as PlayableCombatant;
        playableCombatant.OnChangeMana(manaEffect);
    }
}
