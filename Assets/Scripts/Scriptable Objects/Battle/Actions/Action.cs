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
    ApplyDebuff,
    RemoveDebuff,
    Other
}

public enum AttackType
{
    Physical,
    Magic,
    Other
}

public enum HealthEffectType
{
    Stat,
    Percentage,
    Fixed,
    None
}

public enum TargetingType
{
    TargetHostile,
    TargetFriendly,
    TargetSelf
}

public enum ActionCostType
{
    None,
    HP,
    MP,
    Unique
}

public enum ElementalProperty
{
    None,
    Fire,
    Ice,
    Electric,
    Dark
}

public enum AOEType
{
    Tile,
    Row,
    All
}

public enum ExecutePosition
{
    StartingPosition,
    TargetRow,
    FrontCenter
}

public enum ActionVFXPosition
{
    Target,
    Actor,
    TargetedGridCenter
}

public enum CombatantBindPosition
{
    Center,
    Below, 
    Above,
    Front,
    Back
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Action", menuName = "Action/Action")]
public class Action : ScriptableObject
{
    [field: Header("Display Info")]
    [field: SerializeField] public string ActionName { get; protected set; }
    [field: SerializeField] public Sprite Icon { get; protected set;}
    [field: SerializeField, TextArea(1, 3)] public string Description { get; protected set; }
    [field: SerializeField] public ActionType ActionType { get; protected set; }
    [field: Header("Turn Modifiers")]
    [field: SerializeField, Range(-0.4f, 0.4f)] public float ActorTurnModifier { get; protected set; } = 0;
    [field: SerializeField, Range(-0.4f, 0.4f)] public float TargetTurnModifier { get; protected set; } = 0;

    [field: Header("Cast Time")]
    [field: SerializeField] public bool HasCastTime { get; protected set; } = false;
    [field: Header("Cost")]
    [field: SerializeField] public ActionCostType ActionCostType { get; protected set; }
    [field: SerializeField] public int Cost { get; protected set; } = 0;
    [Header("Health Effects")]
    [SerializeField] private HealthEffectType healthEffectType;
    [SerializeField] private float power = 1f;
    [SerializeField] private float primaryTargetBonus = 0;
    //[SerializeField] private float multiHitMultiplier = 0;
    [SerializeField] private StatType offensiveStat = StatType.Attack;
    [SerializeField] private StatType defensiveStat = StatType.Defense;
    private float varianceMin = 0.95f;
    private float varianceMax = 1.05f;
    [field: Header("Damage Properties")]
    [field: SerializeField] public AttackType AttackType { get; protected set; }
    [field: SerializeField] public ElementalProperty ElementalProperty { get; private set; } = ElementalProperty.None;
    [field: SerializeField] public bool IsMelee { get; private set; } = false;
    [field: SerializeField, Range(0, 1)] public float Pierce { get; private set; } = 0;
    [field: Header("Accuracy/Crit/Hits")]
    [field: SerializeField] public int HitCount { get; protected set; } = 1;
    [field: SerializeField, Range(1, 100)] public float HitRate { get; protected set; } = 100f;
    [field: SerializeField] public bool GuaranteedHit { get; protected set; } = false;
    [field: SerializeField, Range(0.25f, 2f)] public float CritRateMultiplier { get; protected set; } = 1f;
    [field: Header("Targeting")]
    [field: SerializeField] public TargetingType TargetingType { get; protected set; }
    [field: SerializeField] public AOEType AOEType { get; protected set; }
    [field: SerializeField] public bool HitRandomTarget { get; protected set; } = false;
    [field: SerializeField, Header("Conditional Modifiers")]
    public List<ActionModifier> ActionModifiers { get; protected set; } = new List<ActionModifier>();
    public Dictionary<ActionModifierType, List<ActionModifier>> ActionModifierDict { get; protected set; }
    [field: SerializeField, Header("Conditional Triggerable Effects")]
    public List<TriggerableBattleEffect> TriggerableBattleEffects { get; protected set; } = new List<TriggerableBattleEffect>();
    [field: Header("Animations + Sound Effects")]
    [field: SerializeField] public ExecutePosition ExecutePosition { get; protected set; } = ExecutePosition.StartingPosition;
    [field: SerializeField] public List<ActionAnimationData> ActionAnimations { get; protected set; } = new List<ActionAnimationData>();

    private void OnEnable()
    {
        ActionModifierDict = new Dictionary<ActionModifierType, List<ActionModifier>>();
    }

    public bool IsUsable(Combatant user)
    {
        //is the action locked by a status effect?
        bool isUsable = true;
        if(IsMelee && user.Tile.X == 0)
        {
            isUsable = false;
        }

        //can the user pay the required cost?
        if(ActionCostType != ActionCostType.None)
        {
            int userResource = 0;
            if (ActionCostType == ActionCostType.HP)
            {
                //attacks that use HP cannot be used if they'd kill the user
                userResource = user.HP.CurrentValue - 1;
            }
            else if (ActionCostType == ActionCostType.MP)
            {
                userResource = user.MP.CurrentValue;
            }
            //check cost
            if (userResource < Cost) 
            {
                isUsable = false;
            }
        }

        return isUsable;
    }

    public void ApplyEffect(ActionSubevent actionSubevent, bool isCrit, int hitNum)
    {
        if (ActionType == ActionType.Attack)
        {
            Debug.Log("applying attack to target");
            actionSubevent.Target.OnAttacked(GetHealthEffect(actionSubevent, isCrit), isCrit, ElementalProperty);
        }
        else if (ActionType == ActionType.Heal)
        {
            actionSubevent.Target.OnHealed(GetHealthEffect(actionSubevent, isCrit), isCrit);
        }
    }

    private int GetHealthEffect(ActionSubevent actionSubevent, bool isCrit)
    {
        //get action power
        float actionPower = power;
        if (actionSubevent.TargetIndex == 0)
        {
            actionPower += primaryTargetBonus;
        }

        //get relevent stat
        float stat = 0;
        if (actionSubevent.Actor.Stats.ContainsKey(offensiveStat))
        {
            stat = actionSubevent.Actor.Stats[offensiveStat].GetValue();
        }
        actionPower *= stat;

        //check for crit
        float critMultiplier = 1f;
        if (isCrit)
        {
            critMultiplier = 1.5f;
        }
        actionPower *= critMultiplier;

        //outgoing power modifiers
        ActionModifierType outgoingModifierType = ActionModifierType.Damage;
        if (ActionType == ActionType.Heal)
        {
            outgoingModifierType = ActionModifierType.Healing;
        }
        foreach (ActionModifier actionModifier in actionSubevent.Actor.ActionModifiers[BattleEventType.Acting][outgoingModifierType])
        {
            actionPower += actionPower * actionModifier.GetModifier(actionSubevent);
        }

        //defense modifiers
        if(ActionType == ActionType.Attack)
        {
            //get defense
            float defense = (actionSubevent.Target.Stats[defensiveStat].GetValue() * 3);
            foreach (ActionModifier actionModifier in actionSubevent.Target.ActionModifiers[BattleEventType.Targeted][ActionModifierType.Defense])
            {
                defense += defense * actionModifier.GetModifier(actionSubevent);
            }
            foreach (ActionModifier actionModifier in actionSubevent.Actor.ActionModifiers[BattleEventType.Acting][ActionModifierType.Defense])
            {
                defense += defense * actionModifier.GetModifier(actionSubevent);
            }

            //attack - defense
            defense -= (defense * Pierce);
            actionPower = actionPower - defense;
        }

        //elemental multiplier
        ElementalResistance elementalResistance = actionSubevent.Target.Resistances[ElementalProperty];
        float resistMultiplier = 1f;
        if (elementalResistance == ElementalResistance.Weak)
        {
            resistMultiplier = 1.5f;
        }
        else if (elementalResistance == ElementalResistance.Resist)
        {
            resistMultiplier = 0.6f;
        }
        else if (elementalResistance == ElementalResistance.Null)
        {
            resistMultiplier = 0f;
        }
        actionPower *= resistMultiplier;

        //elemental resist modifiers (playable characters only)
        actionPower -= (actionPower * (actionSubevent.Target.ResistanceModifiers[ElementalProperty] / 100f));

        //incoming power modifiers
        ActionModifierType incomingModifierType = ActionModifierType.Damage;
        if (ActionType == ActionType.Heal)
        {
            incomingModifierType = ActionModifierType.Healing;
        }
        foreach (ActionModifier actionModifier in actionSubevent.Target.ActionModifiers[BattleEventType.Targeted][incomingModifierType])
        {
            actionPower += actionPower * actionModifier.GetModifier(actionSubevent);
        }

        //rng variance
        actionPower *= Random.Range(varianceMin, varianceMax);

        //final value
        return Mathf.Clamp(Mathf.FloorToInt(actionPower), 1, 9999);
    }
}

