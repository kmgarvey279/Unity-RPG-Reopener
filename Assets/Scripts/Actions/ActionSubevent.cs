using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSubevent
{
    public Action Action { get; private set; }
    public Combatant Actor { get; private set; }
    public Combatant Target { get; private set; }
    public int TargetIndex { get; private set; }
    public int HitTally { get; private set; } = 0;
    public bool DidMiss { get; private set; } = false;
    public bool DidCrit { get; private set; } = false;
    public float HealthEffectSum { get; private set; } = 0;

    public ActionSubevent(Action _action, Combatant _actor, Combatant _target, int _targetIndex)
    {
        Action = _action;
        Actor = _actor;
        Target = _target;
        TargetIndex = _targetIndex;
    }

    public void Execute()
    {
        //if hit
        if (HitCheck())
        {
            bool isCrit = CritCheck();
            if(isCrit && !DidCrit)
            {
                DidCrit = true;
            }
            HitTally++;
            Debug.Log("Calling on action to execute");
            Action.ApplyEffect(this, isCrit, HitTally);
        }
        //miss
        else
        {
            Target.Evade();
            if (!DidMiss)
            {
                DidMiss = true;
            }
        }
    }

    public float GetHitRate()
    {
        //get base hit rate for action
        float hitRate = Action.HitRate - Target.Stats[StatType.Evade].GetValue();
        //apply any actor modifiers
        foreach (ActionModifier actionModifier in Actor.ActionModifiers[BattleEventType.Acting][ActionModifierType.HitRate])
        {
            hitRate += Mathf.Floor(hitRate * actionModifier.GetModifier(this));
        }
        //apply any target modifiers
        foreach (ActionModifier actionModifier in Target.ActionModifiers[BattleEventType.Targeted][ActionModifierType.HitRate])
        {
            hitRate += Mathf.Floor(hitRate * actionModifier.GetModifier(this));
        }
        //clamp between 1 and 100;
        hitRate = Mathf.Clamp(hitRate, 1f, 100f);
        Debug.Log("Hit rate: " + hitRate);
        return hitRate;
    }

    public bool HitCheck()
    {
        if (Action.GuaranteedHit || Action.TargetingType == TargetingType.TargetFriendly || Action.TargetingType == TargetingType.TargetSelf)
        {
            return true;
        }
        if (Roll(GetHitRate()))
        {
            return true;
        }
        return false;
    }

    public float GetCritRate()
    {
        float critRate = Actor.Stats[StatType.Crit].GetValue() * Action.CritRateMultiplier;
        foreach (ActionModifier actionModifier in Actor.ActionModifiers[BattleEventType.Acting][ActionModifierType.CritRate])
        {
            critRate += critRate * actionModifier.GetModifier(this);
        }
        foreach (ActionModifier actionModifier in Target.ActionModifiers[BattleEventType.Targeted][ActionModifierType.CritRate])
        {
            critRate += critRate * actionModifier.GetModifier(this);
        }
        Debug.Log("Crit rate: " + critRate);
        return critRate;
    }
    public bool CritCheck()
    {
        if (Roll(GetCritRate()))
        {
            return true;
        }
        return false;
    }

    public bool Roll(float chance)
    {
        int roll = Random.Range(1, 100);
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
