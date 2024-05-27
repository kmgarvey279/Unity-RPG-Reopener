using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//also the order of execution for health effects
public enum HealthEffectType
{
    None,
    Heal,
    Damage
}

public enum StatusEffectType
{
    Buff,
    Debuff,
    Other
}

public enum TurnEventType
{
    OnStart,
    OnEnd
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffect : ScriptableObject
{
    [field: Header("Effect Info")]
    [field: SerializeField] public bool DisplayIcon { private set; get; } = true;
    [field: SerializeField] public Sprite Icon { private set; get; }
    [field: SerializeField] public string EffectName { private set; get; } = "";
    [field: SerializeField, TextArea(2, 10)] public string Description { private set; get; } = "";
    [field: SerializeField, TextArea(2, 10)] public string SecondaryDescription { private set; get; } = "";
    [field: SerializeField] public StatusEffectType StatusEffectType { private set; get; }

    [field: Header("Animation")]
    [field: SerializeField] public BattleAnimatorTrigger AnimatorOverride { private set; get; }
    [field: SerializeField] public string CustomAnimatorOverride { private set; get; }
    //[field: SerializeField] public float TargetSpeedChange = 0;
    [field: SerializeField] public GameObject PersistentVFX { private set; get; }
    [field: SerializeField] public CombatantSpawnPosition PersistentVFXPosition { private set; get; } = CombatantSpawnPosition.Center;
    [field: SerializeField] public GameObject PersistentTileVFX { private set; get; }
    [field: SerializeField] public bool SetTranparent { private set; get; }
    
    [field: Header("Counter")]
    [field: SerializeField] public bool HasDuration { private set; get; } = false;
    [field: SerializeField] public int DurationToApply { private set; get; } = 1;
    [field: SerializeField] public int DurationMax { private set; get; } = 1;
    [field: SerializeField] public bool CanIncreaseDuration { private set; get; } = true;
    [field: SerializeField] public bool CanDecreaseDuration { private set; get; } = true;
    [field: Header("Stacks")]
    [field: SerializeField] public bool HasStacks { private set; get; } = false;
    [field: SerializeField] public int StacksToApply { private set; get; } = 1;
    [field: SerializeField] public int StacksMax { private set; get; } = 1;
    [field: SerializeField] public bool CanIncreaseStacks { private set; get; } = true;

    [field: Header("Removal")]
    [field: SerializeField] public TurnEventType RemovalCheckTurnEventType { private set; get; }
    [field: SerializeField] public bool CanPurge { private set; get; } = true;
    [field: SerializeField] public bool DoNotRemoveOnKO { private set; get; }

    [field: Header("Potency")]
    //[field: SerializeField] public HealthEffectType HealthEffectType { private set; get; }
    //[field: SerializeField] public PotencyCalculatorType PotencyCalculatorType { private set; get; }
    //stat based hots/dots
    //[field: SerializeField] public float Power { private set; get; } = 0f;
    //[field: SerializeField] public IntStatType ActorStat { private set; get; }
    //changes on tick
    //[field: SerializeField] public bool ApplyPotencyAsBreach { private set; get; }
    [field: SerializeField] public float PotencyTurnMultiplier { private set; get; } = 1f;
    [field: SerializeField] public bool CanIncreasePotency { private set; get; }
    //[field: SerializeField] public float CarryoverPotencyMultiplier = 0;
    
    [field: Header("Turn Effects")]
    //trigger rate/conditions
    [field: SerializeField] public TurnEventType TriggerTurnEventType { private set; get; }
    [field: SerializeField] public List<BattleEventTrigger> TurnEventTriggers = new List<BattleEventTrigger>();
    [field: Header("Turn Modifier")]
    [field: SerializeField] public float TurnModifier { get; private set; } = 0;
    //[field: SerializeField] public bool ApplyModifierToNextTurnOnly { get; private set; } = false;
    //[field: SerializeField] public bool SubtractModifierOnExternalRemoval { get; private set; } = false;
    //[field: SerializeField] public bool ReapplyModifierOnStackIncrease { get; private set; } = false;
    [field: Header("Other Effects")]
    //[field: SerializeField] public Attack OverrideAttack { get; private set; }
    [field: SerializeField] public List<CombatantBool> BoolsToModify { get; private set; } = new List<CombatantBool>();
    //[field: SerializeField] public List<IntStatModifier> IntStatModifiers { get; private set; } = new List<IntStatModifier>();
    //[field: SerializeField] public List<FloatStatModifier> FloatStatModifiers { get; private set; } = new List<FloatStatModifier>();
    [field: SerializeField] public List<UniversalModifier> UniversalModifiers { private set; get; } = new List<UniversalModifier>();
    [field: SerializeField] public List<ActionModifier> ActionModifiers { private set; get; } = new List<ActionModifier>();
    [field: SerializeField] public List<PreemptiveBattleEventTrigger> PreemptiveBattleEventTriggers { private set; get; } = new List<PreemptiveBattleEventTrigger>();
    [field: SerializeField] public List<BattleEventTrigger> BattleEventTriggers { private set; get; } = new List<BattleEventTrigger>();

    [field: Header("Interactions w/ other status effects")]
    [field: SerializeField] public List<StatusEffect> EffectsToRemove { private set; get; } = new List<StatusEffect>();
}

