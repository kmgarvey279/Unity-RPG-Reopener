using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    GenericAttack,
    Skill,
    Item
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
    public float mpCost;
    public int turnPointCost;
    public float timeCost;
    [Header("Properties")]
    public float power;
    public int accuracy;
    public int range;
    public int aoe;
    public AttackProperty attackProperty1;
    public AttackProperty attackProperty2;
    [Header("Effects")]
    public List<ActionEffect> effects = new List<ActionEffect>();
    [Header("Targeting")]
    public bool targetFriendly;
    public bool targetHostile;
    public bool fixedTarget;
    public GameObject hitboxPrefab;
    [Header("Animation")]
    public GameObject useGraphicPrefab;
    public GameObject effectGraphicPrefab;
    public string animatorTrigger;
}

