using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect/Percentage")]

public class TriggerableEffectHealthPercentage : TriggerableEffect
{
    [Header("Battle Effect Health Percentage")]
    [SerializeField] private bool isHeal;
    private float bossDivider = 16f;

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        float healthEffect = 0;
        float percentageTemp = Mathf.Clamp01(value);

        //if target is a boss
        if (target is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)target;
            if (enemyCombatant.IsBoss)
            {
                Mathf.Round(percentageTemp /= bossDivider);
            }        
        }
        float healthTotalToUse = target.MaxHP;
        
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
            target.OnAttacked(finalHealthEffect, false, false);
        }
    }
}