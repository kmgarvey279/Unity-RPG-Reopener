using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Revive")]
public class TriggerableBattleEffectRevive : TriggerableBattleEffect
{
    [field: SerializeField] public float percentOfHPToRestore;

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        base.ApplyEffect(actor, target, actionSummary);
        target.OnRevive(percentOfHPToRestore);
    }
}
