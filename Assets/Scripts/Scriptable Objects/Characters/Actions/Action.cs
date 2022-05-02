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
    AddStatusEffect,
    RemoveStatusEffects,
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
public class Knockback
{
    public bool doKnockback = false;
    [Range(-1, 1)] public int moveX;
    [Range(-1, 1)] public int moveY;

    public Vector2Int GetKnockbackDestination(Tile start)
    {
        Vector2Int newCoordinates = new Vector2Int(start.x, start.y);
        if(moveX != 0 && moveY != 0)
        {
            return newCoordinates;
        }
        int newX = newCoordinates.x + moveX;
        if(newX >= 0 && newX < 3)
        {
            newCoordinates.x = newCoordinates.x + moveX;
        }
        int newY = newCoordinates.y + moveY;
        if(newY >= 0 && newY < 3)
        {
            newCoordinates.y = newCoordinates.y + moveY;   
        }
        return newCoordinates;
    }
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
    [Range(0,99)]public int mpCost;
    public int apCost;
    [Header("Damage")]
    public int power;
    public int hitCount = 1;
    public BattleStatType offensiveStat;
    public BattleStatType defensiveStat;
    public ElementalProperty elementalProperty;
    [Header("Accuracy")]
    [Range(0,99)] public int accuracy;
    public bool guaranteedHit;
    [Header("Status Effect")]
    [Range(1, 100)] public int statusEffectChance;
    public StatusEffectSO statusEffectSO;
    [Header("Knockback")]
    public Knockback knockback = new Knockback();
    //other effects
    [Header("Additional Effects")]
    public List<SubEffect> additionalEffects;
    [Header("AOE")]
    public List<AOE> aoes = new List<AOE>();
    public bool isFixedAOE = false;
    public bool canFlip = false;
    //generates a line AOE instead of a circle
    [Header("Targeting")]
    public bool isMelee;
    public bool isFixedTarget;
    public bool targetFriendly;
    public bool hitRandomTarget;
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
    [Header("Animation (Effect)")]
    public string effectAnimatorTrigger;
    public GameObject effectGraphicPrefab;
    public float effectAnimationDuration = 0.4f;
    public float effectGraphicDelay = 0.15f;
}

