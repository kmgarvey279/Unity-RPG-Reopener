using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect")]

public class TriggerableBattleEffectHealth : TriggerableBattleEffect
{
    [Header("Battle Effect Damage")]
    [SerializeField] private bool isHeal;
    [SerializeField] private HealthEffectType healthEffectType;
    [SerializeField] private float power = 10f;
    [Header("Stat Based Damage Properties")]
    [SerializeField] private StatType offensiveStat;
    private float varianceMinDamage = 0.95f;
    private float varianceMaxDamage = 1.05f;
    private float varianceMinHeal = 0.95f;
    private float varianceMaxHeal = 1.05f;
    [Header("Percentage Damage Properties")]
    [SerializeField] private bool useCurrentHP = false;
    private float bossDivider = 16f;

    public override void ApplyEffect(Combatant actor, Combatant target, float potencyOverride = 0)
    {
        base.ApplyEffect(actor, target, potencyOverride);
        Debug.Log("Triggering Health Effect");
        float healthEffect = 0;
        float varianceMin = varianceMinDamage;
        if(isHeal)
        {
            varianceMin = varianceMinHeal;
        }
        float varianceMax = varianceMaxDamage;
        if (isHeal)
        {
            varianceMax = varianceMaxHeal;
        }

        switch (healthEffectType)
        {
            case HealthEffectType.Stat:
                //get action power
                float actionPower = power;
                float stat = actor.Stats[offensiveStat].GetValue();
                healthEffect = actionPower * stat;

                //rng variance
                healthEffect *= Random.Range(varianceMin, varianceMax);
                break;

            case HealthEffectType.Percentage:
                float percentageTemp = power;
                if(target is EnemyCombatant)
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
                break;

            case HealthEffectType.Fixed:
                if (potencyOverride > 0)
                {
                    healthEffect = potencyOverride;
                }
                else
                {
                    healthEffect = power;
                }
                break;

            default:
                break;
        }

        //final value
        healthEffect = Mathf.Clamp(Mathf.Round(healthEffect), 1, 9999);
        if(isHeal)
        {
            Debug.Log("Heal: " + healthEffect);
            target.OnHealed(Mathf.FloorToInt(healthEffect), false);
        }
        else
        {
            Debug.Log("Damage: " + healthEffect);
            target.OnDamaged(Mathf.FloorToInt(healthEffect), false, ElementalProperty.None);
        }
    }
}
