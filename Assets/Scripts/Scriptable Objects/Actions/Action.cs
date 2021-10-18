using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ActionType
{
    Attack,
    Heal,
    CureAilment,
    Buff,
    Debuff,
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
    public Image icon;
    public string actionName;
    public string descripton;
    public ActionType actionType;
    [Header("Costs")]
    public int timeCost;
    //playable characters only
    public int mpCost;
    //enemies only
    public int cooldown;
    [Header("Properties")]
    public float power;
    public int accuracy;
    public int range;
    public int aoe;
    public bool excludeStartingTile;
    //generates a line AOE instead of a circle
    public bool lineAOE;
    //end line AOE if it hits a target
    public bool stopAtOccupiedTile;
    //determines what stat is used to calculate offense
    public BattleStatType offensiveStat;
    //determines what stats is used to calculate defense
    public BattleStatType defensiveStat;
    public ElementalProperty elementalProperty;
    public bool guaranteedHit;
    public bool distancePenalty;
    [Header("Effects")]
    public List<ActionEffect> effects = new List<ActionEffect>();
    public StatusEffect statusEffect;
    public int knockback;
    public bool useDirection;
    [Header("Targeting")]
    public bool targetFriendly;
    public bool targetHostile;
    public bool targetSelfOnly;
    [Header("Animation (Cast)")]
    public bool hasCastAnimation;
    public string castAnimatorTrigger;
    public GameObject castGraphicPrefab;
    public float castAnimationDuration;
    public float castGraphicDelay;
    [Header("Animation (Projectile)")]
    public bool hasProjectileAnimation;
    public string projectileAnimatorTrigger;
    public GameObject projectileGraphicPrefab;
    public float projectileSpeed;
    public float projectileGraphicDelay;
    [Header("Animation (Move)")]
    public bool hasMoveAnimation;
    public string moveAnimatorTrigger;
    public GameObject moveGraphicPrefab;
    [Header("Animation (Effect)")]
    public string effectAnimatorTrigger;
    public GameObject effectGraphicPrefab;
    public float effectAnimationDuration;
    public float effectGraphicDelay;
}

