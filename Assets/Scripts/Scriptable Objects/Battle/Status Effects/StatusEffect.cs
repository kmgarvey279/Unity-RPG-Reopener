using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatusCounterType
{
    Turns, 
    Stacks,
    None
}

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

[System.Serializable]
[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffect : ScriptableObject
{
    [field: Header("Effect Info")]
    [field: SerializeField] public bool DisplayOnTargetInfo { private set; get; } = true;
    [field: SerializeField] public Sprite Icon { private set; get; }
    [field: SerializeField] public string EffectName { private set; get; } = "";
    [field: SerializeField, TextArea(1, 3)] public string EffectInfo { private set; get; } = "";
    [field: SerializeField] public StatusEffectType StatusEffectType { private set; get; }
    [field: SerializeField] public bool CanRemove { private set; get; } = true;
    [field: SerializeField] public bool RemoveOnKO { private set; get; } = true;

    [field: Header("Animation")]
    [field: SerializeField] public BattleAnimatorTrigger AnimatorOverride { private set; get; }
    [field: SerializeField] public string CustomAnimatorOverride { private set; get; }
    [field: SerializeField] public float TargetSpeedChange = 0;
    [field: SerializeField] public GameObject TriggerVFX { private set; get; }
    [field: SerializeField] public GameObject PersistentVFX { private set; get; }

    [field: Header("Counter")]
    [field: SerializeField] public StatusCounterType StatusCounterType { private set; get; }
    [field: SerializeField] public int CounterApply { private set; get; } = 3;
    [field: SerializeField] public int CounterMax { private set; get; } = 5;
    [field: SerializeField] public bool TickAtTurnStart { private set; get; } = false;
    [field: SerializeField] public bool RemoveOnTick { private set; get; } = false;
    [field: Header("Health Effect")]
    [field: SerializeField] public HealthEffectType HealthEffectType { private set; get; }
    [field: SerializeField] public float Power { private set; get; } = 0f;
    [field: SerializeField] public StatType OffensiveStat { private set; get; }
    [field: SerializeField] public StatType DefensiveStat { private set; get; }
    [field: Header("Other Effects")]
    [field: SerializeField] public List<ActionModifier> ActionModifiers { private set; get; } = new List<ActionModifier>();
    [field: SerializeField] public List<TriggerableBattleEffect> TurnEffects { private set; get; } = new List<TriggerableBattleEffect>();
    [field: SerializeField] public List<TriggerableBattleEffect> TriggerableBattleEffects { private set; get; } = new List<TriggerableBattleEffect>();

    [field: Header("Interactions w/ other status effects")]
    [field: SerializeField] public List<StatusEffect> EffectsToRemove { private set; get; } = new List<StatusEffect>();
}

