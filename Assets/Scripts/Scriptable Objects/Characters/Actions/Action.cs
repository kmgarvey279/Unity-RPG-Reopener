using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ActionType
{
    Attack, 
    Heal,
    Move,
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
public enum AOEType
{
    Single,
    Cross,
    X,
    Row,
    Column,
    Diagonal,
    All
}

[System.Serializable]
public class AOE
{
    public AOEType aoeType;
    public Vector2Int fixedStartPosition = new Vector2Int(0,0);
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
    public int apCost;
    [Header("Damage")]
    public int power;
    public BattleStatType offensiveStat;
    public BattleStatType defensiveStat;
    public ElementalProperty elementalProperty;
    [Header("Status Effect")]
    public float statusEffectPower;
    public int statusEffectChance;
    public StatusEffectSO statusEffectSO;
    [Header("Knockback")]
    public bool knockback;
    //other effects
    [Header("Additional Effects")]
    public List<SubEffect> additionalEffects;
    [Header("AOE")]
    public List<AOE> aoes = new List<AOE>();
    public bool isFixedAOE = false;
    //generates a line AOE instead of a circle
    [Header("Targeting")]
    public bool isMelee;
    public bool isFixedTarget;
    public bool targetFriendly;
    [Header("Animation (Cast)")]
    public string castAnimatorTrigger;
    public GameObject castGraphicPrefab;
    public float castAnimationDuration = 0.4f;
    public float castGraphicDelay = 0.15f;
    [Header("Animation (Projectile)")]
    public bool hasProjectileAnimation;
    public string projectileAnimatorTrigger;
    public GameObject projectileGraphicPrefab;
    public float projectileGraphicDelay = 0.15f;
    [Header("Animation (Effect)")]
    public string effectAnimatorTrigger;
    public GameObject effectGraphicPrefab;
    public float effectAnimationDuration = 0.4f;
    public float effectGraphicDelay = 0.15f;
    [Header("Animation (Tile Effect)")]
    public GameObject tileEffectGraphicPrefab;
    public float tileEffectAnimationDuration = 0.4f;

    public void TriggerDamageEffect(Combatant user, Combatant target)
    {
        if(actionType == ActionType.Other)
        {
            return;
        }
        float rawDamage = Mathf.Clamp((float)power * (user.battleStatDict[offensiveStat].GetValue() / 100f + user.battleStatDict[offensiveStat].GetValue()) * Random.Range(0.85f, 1f), 1, 9999);
        Debug.Log("raw damage: " + rawDamage);
        if(actionType == ActionType.Heal)
        {
            int finalHeal = Mathf.FloorToInt(rawDamage);
            target.Heal(finalHeal);
        }
        else
        {
            bool didCrit = HitCheck(user.battleStatDict[BattleStatType.CritRate].GetValue()); 
            if(didCrit)
            {
                rawDamage = rawDamage * 1.75f;
            }
            Debug.Log("Multiplier: " + 100f / (100f + target.battleStatDict[defensiveStat].GetValue()));
            int finalDamage = Mathf.FloorToInt(rawDamage * (100f / (100f + target.battleStatDict[defensiveStat].GetValue())));
            Debug.Log("final damage: " + finalDamage);
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

