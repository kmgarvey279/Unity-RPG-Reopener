using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect/Fixed")]

public class TriggerableBattleEffectHealthFixed : TriggerableBattleEffect
{
    [Header("Battle Effect Health Fixed")]
    [SerializeField] private int healthEffect;

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary)
    {
        base.ApplyEffect(actor, target, actionSummary);

        if (healthEffect > 0)
        {
            target.OnHealed(healthEffect, false);
        }
        else
        {
            target.OnDamaged(healthEffect, false, ElementalProperty.None);
        }
    }
}