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
    public int power;
    public int accuracy;
    public int range;
    public int aoe;
    //determines what stat is used to calculate offense
    public BattleStatType offensiveStat;
    //determines what stats is used to calculate defense
    public BattleStatType defensiveStat;
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

