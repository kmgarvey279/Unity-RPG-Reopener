using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BattleStatModifier
{
    public BattleStatType statToModify;
    public float multiplier;
}

[System.Serializable]
public class ResistanceModifier
{
    public ElementalProperty resistanceToModify;
    public float multiplier;
}

public enum StatusType
{
    Physical,
    Mental
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffectSO : ScriptableObject
{
    [Header("Effect info")]
    public string effectName;
    public string effectInfo;
    public StatusType statusType;
    public bool isBuff;
    public bool canRemove;
    public bool cannotRefresh;
    public bool endOnHit;
    [Header("Icon + Animation")]
    public Sprite icon;
    public string animatorTrigger;
    public GameObject effectObject;
    [Header("Duration")]
    public bool hasDuration;
    public int turnDuration;
    [Header("Effects")]
    //heal/damage over time 
    public bool damageOverTime;
    public bool healOverTime;
    //stat changes
    public List<BattleStatModifier> battleStatModifiers;
    //resistance changes
    public List<ResistanceModifier> resistanceModifiers;
    //other effects
    public List<TriggerableSubEffect> triggerableSubEffects;

    public virtual int GetHealthChange(Combatant combatant, int potency, int remainingTurns)
    {
        return 0;
    }
}

