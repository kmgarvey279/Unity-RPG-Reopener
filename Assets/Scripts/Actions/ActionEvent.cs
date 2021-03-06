using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEvent
{
    public Action action;
    public Combatant actor;                                         
    public Combatant primaryTarget;                                                           
    public List<Combatant> targets = new List<Combatant>();
    public CombatantType combatantType;
    public int hitCounter = 0;
    public bool canCounter = false;
    public bool turnModifierApplied = false;
    public Dictionary<ActionModifierType, List<ActionModifier>> actionModifiers = new Dictionary<ActionModifierType, List<ActionModifier>>()
    {
        {ActionModifierType.ActionPowerPhysical, new List<ActionModifier>()},
        {ActionModifierType.ActionPowerMagic, new List<ActionModifier>()},
        {ActionModifierType.ActionPowerHeal, new List<ActionModifier>()},
        {ActionModifierType.TargetDefensePhysical, new List<ActionModifier>()},
        {ActionModifierType.TargetDefenseMagic, new List<ActionModifier>()},
        {ActionModifierType.HitRate, new List<ActionModifier>()},
        {ActionModifierType.CritRate, new List<ActionModifier>()},
        {ActionModifierType.CritPower, new List<ActionModifier>()},
        {ActionModifierType.EffectRate, new List<ActionModifier>()},
        {ActionModifierType.FinalPowerPhysical, new List<ActionModifier>()},
        {ActionModifierType.FinalPowerMagic, new List<ActionModifier>()}
    };

    public string targetAnimatorTrigger = "";
    public Combatant overrideTarget; 

    public ActionEvent(Action action, Combatant actor)
    {
        this.action = action;
        this.actor = actor;
        this.combatantType = GetCombatantType();
        if(action.targetingType == TargetingType.TargetHostile)
        {
            targetAnimatorTrigger = "Stun";
        }
        foreach(ActionEffect actionEffect in action.actionEffects)
        {
            if(actionEffect is ActionEffectDamage)
            {
                canCounter = true;
                break;
            }
        }
    }

    public void TriggerEvent()
    {
        //hit check
        float hitRate = GetHitRate();
        Combatant target = targets[0];
        //hit
        if(Roll(hitRate) || action.guaranteedHit)
        {
            //change target sprite
            target.TriggerActionEffectAnimation(targetAnimatorTrigger);
            //apply effects to target
            foreach(ActionEffect actionEffect in action.actionEffects)
            {
                actionEffect.ApplyEffect(this);
            } 
        }
        //miss
        else
        {
            target.TriggerActionEffectAnimation("Move");
            target.DisplayMessage(PopupType.Miss, "MISS");
        }

    }

    public ActionEvent Clone()
    {
        ActionEvent copy = new ActionEvent(action, actor);
        copy.primaryTarget = primaryTarget;
        copy.targets = targets;
        return copy;
    }

    public CombatantType GetCombatantType()
    {
        CombatantType combatantType = CombatantType.Player;
        if(action.targetingType == TargetingType.TargetHostile && actor.combatantType == CombatantType.Player
            || action.targetingType == TargetingType.TargetFriendly && actor.combatantType == CombatantType.Enemy)
        {
            combatantType = CombatantType.Enemy;
        }
        return combatantType;
    }

    public bool Roll(float chance)
    {
        int roll = Random.Range(1, 100);
        if(roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetHitRate()
    {
        float accuracy = action.accuracy + actor.battleStatDict[BattleStatType.Accuracy].GetValue();
        float evasion = targets[0].battleStatDict[BattleStatType.Evasion].GetValue();
        float hitRate = accuracy - evasion;
        foreach(ActionModifier actionModifier in actionModifiers[ActionModifierType.HitRate])
        {
            hitRate *= actionModifier.multiplier;
        }
        foreach(ActionModifier actionModifier in actionModifiers[ActionModifierType.HitRate])
        {
            hitRate += actionModifier.additive;
        }
        return Mathf.Clamp(hitRate, 1f, 100f);
    }
}