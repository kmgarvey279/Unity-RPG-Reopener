using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    Attack,
    Heal,
    ApplyBuff,
    RemoveBuff,
    RemoveAllBuffs,
    ApplyDebuff,
    RemoveDebuff,
    RemoveAllDebuffs,
    Other
}

public enum TargetingType
{
    TargetHostile,
    TargetFriendly,
    TargetSelf,
    TargetAllies,
    TargetKOAllies
}

public enum AOEType
{
    Single,
    Row,
    All,
    Random
}

public enum ExecutePosition
{
    StartingPosition,
    TargetRow,
    TargetProximity,
    FrontCenter
}

[System.Serializable]
public class ConditionalTurnModifierPreview
{
    [field: SerializeField, Range(-1f, 1f)] public float TurnModifier { get; protected set; } = 0;
    [field: SerializeField] public bool ApplyToNextTurnOnly { get; private set; }
    [SerializeField] private List<BattleConditionContainer> modifierConditions = new List<BattleConditionContainer>();
    public bool ConditionsCheck(Combatant combatantA, Combatant combatantB, ActionSummary actionSummary)
    {
        bool conditionsSatisfied = true;
        foreach (BattleConditionContainer battleConditionContainer in modifierConditions)
        {
            //get combatant to check
            Combatant combatantToCheck = combatantA;
            if (battleConditionContainer.BattleConditionCombatant == BattleConditionCombatant.CombatantB)
            {
                combatantToCheck = combatantB;
            }

            //check conditions
            if (battleConditionContainer.Conditional == Conditional.If)
            {
                conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
            }
            else if (battleConditionContainer.Conditional == Conditional.And)
            {
                if (!conditionsSatisfied)
                {
                    break;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
                }
            }
            else if (battleConditionContainer.Conditional == Conditional.Or)
            {
                if (conditionsSatisfied)
                {
                    return true;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
                }
            }
        }
        return conditionsSatisfied;
    }
}


[System.Serializable]
[CreateAssetMenu(fileName = "New Action", menuName = "Action/Action")]
public class Action : ScriptableObject
{
    public string ActionID { private set; get; } = System.Guid.NewGuid().ToString();

    [field: Header("Display Info")]
    [field: SerializeField] public string ActionName { get; protected set; }
    [field: SerializeField] public ActionType ActionType { get; private set; }
    [field: SerializeField] public Sprite Icon { get; protected set;}
    [field: SerializeField, TextArea(2, 6)] public string Description { get; protected set; }
    [field: SerializeField, TextArea(2, 6)] public string SecondaryDescription { get; protected set; }
    //[field: SerializeField] public ActionType ActionType { get; protected set; }
    //only applies to target select preview, actual changes are handled by triggerable effects
    [field: Header("Turn Modifier Preview")]
    [field: SerializeField] public ConditionalTurnModifierPreview ActorConditionalModifier { get; protected set; } = new ConditionalTurnModifierPreview();
    [field: SerializeField] public ConditionalTurnModifierPreview TargetConditionalModifier { get; protected set; } = new ConditionalTurnModifierPreview();

    //[field: Header("Cast Time")]
    //[field: SerializeField] public bool HasCastTime { get; protected set; } = false;
    [field: Header("MP Cost")]
    [field: SerializeField] public int MPCost { get; protected set; } = 0;
    [field: SerializeField] public bool ConsumeAllMP { get; protected set; } = false;
    [field: Header("HP Cost")]
    [field: SerializeField, Range(0, 0.9f)] public float HPPercentCost { get; protected set; } = 0;
    [field: Header("Hits and Targeting")]
    [field: SerializeField] public int HitCount { get; protected set; } = 1;
    [field: SerializeField] public TargetingType TargetingType { get; protected set; }
    [field: SerializeField] public AOEType AOEType { get; protected set; }
    [field: SerializeField] public bool IsMelee { get; private set; } = false;
    [field: SerializeField] public bool IsBackAttack { get; private set; } = false;
    [field: Header("Special Use Conditions")]
    [field: SerializeField] public List<BattleConditionContainer> UseConditions { get; private set; } = new List<BattleConditionContainer>();
    [Header("Conditional Modifiers")]
    [field: SerializeField] private List<ActionModifier> ActionModifiers = new List<ActionModifier>();
    public Dictionary<ActionModifierType, List<ActionModifier>> ActionModifierDict { get; protected set; }
    [field: SerializeField] private List<CustomActionModifier> CustomActionModifiers = new List<CustomActionModifier>();
    public Dictionary<ActionModifierType, List<CustomActionModifier>> CustomActionModifierDict { get; protected set; }
    [field: Header("Conditional Triggerable Effects")]
    [field: SerializeField] public List<BattleEventTrigger> BattleEventTriggers { get; protected set; } = new List<BattleEventTrigger>();
    //only used to display status effect info  
    [field: SerializeField] public List<StatusEffect> StatusEffectTags { get; protected set; } = new List<StatusEffect>();
    [field: Header("Animations + Sound Effects")]
    [field: SerializeField] public ExecutePosition ExecutePosition { get; protected set; } = ExecutePosition.StartingPosition;
    [field: SerializeField] public List<ActionAnimationData> ActionAnimations { get; protected set; } = new List<ActionAnimationData>();

    private void OnEnable()
    {
        ActionModifierDict = new Dictionary<ActionModifierType, List<ActionModifier>>();
        foreach (ActionModifierType actionModifierType in System.Enum.GetValues(typeof(ActionModifierType)))
        {
            ActionModifierDict.Add(actionModifierType, new List<ActionModifier>());
        }
        foreach (ActionModifier actionModifier in ActionModifiers)
        {
            ActionModifierDict[actionModifier.ActionModifierType].Add(actionModifier);
        }

        CustomActionModifierDict = new Dictionary<ActionModifierType, List<CustomActionModifier>>();
        foreach (ActionModifierType actionModifierType in System.Enum.GetValues(typeof(ActionModifierType)))
        {
            CustomActionModifierDict.Add(actionModifierType, new List<CustomActionModifier>());
        }
        foreach (CustomActionModifier customActionModifier in CustomActionModifiers)
        {
            CustomActionModifierDict[customActionModifier.ActionModifierType].Add(customActionModifier);
        }
    }

    public bool IsUsable(Combatant actor)
    {
        //check special use conditions
        if (!ConditionsCheck(actor)) 
        {
            return false;
        }
        //check mp cost
        if (MPCost > actor.MaxMP)
        {
            return false;
        }
        //check hp cost
        if (HPPercentCost > 0)
        {
            int requiredHP = Mathf.FloorToInt(HPPercentCost / actor.MaxHP) + 1;
            
            if (requiredHP > actor.HP)
            {
                return false;
            }
        }
        return true;
    }

    public bool ConditionsCheck(Combatant actor)
    {
        bool conditionsSatisfied = true;
        foreach (BattleConditionContainer battleConditionContainer in UseConditions)
        {
            Combatant combatantToCheck = actor;
            if (battleConditionContainer.Conditional == Conditional.If)
            {
                conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck);
            }
            else if (battleConditionContainer.Conditional == Conditional.And)
            {
                if (!conditionsSatisfied)
                {
                    continue;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck);
                }
            }
            else if (battleConditionContainer.Conditional == Conditional.Or)
            {
                if (conditionsSatisfied)
                {
                    return true;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck);
                }
            }
        }
        return conditionsSatisfied;
    }
}

