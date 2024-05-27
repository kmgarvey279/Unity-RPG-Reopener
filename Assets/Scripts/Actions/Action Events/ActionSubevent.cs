
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum ActionSummaryValue
{
    DidHit,
    DidMiss,
    DidCrit,
    DidGuard,
    DidHitWeakness,
    IsIntervention
}
 
public class ActionSummary
{
    public Action Action { get; private set; }
    public int CumHealthEffect { get; private set; } = 0;
    public int Hits { get; private set; } = 0;
    public Dictionary<ActionSummaryValue, bool> Values = new Dictionary<ActionSummaryValue, bool>();

    public ActionSummary(Action action, bool isIntervention)
    {
        Action = action;
        foreach (ActionSummaryValue actionSummaryValue in System.Enum.GetValues(typeof(ActionSummaryValue)))
        {
            Values.Add(actionSummaryValue, false);
        }
        if (isIntervention)
        {
            SetValueAsTrue(ActionSummaryValue.IsIntervention);
        }
    }

    public void UpdateCumHealthEffect(int change)
    {
        CumHealthEffect += change;
    }

    public void SetValueAsTrue(ActionSummaryValue actionSummaryValue)
    {
        Values[actionSummaryValue] = true;
    }

    public void OnHit()
    {
        Hits++;
        if (!Values[ActionSummaryValue.DidHit])
        {
            SetValueAsTrue(ActionSummaryValue.DidHit);
        }

        Debug.Log("Hits: " + Hits);
    }
}

[System.Serializable]
public class ActionSubevent
{
    [field: SerializeField] public ActionSummary ActionSummary { get; private set; }
    [field: SerializeField] public Combatant Actor { get; private set; }
    [field: SerializeField] public Combatant Target { get; private set; }
    private List<float> preemptiveEventDamageMultipliers = new List<float>();

    public ActionSubevent(Action _action, Combatant _actor, Combatant _target, bool _isIntervention)
    {
        ActionSummary = new ActionSummary(_action, _isIntervention);
        Actor = _actor;
        Target = _target;
    }

    public void ApplyPreemptiveEventMultiplier(float newMultiplier)
    {
        preemptiveEventDamageMultipliers.Add(newMultiplier);
    }

    public void Execute()
    {
        if (ActionSummary.Action is Attack)
        {
            ApplyAttack();
        }
        else if (ActionSummary.Action is Heal)
        {
            ApplyHeal();
        }
    }

    private bool HitCheck()
    {
        int hitRate = GetHitRate();
        hitRate = Mathf.FloorToInt(ApplyActionModifiers(hitRate, ActionModifierType.HitRate));
        Debug.Log("Hit Rate after mods: " + hitRate);
        
        return Roll(hitRate);
    }

    private bool CritCheck()
    {
        if ((ActionSummary.Action is Attack && Actor.CheckBool(CombatantBool.AttacksAlwaysCrit))
            || (ActionSummary.Action is Heal && Actor.CheckBool(CombatantBool.HealsAlwaysCrit)))
        {
            return true;
        }
        else
        {
            //get base crit rate
            int critRate = GetCritRate();
            
            //apply modifiers
            critRate = Mathf.FloorToInt(ApplyActionModifiers(critRate, ActionModifierType.CritRate));
            return Roll(critRate);
        }
    }

    public void ApplyHeal()
    {
        Heal heal = (Heal)ActionSummary.Action;
        if (heal == null)
        {
            return;
        }
        ActionSummary.OnHit();

        //get base power
        float healAmount = heal.Power * Actor.Stats[IntStatType.Healing] * BattleConsts.HealingMultiplierConst;
        Debug.Log("Base heal power: " + healAmount);

        //if crit
        if (CritCheck())
        {
            //apply crit multiplier
            healAmount *= BattleConsts.BaseCritMultiplierConst;
            ActionSummary.SetValueAsTrue(ActionSummaryValue.DidCrit);

            Debug.Log("Crit! Heal power: " + healAmount);
        }

        //action modifiers
        healAmount = ApplyActionModifiers(healAmount, ActionModifierType.Healing);
        Debug.Log("Heal after all mods: " + healAmount);

        //universal modifiers
        foreach (float modifier in Actor.UniversalModifiers[BattleEventType.Acting][UniversalModifierType.Healing])
        {
            healAmount *= modifier;
        }
        Debug.Log("Heal after actor's universal mods: " + healAmount);
        foreach (float modifier in Target.UniversalModifiers[BattleEventType.Targeted][UniversalModifierType.Healing])
        {
            healAmount *= modifier;
        }
        Debug.Log("Heal after target's universal mods: " + healAmount);

        //rng variance
        healAmount *= Random.Range(BattleConsts.VarianceMinHeal, BattleConsts.VarianceMaxHeal);
        Debug.Log("Heal after rng: " + healAmount);

        //final heal
        int intHeal = Mathf.Clamp(Mathf.CeilToInt(healAmount), 1, 9999);
        Debug.Log("int heal value: " + healAmount);

        ActionSummary.UpdateCumHealthEffect(intHeal);
        Target.OnHealed(intHeal, ActionSummary.Values[ActionSummaryValue.DidCrit]);
    }

    public void ApplyAttack()
    {
        Attack attack = (Attack)ActionSummary.Action;
        if (attack == null)
        {
            return;
        }

        //if hit
        if (HitCheck())
        {
            ActionSummary.OnHit();
            float damage = 0;

            //get relevent stat
            IntStatType actorStat = IntStatType.MAttack;
            IntStatType targetStat = IntStatType.MDefense;
            //switch to physical stats?
            if (attack.ElementalProperty == ElementalProperty.Slash
                || attack.ElementalProperty == ElementalProperty.Strike
                || attack.ElementalProperty == ElementalProperty.Pierce)
            {
                actorStat = IntStatType.Attack;
                targetStat = IntStatType.Defense;
            }
            //dark always hits the weaker defense stat
            if (attack.ElementalProperty == ElementalProperty.Dark)
            {
                targetStat = IntStatType.Defense;
                if (Target.Stats[IntStatType.Defense] > Target.Stats[IntStatType.MDefense])
                {
                    targetStat = IntStatType.MDefense;
                }
            }

            //get base power
            damage = Actor.Stats[actorStat] * attack.Power * BattleConsts.AttackMultiplierConst;

            //apply defense
            float targetDefense = Target.Stats[targetStat] * BattleConsts.DefenseMultiplierConst;
            damage *= (BattleConsts.DefenseApplicationConst / (BattleConsts.DefenseApplicationConst + targetDefense));
            Debug.Log("Damage After Defense: " + damage);

            //crit (multiplicative)
            bool thisHitCrit = false;
            if (CritCheck())
            {
                thisHitCrit = true;
                ActionSummary.SetValueAsTrue(ActionSummaryValue.DidCrit);
                //apply crit multiplier
                damage *= BattleConsts.BaseCritMultiplierConst;
                Debug.Log("Crit! New Damage: " + damage);
            }

            //preemptive event multipliers (multiplicative)
            foreach (float modifier in preemptiveEventDamageMultipliers)
            {
                damage *= modifier;
            }
            Debug.Log("Damage After Preemptive Mods: " + damage);

            //action modifiers (multiplicative)
            damage = ApplyActionModifiers(damage, ActionModifierType.Damage);
            Debug.Log("Damage After All Action Mods: " + damage);

            //universal modifiers (multiplicative)
            foreach (float modifier in Actor.UniversalModifiers[BattleEventType.Acting][UniversalModifierType.Damage])
            {
                //Debug.Log("Actor Universal Multiplier: " + modifier);
                damage += modifier * damage;
            }
            foreach (float modifier in Target.UniversalModifiers[BattleEventType.Targeted][UniversalModifierType.Damage])
            {
                //Debug.Log("Target Universal Multiplier: " + modifier);
                damage += modifier * damage;
            }
            Debug.Log("Damage After Universal Mods: " + damage);

            //vulnerable/open bonus (enemies) (multiplicative)
            if (Target is EnemyCombatant)
            {
                EnemyCombatant enemyCombatant = (EnemyCombatant)Target;
                if (enemyCombatant && enemyCombatant.Vulnerabilities.Contains(attack.ElementalProperty))
                {
                    ActionSummary.SetValueAsTrue(ActionSummaryValue.DidHitWeakness);
                }
                damage = enemyCombatant.ApplyVulnerabiltyMultiplier(damage, ActionSummary.Values[ActionSummaryValue.DidHitWeakness]);
                Debug.Log("Damage After Vulneability Check: " + damage);
            }

            //rng variance (multiplicative)
            damage *= Random.Range(BattleConsts.VarianceMinDamage, BattleConsts.VarianceMaxDamage);
            Debug.Log("Damage After RNG: " + damage);

            //final damage
            int intDamage = Mathf.Clamp(Mathf.CeilToInt(damage), 1, 9999);

            //barrier (subtractive)
            if (Target.Barrier > 0)
            {
                intDamage = Target.OnBarrierAbsorbDamage(intDamage);
            }
            //Debug.Log("Damage After Barrier: " + intDamage);

            //breach (additive)
            //intDamage += Target.Breach;
            //Debug.Log("Damage After Breach: " + intDamage);

            //final damage
            intDamage = Mathf.Clamp(intDamage, 0, 9999);
            Debug.Log("Final Damage: " + intDamage);

            ActionSummary.UpdateCumHealthEffect(intDamage);
            if (intDamage > 0)
            {
                Target.OnAttacked(intDamage, thisHitCrit, ActionSummary.Values[ActionSummaryValue.DidHitWeakness]);
            }
        }
        //miss
        else
        {
            ActionSummary.SetValueAsTrue(ActionSummaryValue.DidMiss);
            Target.OnEvade();
        }
    }

    public int GetHitRate()
    {
        if (ActionSummary.Action is Attack)
        {
            Attack attack = (Attack)ActionSummary.Action;
            int hitRate = 0;

            //base accuracy - evade stat
            hitRate = attack.HitRate - Target.Stats[IntStatType.EvadeRate];
            Debug.Log("action accuracy: " + attack.HitRate);
            Debug.Log("target evasion: " + Target.Stats[IntStatType.EvadeRate]);
            Debug.Log("hit rate: " + hitRate);

            //return final value
            return Mathf.Clamp(hitRate, 1, 100);
        }
        return 100;
    }

    public int GetCritRate()
    {
        int critRate = 0;
        if (ActionSummary.Action is Attack)
        {
            Attack attack = (Attack)ActionSummary.Action;

            //get base crit rate
            critRate = Mathf.CeilToInt(Actor.Stats[IntStatType.CritRate] * attack.CritRateMultiplier);
        }
        else if (ActionSummary.Action is Heal)
        {
            //get base crit rate
            critRate = Mathf.CeilToInt(Actor.Stats[IntStatType.CritRate]);
        }
        return Mathf.Clamp(critRate, 1, 100);
    }

    private float ApplyActionModifiers(float baseValue, ActionModifierType actionModifierType)
    {
        //action
        foreach (ActionModifier actionModifier in ActionSummary.Action.ActionModifierDict[actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, Actor, Target, ActionSummary);
        }
        //action (custom)
        foreach (CustomActionModifier actionModifier in ActionSummary.Action.CustomActionModifierDict[actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, Actor, Target, ActionSummary);
        }
        //actor
        foreach (ActionModifier actionModifier in Actor.ActionModifiers[BattleEventType.Acting][actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, Actor, Target, ActionSummary);
        }
        //target
        foreach (ActionModifier actionModifier in Target.ActionModifiers[BattleEventType.Targeted][actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, Target, Actor, ActionSummary);
        }
        return baseValue;
    }

    private bool Roll(int chance)
    {
        int roll = Random.Range(0, 100);
        Debug.Log("Chance: " + chance + " Roll: " + roll);
        if (roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
