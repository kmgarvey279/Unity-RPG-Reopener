using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ActionType
{
    Attack, 
    Heal,
    Other
}

[System.Serializable]
public enum ElementalProperty
{
    None,
    Fire,
    Ice,
    Electric,
    Dark
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Action", menuName = "Action/Action")]
public class Action : ScriptableObject
{   
    [Header("Basic Info")]
    public string actionName;
    public ActionType actionType;
    [Header("Player Only")]
    public Image icon;
    [TextArea(5,10)]
    public string description;
    public int mpCost;
    [Header("Properties")]
    public int timeModifier;
    [Header("Damage")]
    public int power;
    public BattleStatType offensiveStat;
    public BattleStatType defensiveStat;
    ElementalProperty elementalProperty;
    [Header("Status Effect")]
    public float statusEffectPower;
    public int statusEffectChance;
    public StatusEffectSO statusEffectSO;
    [Header("Knockback")]
    public int knockback;
    //other effects
    [Header("Additional Effects")]
    public List<SubEffect> additionalEffects;
    [Header("Range and Radius")]
    public int range;
    public int aoe;
    public bool excludeStartingTile;
    //generates a line AOE instead of a circle
    public bool lineAOE;
    //end line AOE if it hits a target
    public bool stopAtOccupiedTile;
    [Header("Targeting")]
    public bool targetFriendly;
    public bool targetHostile;
    public bool targetSelfOnly;
    [Header("Animation (Cast)")]
    public bool hasCastAnimation;
    public string castAnimatorTrigger;
    public GameObject castGraphicPrefab;
    public float castAnimationDuration = 0.4f;
    public float castGraphicDelay = 0.15f;
    [Header("Animation (Projectile)")]
    public bool hasProjectileAnimation;
    public string projectileAnimatorTrigger;
    public GameObject projectileGraphicPrefab;
    public float projectileGraphicDelay = 0.15f;
    [Header("Animation (Move)")]
    public bool hasMoveAnimation;
    public string moveAnimatorTrigger;
    public GameObject moveGraphicPrefab;
    [Header("Animation (Effect)")]
    public string effectAnimatorTrigger;
    public GameObject effectGraphicPrefab;
    public float effectAnimationDuration = 0.4f;
    public float effectGraphicDelay = 0.15f;

    public void TriggerDamageEffect(Combatant user, Combatant target)
    {
        if(actionType == ActionType.Other)
        {
            return;
        }
        float rawDamage = Mathf.Clamp((float)power * (user.battleStatDict[offensiveStat].GetValue() / 100f + user.battleStatDict[offensiveStat].GetValue()) * Random.Range(0.85f, 1f), 1, 9999);
        if(actionType == ActionType.Heal)
        {
            int finalHeal = Mathf.FloorToInt(rawDamage);
            target.Heal(finalHeal);
        }
        else
        {
            bool didCrit = CritCheck(user); 
            if(didCrit)
            {
                rawDamage = rawDamage * 1.75f;
            }
            int finalDamage = Mathf.FloorToInt(rawDamage * (100 / (100 + target.battleStatDict[defensiveStat].GetValue())));
            target.Damage(finalDamage, user, didCrit);
        }
    }

    public bool HitCheck(int hitChance)
    {
        int roll = Random.Range(1, 100);
        if(roll <= hitChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CritCheck(Combatant user)
    {
        float critChance = user.battleStatDict[BattleStatType.CritRate].GetValue();
        float roll = Random.Range(1, 100);
        if(roll <= critChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TriggerStatusEffect(Combatant user, Combatant target)
    {
        if(statusEffectSO != null)
        {
            if(statusEffectSO.isBuff)
            {
                target.AddStatusEffect(statusEffectSO, user.battleStatDict[offensiveStat].GetValue());
            }
            else if(HitCheck(statusEffectChance))
            {
                target.AddStatusEffect(statusEffectSO, user.battleStatDict[offensiveStat].GetValue());
            }
        }
    }

    public void TriggerAdditionalEffects(Combatant user, Combatant target)
    {
        foreach(SubEffect additionalEffect in additionalEffects)
        {
            additionalEffect.TriggerEffect(user, target);
        }
    }
}

