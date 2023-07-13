using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionSummaryValue
{
    DidHit,
    DidMiss,
    DidCrit,
    DidBlock,
    DidHitWeakness
}

public class ActionSummary
{
    public Action Action { get; private set; }
    public int CumHealthEffect { get; private set; } = 0;
    public Dictionary<ActionSummaryValue, bool> Values = new Dictionary<ActionSummaryValue, bool>();

    public ActionSummary(Action action)
    {
        Action = action;
        foreach (ActionSummaryValue actionSummaryValue in System.Enum.GetValues(typeof(ActionSummaryValue)))
        {
            Values.Add(actionSummaryValue, false);
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
}

[System.Serializable]
public class ActionSubevent
{
    [field: SerializeField] public ActionSummary ActionSummary { get; private set; }
    [field: SerializeField] public Combatant Actor { get; private set; }
    [field: SerializeField] public Combatant Target { get; private set; }
    private float chainMultiplier = 1f;
    private BattleFormulas battleFormulas = new BattleFormulas();

    public ActionSubevent(Action _action, Combatant _actor, Combatant _target, float _chainMultiplier)
    {
        ActionSummary = new ActionSummary(_action);
        Actor = _actor;
        Target = _target;
        chainMultiplier = _chainMultiplier;
        //TargetIndex = _targetIndex;
    }

    public void Execute()
    {
        if (ActionSummary.Action.ActionType == ActionType.Heal || ActionSummary.Action.ActionType == ActionType.Attack)
        {
            //if hit
            if (HitCheck())
            {
                ActionSummary.SetValueAsTrue(ActionSummaryValue.DidHit);
                bool didCrit = CritCheck();
                if (didCrit)
                {
                    ActionSummary.SetValueAsTrue(ActionSummaryValue.DidCrit);
                }
                bool wasBlocked = false;
                if (wasBlocked)
                {
                    ActionSummary.SetValueAsTrue(ActionSummaryValue.DidBlock);
                }
                if (ActionSummary.Action.ActionType == ActionType.Attack)
                {
                    Target.OnAttacked(GetHealthEffect(didCrit, wasBlocked), didCrit, wasBlocked, ActionSummary.Action.ElementalProperty);
                }
                else if (ActionSummary.Action.ActionType == ActionType.Heal)
                {
                    Target.OnHealed(GetHealthEffect(didCrit, wasBlocked), didCrit);
                }
            }
            //miss
            else
            {
                ActionSummary.SetValueAsTrue(ActionSummaryValue.DidMiss);
                Target.OnEvade();
            }
        }
    }

    public float GetHitRate()
    {
        float hitRate = battleFormulas.GetHitRate(ActionSummary.Action, Target);
        hitRate = ApplyActionModifiers(hitRate, ActionModifierType.HitRate);

        return Mathf.Clamp(hitRate, 0, 1f);
    }

    private float GetCritRate()
    {
        float critRate = battleFormulas.GetCritRate(ActionSummary.Action, Actor);
        critRate = ApplyActionModifiers(critRate, ActionModifierType.CritRate);

        return Mathf.Clamp(critRate, 0, 1f);
    }


    private bool HitCheck()
    {
        if (ActionSummary.Action.GuaranteedHit || ActionSummary.Action.TargetingType != TargetingType.TargetHostile)
        {
            return true;
        }
        if (Roll(GetHitRate()))
        {
            return true;
        }
        return false;
    }

    private bool CritCheck()
    {
        if (Roll(GetCritRate()))
        {
            return true;
        }
        return false;
    }

    //private bool BlockCheck()
    //{
    //    if (Roll(GetBlockRate()))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    public int GetHealthEffect(bool didCrit, bool wasBlocked)
    {
        HealthEffectType healthEffectType = HealthEffectType.Damage;
        if (ActionSummary.Action.ActionType == ActionType.Heal)
        {
            healthEffectType = HealthEffectType.Heal;
        }
        //get action power
        int attackPower = battleFormulas.GetActorPower(ActionSummary.Action.Power, Actor, ActionSummary.Action.OffensiveStat);
        Debug.Log("Base attack power: " + attackPower);

        //defense modifiers
        int defensivePower = 0;
        if (ActionSummary.Action.ActionType == ActionType.Attack)
        {
            //get defense
            defensivePower = battleFormulas.GetTargetDefense(Target, ActionSummary.Action.DefensiveStat, ActionSummary.Action.Pierce);
        }

        int finalActionPower = battleFormulas.GetHealthEffect(healthEffectType, attackPower, defensivePower);

        //crit
        float critMultiplier = 1f;
        if (didCrit)
        {
            critMultiplier = ApplyActionModifiers(Actor.SecondaryStats[SecondaryStatType.CritPower].CurrentValue, ActionModifierType.CritPower);
        }
        finalActionPower = Mathf.FloorToInt(finalActionPower * critMultiplier);

        //elemental multiplier
        ElementalResistance elementalResistance = Target.Resistances[ActionSummary.Action.ElementalProperty];
        float resistMultiplier = 1f;
        if (elementalResistance == ElementalResistance.Weak)
        {
            resistMultiplier = 1.5f;
        }
        else if (elementalResistance == ElementalResistance.Resist)
        {
            resistMultiplier = 0.6f;
        }
        else if (elementalResistance == ElementalResistance.Null)
        {
            resistMultiplier = 0f;
        }
        finalActionPower = Mathf.FloorToInt(finalActionPower * resistMultiplier);

        //elemental resist modifiers(playable characters only)
        //actionPower -= (actionPower * (actionSubevent.Target.ResistanceModifiers[ElementalProperty] / 100f));

        //misc. modifiers
        ActionModifierType actionModifierType = ActionModifierType.Damage;
        if (ActionSummary.Action.ActionType == ActionType.Heal)
        {
            actionModifierType = ActionModifierType.Healing;
        }
        finalActionPower = Mathf.FloorToInt(ApplyActionModifiers(finalActionPower, actionModifierType));
        
        //chain multiplier
        finalActionPower = Mathf.FloorToInt(finalActionPower * chainMultiplier);
        Debug.Log("action power after modifiers is " + finalActionPower);

        //block flat damage reduction
        if (ActionSummary.Action.ActionType == ActionType.Attack)
        {
            if (wasBlocked)
            {
                int blockPower = battleFormulas.GetTargetBlock(Target, ActionSummary.Action.DefensiveStat, ActionSummary.Action.Pierce);
                Debug.Log("block power: " + blockPower);

                finalActionPower = finalActionPower - blockPower;
            }
        }

        //final value
        finalActionPower = Mathf.Clamp(finalActionPower, 1, 9999);
        ActionSummary.UpdateCumHealthEffect(finalActionPower);

        Debug.Log("final action power: " + finalActionPower);
        return finalActionPower;
    }

    private float ApplyActionModifiers(float baseValue, ActionModifierType actionModifierType)
    {
        foreach (ActionModifier actionModifier in ActionSummary.Action.ActionModifierDict[actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, Actor, Target, ActionSummary);
        }
        foreach (ActionModifier actionModifier in Actor.ActionModifiers[BattleEventType.Acting][actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, Actor, Target, ActionSummary);
        }
        foreach (ActionModifier actionModifier in Target.ActionModifiers[BattleEventType.Targeted][actionModifierType])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, Target, Actor, ActionSummary);
        }
        return baseValue;
    }

    private bool Roll(float chance)
    {
        float roll = Random.Range(0, 1f);
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
