using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFormulas
{
    private float attackMultiplierConst = 8f;
    private float healMultiplierConst = 5f;
    private float defenseMultiplierConst = 5f;
    private float varianceMinAttack = 0.95f;
    private float varianceMaxAttack = 1.05f;
    private float varianceMinHeal = 0.985f;
    private float varianceMaxHeal = 1.015f;

    public float GetHitRate(Action action, Combatant target)
    {
        float hitRate = 0;
        hitRate = action.HitRate - target.SecondaryStats[SecondaryStatType.EvadeRate].CurrentValue;
        return Mathf.Clamp(hitRate, 0.05f, 1f);
    }

    public float GetCritRate(Action action, Combatant actor)
    {
        float critRate = 0;
        critRate = actor.SecondaryStats[SecondaryStatType.CritRate].CurrentValue * action.CritRateMultiplier;
        return Mathf.Clamp(critRate, 0, 1f);
    }

    public int GetActorPower(float actionPower, Combatant actor, StatType offensiveStat)
    {
        //get relevent stat
        float stat = 0;
        //get multiplier const
        float multiplierConst = attackMultiplierConst;
        if (offensiveStat == StatType.Healing)
        {
            multiplierConst = healMultiplierConst;
        }
        //get actor stat
        if (actor.Stats.ContainsKey(offensiveStat))
        {
            stat = actor.Stats[offensiveStat].CurrentValue;
        }
        float actorPower = actionPower * stat * multiplierConst;
        return Mathf.FloorToInt(actorPower);
    }

    public int GetTargetDefense(Combatant target, StatType defensiveStat, float pierce)
    {
        float defensivePower = target.Stats[defensiveStat].CurrentValue * defenseMultiplierConst;
        defensivePower -= defensivePower * pierce;
        return Mathf.FloorToInt(defensivePower);
    }

    public int GetTargetBlock(Combatant target, StatType defensiveStat, float pierce)
    {
        float blockPower = target.Stats[defensiveStat].CurrentValue * defenseMultiplierConst * target.SecondaryStats[SecondaryStatType.BlockPower].CurrentValue;
        blockPower -= blockPower * pierce;
        return Mathf.FloorToInt(blockPower);
    }

    public int GetHealthEffect(HealthEffectType healthEffectType, int actorPower, int targetDefense)
    {
        if (healthEffectType == HealthEffectType.None)
        {
            return 0;
        }
        //rng variance
        float varianceMin = varianceMinAttack;
        float varianceMax = varianceMaxAttack;
        if (healthEffectType == HealthEffectType.Heal)
        {
            varianceMin = varianceMinHeal;
            varianceMax = varianceMaxHeal;
        }
        float healthEffect = actorPower * (100f / (100f + targetDefense));
        Debug.Log(healthEffect);
        healthEffect *= Random.Range(varianceMin, varianceMax);
        return Mathf.RoundToInt(healthEffect);
    }
}
