using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [System.Serializable]
// public class HealthEffect
// {
//     public HealthEffectType healthEffectType;
//     public bool doFixedPercentage;
//     public float fixedPercentage; 
//     public float fixedPercentageBoss;

//     public void Trigger(Combatant combatant, float potency)
//     {
//         if(healthEffectType == HealthEffectType.DecreaseHP)
//         {
//             combatant.Damage(potency);
//         }
//         else if(healthEffectType == HealthEffectType.IncreaseHP)
//         {
//             combatant.Heal(potency);
//         }
//     }
// }

[System.Serializable]
public class ResistanceModifier
{
    public ElementalProperty resistanceToModify;
    public int additive = 0;
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffectSO : ScriptableObject
{
    [Header("Effect info")]
    public Sprite icon;
    public string effectName;
    public string effectInfo;
    public bool isBuff;
    public bool canRemove;
    public bool cannotRefresh;
    [Header("Animation")]
    public string animatorTrigger;
    public GameObject effectObject;
    [Header("Duration")]
    public bool hasDuration;
    public int turnDuration;
    [Header("Other Effects")]
    //stat changes
    public List<BattleStatModifier> battleStatModifiers;
    //only applied when taking certain actions
    public List<ActionModifier> actionModifiers;
    //resistance changes
    public List<ResistanceModifier> resistanceModifiers;
    // //other effects
    // public List<TriggerableSubEffect> triggerableSubEffects;
    // public HealthEffect healthEffect;
    public List<SubEffect> onApplyEffects = new List<SubEffect>();
    public List<SubEffect> onRemoveEffects = new List<SubEffect>();
    public List<TriggerableSubEffect> onTurnEndEffects = new List<TriggerableSubEffect>();
    [Header("Interactions w/ other status effects")]
    public StatusEffectSO effectToCancel;
    public bool cancelSelf;


    public StatusEffectInstance CreateInstance()
    {
        return new StatusEffectInstance(this);
    }
}

