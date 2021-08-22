using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    Attack,
    Skill,
    Item,
    Move
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
    public int mpCost;
    public int timeCost;
    [Header("Properties")]
    public int power;
    public int accuracy;
    public int range;
    public int aoe;
    public AttackProperty attackProperty1;
    public AttackProperty attackProperty2;
    public bool isSpecial;
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

