using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Turn Modifier Effect")]
public class TriggerableBattleEffectTurnModifier : TriggerableBattleEffect
{
    [field: SerializeField, Range(-1f, 1f)] public float TurnModifier { get; private set; }

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        base.ApplyEffect(actor, target, actionSummary);
        target.ApplyTurnModifier(TurnModifier);
    }
}
