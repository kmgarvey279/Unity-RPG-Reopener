using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    Attack,
    Heal,
    CureAilment,
    Buff,
    Debuff,
    Other
}

public enum AttackProperty
{
    None,
    Melee,
    Ranged,
    Magic
}

public enum ElementalProperty
{
    None,
    Fire,
    Ice,
    Electric,
    Void
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
    public int power;
    public int accuracy;
    public int range;
    public int aoe;
    public AttackProperty attackProperty;
    public ElementalProperty elementalProperty;
    public bool guaranteedHit;
    public bool distancePenalty;
    [Header("Effects")]
    public List<ActionEffect> effects = new List<ActionEffect>();
    [Header("Targeting")]
    public bool targetFriendly;
    public bool targetHostile;
    public bool fixedTarget;
    [Header("Animation")]
    public GameObject useGraphicPrefab;
    public GameObject effectGraphicPrefab;
    public string animatorTrigger;
}

