using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Apply Status Effect")]
public class TriggerableBattleEffectApplyStatus : TriggerableBattleEffect
{

    [SerializeField] private float power = 0f;
    [SerializeField] private bool useActorStat;
    [SerializeField] private StatType offensiveStat;
    [field: SerializeField] public StatusEffect StatusEffect { get; private set; }
    public override void ApplyEffect(Combatant actor, Combatant target, float potencyOverride = 0)
    {
        base.ApplyEffect(actor, target, potencyOverride);

        float statusPotency = power;
        if (useActorStat)
        {
            statusPotency *= actor.Stats[offensiveStat].GetValue();
        }
        target.AddStatusEffect(StatusEffect, statusPotency);
    }
}
