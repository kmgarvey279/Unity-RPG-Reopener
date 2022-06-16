using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public enum ActionType
{
    Attack, 
    Support,
    Move,
    Other
}

public enum TargetingType
{
    TargetFriendly,
    TargetHostile,
    TargetAll,
    TargetSelf

}

[System.Serializable]
public enum ElementalProperty
{
    None,
    Fire,
    Ice,
    Electric,
    Dark,
    Light
}

[System.Serializable]
public enum AOEType
{
    Single,
    Cross,
    X,
    Row,
    Column,
    All
}

[System.Serializable]
public class AOE
{
    public AOEType aoeType;
    public Vector2Int fixedStartPosition = new Vector2Int(0,0);
}

[System.Serializable]
public class ActionEffectTrigger
{
    public ActionEffect actionEffect;
    [Range(1, 100)] public float successRate = 100f;

    public virtual void TriggerActionEffect(ActionEvent actionEvent)
    {
        if(Roll(successRate))
            actionEffect.ApplyEffect(actionEvent);
    }

    private bool Roll(float chance)
    {
        float roll = Random.Range(1, 100);
        if(roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Action", menuName = "Action/Action")]
public class Action : ScriptableObject
{   
    [Header("Basic Info")]
    public string actionName;
    public ActionType actionType;
    [Header("Player UI Info")]
    public Image icon;
    [TextArea(5,10)]
    public string description;
    [Header("Cost")]
    [Range(0,99)]public int mpCost;
    public bool costsHP;
    [Header("Properties")]
    public float power = 10f;
    public int hitCount = 1;
    public BattleStatType offensiveStat = BattleStatType.None;
    public BattleStatType defensiveStat = BattleStatType.None;
    public ElementalProperty elementalProperty = ElementalProperty.None;
    [Header("Effects")]
    public List<ActionEffectTrigger> actionEffectTriggers = new List<ActionEffectTrigger>();
    [Header("Accuracy and Crit Rate")]
    [Range(1,100)] public float accuracy = 95f;
    [Range(1,100)] public float critRate = 7f;
    public bool guaranteedHit = false;
    [Header("Status Effect")]
    public StatusEffectSO statusEffectSO;
    [Header("AOE")]
    public List<AOE> aoes = new List<AOE>();
    public bool isFixedAOE = false;
    public bool canFlip = false;
    //generates a line AOE instead of a circle
    [Header("Targeting")]
    public bool isMelee;
    public bool isFixedTarget;
    public TargetingType targetingType;
    public bool hitRandomTarget;
    [Header("Animation (Cast)")]
    public bool hasCastAnimation;
    public string castAnimatorTrigger;
    public GameObject castGraphicPrefab;
    public float castAnimationDuration = 0.4f;
    public float castGraphicDelay = 0.2f;
    [Header("Animation (Execute)")]
    public string executeAnimatorTrigger;
    public float effectGraphicDelay = 0.4f;
    [Header("Animation (Projectile)")]
    public bool hasProjectileAnimation;
    public GameObject projectileGraphicPrefab;
    [Header("Animation (Effect)")]
    public GameObject effectGraphicPrefab;
    public float effectAnimationDuration = 0.4f;
}

