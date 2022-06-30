using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    None,
    Tile,
    Cross,
    Row,
    Column,
    All
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
    [Header("Player UI Info")]
    public Image icon;
    [TextArea(5,10)]
    public string description;
    [Header("Cost")]
    [Range(0,99)]public int mpCost;
    public bool costsHP;
    [Header("Targeting and Accuracy")]
    public int hitCount = 1;
    [Range(1,100)] public float accuracy = 95f;
    public bool guaranteedHit = false;
    public AOEType aoeType;
    public bool isMelee;
    public TargetingType targetingType;
    public bool hitRandomTarget;
    [Header("Effects")]
    [SerializeField] private List<ActionEffectDamage> damageEffects = new List<ActionEffectDamage>();
    [SerializeField] private List<ActionEffectHeal> healEffects = new List<ActionEffectHeal>();
    [SerializeField] private List<ActionEffectRemoveStatus> removeStatusEffects = new List<ActionEffectRemoveStatus>();
    [SerializeField] private List<ActionEffectAddStatus> addStatusEffects = new List<ActionEffectAddStatus>();
    [SerializeField] private List<ActionEffectTurnModifier> turnModifierEffects = new List<ActionEffectTurnModifier>();
    [SerializeField] private List<ActionEffectKnockback> knockbackEffects = new List<ActionEffectKnockback>();
    [SerializeField] private List<ActionEffectCustom> customEffects = new List<ActionEffectCustom>();
    [HideInInspector] public List<ActionEffect> actionEffects;
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

    private void OnEnable()
    {
        actionEffects = new List<ActionEffect>();
        foreach(ActionEffectDamage actionEffect in damageEffects)
        {
            actionEffects.Add(actionEffect);
        } 
        foreach(ActionEffectHeal actionEffect in healEffects)
        {
            actionEffects.Add(actionEffect);
        } 
        foreach(ActionEffectRemoveStatus actionEffect in removeStatusEffects)
        {
            actionEffects.Add(actionEffect);
        } 
        foreach(ActionEffectAddStatus actionEffect in addStatusEffects)
        {
            actionEffects.Add(actionEffect);
        } 
        foreach(ActionEffectTurnModifier actionEffect in turnModifierEffects)
        {
            actionEffects.Add(actionEffect);
        } 
        foreach(ActionEffectKnockback actionEffect in knockbackEffects)
        {
            actionEffects.Add(actionEffect);
        } 
        foreach(ActionEffectCustom actionEffect in customEffects)
        {
            actionEffects.Add(actionEffect);
        } 
    }
}

