using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect/Stat")]

public class TriggerableBattleEffectHealthStat : TriggerableBattleEffect
{
    [Header("Battle Effect Health Stat")]
    [SerializeField] private bool isHeal;
    [SerializeField] private float power;
    [SerializeField] private StatType offensiveStat;
    [SerializeField] private bool useDefensiveStat;
    [SerializeField] private StatType defensiveStat;
    private BattleFormulas battleFormulas = new BattleFormulas();

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        base.ApplyEffect(actor, target, actionSummary);
        Debug.Log("Triggering Health Effect");

        int actorPower = battleFormulas.GetActorPower(power, actor, offensiveStat);
        int targetDefense = 0;

        if (isHeal)
        {
            int healthEffect = battleFormulas.GetHealthEffect(HealthEffectType.Heal, actorPower, targetDefense);
            target.OnHealed(Mathf.Clamp(healthEffect, 1, 9999), false);
        }
        else
        {
            if (useDefensiveStat)
            {
                targetDefense = battleFormulas.GetTargetDefense(target, defensiveStat, 0);
            }
            Debug.Log("actorPower" + actorPower);
            Debug.Log(targetDefense);
            int healthEffect = battleFormulas.GetHealthEffect(HealthEffectType.Damage, actorPower, targetDefense);
            Debug.Log(healthEffect);
            target.OnDamaged(Mathf.Clamp(healthEffect, 1, 9999), false, ElementalProperty.None);
        }
    }
}
