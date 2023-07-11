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

public enum TargetingType
{
    TargetHostile,
    TargetFriendly,
    TargetSelf,
    TargetAllies,
    TargetKOAllies
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
    [field: Header("Health Effects")]
    [field: SerializeField] public float Power { get; protected set; } = 1f;
    //[SerializeField] private float multiHitMultiplier = 0;
    [field: Header("Damage Properties")]
    [field: SerializeField] public AttackType AttackType { get; protected set; }
    [field: SerializeField] public StatType OffensiveStat { get; protected set; } = StatType.Attack;
    [field: SerializeField] public StatType DefensiveStat { get; protected set; } = StatType.Defense;
    [field: SerializeField] public ElementalProperty ElementalProperty { get; private set; } = ElementalProperty.None;
    [field: SerializeField] public bool IsMelee { get; private set; } = false;
    [field: SerializeField, Range(0, 1f)] public float Pierce { get; private set; } = 0;
    [field: Header("Accuracy/Crit/Hits")]
    [field: SerializeField] public int HitCount { get; protected set; } = 1;
    [field: SerializeField, Range(0.25f, 1f)] public float HitRate { get; protected set; } = 1f;
    [field: SerializeField] public bool GuaranteedHit { get; protected set; } = false;
    [field: SerializeField, Range(0.25f, 2f)] public float CritRateMultiplier { get; protected set; } = 1f;
    [field: Header("Targeting")]
    [field: SerializeField] public TargetingType TargetingType { get; protected set; }
    [field: SerializeField] public AOEType AOEType { get; protected set; }
    [field: SerializeField] public bool HitRandomTarget { get; protected set; } = false;
    [Header("Conditional Modifiers")]
    [SerializeField] private List<ActionModifier> ActionModifiers = new List<ActionModifier>();
    public Dictionary<ActionModifierType, List<ActionModifier>> ActionModifierDict { get; protected set; }
    [field: SerializeField, Header("Conditional Triggerable Effects")]
    public List<TriggerableBattleEffect> TriggerableBattleEffects { get; protected set; } = new List<TriggerableBattleEffect>();
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
}

