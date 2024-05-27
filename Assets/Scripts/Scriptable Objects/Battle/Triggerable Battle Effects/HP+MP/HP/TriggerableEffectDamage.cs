using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect/Damage")]

public class TriggerableEffectDamage : TriggerableEffect
{
    [Header("Battle Effect Health Damage")]
    [SerializeField] private ElementalProperty elementalProperty;
    private const float powerMultiplierMin = 0.1f;
    private const float powerMultiplierMax = 10f;

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        Debug.Log("Applying Health Effect (Damage)");
        //clamp range of power multipliers
        value = Mathf.Clamp(value, powerMultiplierMin, powerMultiplierMax);

        //get stat
        IntStatType actorStat = IntStatType.MAttack;
        IntStatType targetStat = IntStatType.MDefense;
        if (elementalProperty == ElementalProperty.Slash
            || elementalProperty == ElementalProperty.Strike
            || elementalProperty == ElementalProperty.Pierce)
        {
            actorStat = IntStatType.Attack;
            targetStat = IntStatType.Defense;
        }
        //dark always hits the weaker defense stat
        else if (elementalProperty == ElementalProperty.Dark)
        {
            targetStat = IntStatType.Defense;
            if (target.Stats[IntStatType.Defense] > target.Stats[IntStatType.MDefense])
            {
                targetStat = IntStatType.MDefense;
            }
        }

        //get base power
        float damage = value * actor.Stats[actorStat] * BattleConsts.AttackMultiplierConst;

        //apply defense
        float targetDefense = target.Stats[targetStat] * BattleConsts.DefenseMultiplierConst;
        damage *= (BattleConsts.DefenseApplicationConst / (BattleConsts.DefenseApplicationConst + targetDefense));
        Debug.Log("Base Health Effect: -" + damage);

        //universal modifiers
        foreach (float modifier in actor.UniversalModifiers[BattleEventType.Acting][UniversalModifierType.Damage])
        {
            damage *= modifier;
        }
        foreach (float modifier in target.UniversalModifiers[BattleEventType.Targeted][UniversalModifierType.Damage])
        {
            damage *= modifier;
        }
        Debug.Log("Health Effect After Universal Mods: -" + damage);

        bool isVulnerable = false;
        //vulnerable bonus
        if (target is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)target;
            if (enemyCombatant && enemyCombatant.Vulnerabilities.Contains(elementalProperty))
            {
                isVulnerable = true;
            }
            damage = enemyCombatant.ApplyVulnerabiltyMultiplier(damage, isVulnerable);
            Debug.Log("Health Effect After Vulneability Check: -" + damage);
        }

        //rng variance
        damage *= Random.Range(BattleConsts.VarianceMinDamage, BattleConsts.VarianceMaxDamage);
        Debug.Log("Health Effect After RNG: -" + damage);

        //final damage
        int intDamage = Mathf.Clamp(Mathf.CeilToInt(damage), 1, 9999);

        //barrier
        if (target.Barrier > 0)
        {
            intDamage = target.OnBarrierAbsorbDamage(intDamage);
        }
        Debug.Log("Health Effect After Barrier: -" + intDamage);

        intDamage = Mathf.Clamp(intDamage, 1, 9999);

        //final damage
        Debug.Log("Final Health Effect: -" + intDamage);
        target.OnAttacked(intDamage, false, isVulnerable);
    }
}
