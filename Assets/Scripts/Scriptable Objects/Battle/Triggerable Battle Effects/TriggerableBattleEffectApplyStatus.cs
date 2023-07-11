using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Apply Status Effect")]
public class TriggerableBattleEffectApplyStatus : TriggerableBattleEffect
{ 
    [field: SerializeField] public StatusEffect StatusEffect { get; private set; }
    private BattleFormulas battleFormulas;

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        base.ApplyEffect(actor, target, actionSummary);

        int statusPotency = 0;
        if (StatusEffect.HealthEffectType != HealthEffectType.None)
        {
            int actorPower = battleFormulas.GetActorPower(StatusEffect.Power, actor, StatusEffect.OffensiveStat);
            int targetDefense = 0;
            if (StatusEffect.HealthEffectType == HealthEffectType.Damage)
            {
                targetDefense = battleFormulas.GetTargetDefense(target, StatusEffect.DefensiveStat, 0);
            }
            statusPotency = battleFormulas.GetHealthEffect(StatusEffect.HealthEffectType, actorPower, targetDefense);
        }
        target.AddStatusEffect(StatusEffect, Mathf.Clamp(Mathf.FloorToInt(statusPotency), 1, 9999));
    }
}
