using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect/Percentage")]

public class TriggerableBattleEffectHealthPercentage : TriggerableBattleEffect
{
    [Header("Battle Effect Health Percentage")]
    [SerializeField] private bool isHeal;
    [SerializeField, Range(0.01f, 1f)] private float healthPercentage;
    [SerializeField] private bool useCurrentHP = false;
    private float bossDivider = 16f;

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        base.ApplyEffect(actor, target, actionSummary);
        Debug.Log("Triggering Health Effect");
        float healthEffect = 0;

        float percentageTemp = healthPercentage;
        if (target is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)target;
            if (enemyCombatant.IsBoss)
            {
                Mathf.Round(percentageTemp /= bossDivider);
            }
        }
        float healthTotalToUse = target.HP.GetValue();
        if (useCurrentHP)
        {
            healthTotalToUse = target.HP.CurrentValue;
        }
        healthEffect = healthTotalToUse * percentageTemp;


        //final value
        int finalHealthEffect = Mathf.Clamp(Mathf.FloorToInt(healthEffect), 1, 9999);
        if (isHeal)
        {
            Debug.Log("Heal: " + finalHealthEffect);
            target.OnHealed(finalHealthEffect, false);
        }
        else
        {
            Debug.Log("Damage: " + finalHealthEffect);
            target.OnDamaged(finalHealthEffect, false, ElementalProperty.None);
        }
    }
}