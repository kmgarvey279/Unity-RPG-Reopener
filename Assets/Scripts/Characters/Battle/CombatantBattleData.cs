using Mono.Posix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatContainer
{
    [field: SerializeField] public IntStatType StatType { get; private set; }
    [field: SerializeField] public int StatValue { get; private set; }

    public StatContainer(IntStatType statType, int statValue)
    {
        StatValue = statValue;
        StatType = statType;
    }
}


//always default to false
public enum CombatantBool
{
    //incoming damage/healing
    CannotBeHit,
    CannotTakeDamage,
    CannotBeHealed,
    CannotBeKO,
    CannotEvade,
    CannotCounter,
    TookDamageThisTurn,
    //actions
    CannotActOnTurn,
    CannotUseMelee,
    CannotUseSkills,
    CannotUseItems,
    CannotDefend,
    CannotSwap,
    //hits/crits
    AttacksAlwaysCrit,
    AttacksAlwaysHit,
    HealsAlwaysCrit,
    //interventions
    CannotQueueIntervention,
    //targeting
    CannotTargetSelf,
    CannotTargetAlly,
    CannotTargetHostile,
    CannotBeTargetedByAlly,
    CannotBeTargetedByHostile,
    MustBeTargetedByHostile
}

[System.Serializable]
public class CombatantBattleData
{
    [field: SerializeField] public string CharacterName { get; set; }
    [field: SerializeField] public string CharacterLetter { get; set; } = "";
    public Sprite TurnIcon { get; set; }

    [field: SerializeField] public ClampInt HP { get; set; }
    [field: SerializeField] public ClampInt Barrier { get; set; }
    [field: SerializeField] public ClampInt MP { get; set; }

    public Dictionary<IntStatType, int> Stats { get; set; } = new Dictionary<IntStatType, int>();
    public List<StatContainer> statList = new List<StatContainer>();
    public Dictionary<CombatantBool, int> CombatantBools { get; set; }
    [field: SerializeField] public List<Trait> Traits { get; set; } = new List<Trait>();
    public Dictionary<BattleEventType, Dictionary<UniversalModifierType, List<float>>> UniversalModifiers { get; set; } = new Dictionary<BattleEventType, Dictionary<UniversalModifierType, List<float>>>();
    public Dictionary<BattleEventType, Dictionary<ActionModifierType, List<ActionModifier>>> ActionModifiers { get; set; } = new Dictionary<BattleEventType, Dictionary<ActionModifierType, List<ActionModifier>>>();
    public List<PreemptiveBattleEventTrigger> PreemptiveBattleEventTriggers { get; set; } = new List<PreemptiveBattleEventTrigger>();
    public Dictionary<BattleEventType, List<BattleEventTrigger>> BattleEventTriggers { get; set; } = new Dictionary<BattleEventType, List<BattleEventTrigger>>();
    [field: SerializeField] public List<StatusEffectInstance> StatusEffectInstances { get; set; } = new List<StatusEffectInstance>();
    //public List<StatusEffectInstance> StatusesToRemove { get; protected set; } = new List<StatusEffectInstance>();

    public void CreateDicts()
    {
        //bools, 0 == false, > 0 == true 
        CombatantBools = new Dictionary<CombatantBool, int>();
        foreach (CombatantBool combatantBool in System.Enum.GetValues(typeof(CombatantBool)))
        {
            CombatantBools.Add(combatantBool, 0);
        }

        //triggerable effects: create empty dict/lists
        BattleEventTriggers.Add(BattleEventType.Acting, new List<BattleEventTrigger>());
        BattleEventTriggers.Add(BattleEventType.Targeted, new List<BattleEventTrigger>());

        //universal modifiers: create empty lists
        UniversalModifiers.Add(BattleEventType.Acting, new Dictionary<UniversalModifierType, List<float>>());
        foreach (UniversalModifierType universalModifierType in System.Enum.GetValues(typeof(UniversalModifierType)))
        {
            UniversalModifiers[BattleEventType.Acting].Add(universalModifierType, new List<float>());
        }
        UniversalModifiers.Add(BattleEventType.Targeted, new Dictionary<UniversalModifierType, List<float>>());
        foreach (UniversalModifierType universalModifierType in System.Enum.GetValues(typeof(UniversalModifierType)))
        {
            UniversalModifiers[BattleEventType.Targeted].Add(universalModifierType, new List<float>());
        }

        //action modifiers: create empty lists
        ActionModifiers.Add(BattleEventType.Acting, new Dictionary<ActionModifierType, List<ActionModifier>>());
        foreach (ActionModifierType actionModifierType in System.Enum.GetValues(typeof(ActionModifierType)))
        {
            ActionModifiers[BattleEventType.Acting].Add(actionModifierType, new List<ActionModifier>());
        }
        ActionModifiers.Add(BattleEventType.Targeted, new Dictionary<ActionModifierType, List<ActionModifier>>());
        foreach (ActionModifierType actionModifierType in System.Enum.GetValues(typeof(ActionModifierType)))
        {
            ActionModifiers[BattleEventType.Targeted].Add(actionModifierType, new List<ActionModifier>());
        }
    }

    public void ApplyTraits()
    {
        foreach (Trait trait in Traits)
        {
            foreach (CombatantBool combatantBool in trait.BoolsToModify)
            {
                ModifyBool(combatantBool, true);
            }
            foreach (PreemptiveBattleEventTrigger battleEventTrigger in trait.PreemptiveBattleEventTriggers)
            {
                if (battleEventTrigger != null)
                {
                    PreemptiveBattleEventTriggers.Add(battleEventTrigger);
                }
            }
            foreach (BattleEventTrigger battleEventTrigger in trait.BattleEventTriggers)
            {
                if (battleEventTrigger != null)
                {
                    BattleEventTriggers[battleEventTrigger.BattleEventType].Add(battleEventTrigger);
                }
            }
            foreach (UniversalModifier universalModifier in trait.UniversalModifiers)
            {
                UniversalModifiers[universalModifier.BattleEventType][universalModifier.UniversalModifierType].Add(universalModifier.ModifierValue);
            }
            foreach (ActionModifier actionModifier in trait.ActionModifiers)
            {
                if (actionModifier != null)
                {
                    ActionModifiers[actionModifier.BattleEventType][actionModifier.ActionModifierType].Add(actionModifier);
                }
            }
        }
    }

    public bool CheckBool(CombatantBool combatantBool)
    {
        if (CombatantBools[combatantBool] == 0)
        {
            return false;
        }
        return true;
    }

    public void ModifyBool(CombatantBool combatantBool, bool isTrue)
    {
        if (isTrue)
        {
            CombatantBools[combatantBool] += 1;
        }
        else
        {
            CombatantBools[combatantBool] -= 1;
            if (CombatantBools[combatantBool] < 0)
            {
                CombatantBools[combatantBool] = 0;
            }
        }
    }

    public void ResetBool(CombatantBool combatantBool)
    {
        CombatantBools[combatantBool] = 0;
    }

    public void AddStatusInstance(StatusEffectInstance newStatusInstance)
    {
        StatusEffectInstances.Add(newStatusInstance);

        //bools
        foreach (CombatantBool combatantBool in newStatusInstance.StatusEffect.BoolsToModify)
        {
            ModifyBool(combatantBool, true);
        }

        //triggerable effects
        foreach (BattleEventTrigger battleEventTrigger in newStatusInstance.StatusEffect.BattleEventTriggers)
        {
            if (battleEventTrigger != null)
            {
                BattleEventTriggers[battleEventTrigger.BattleEventType].Add(battleEventTrigger);
            }
        }
        foreach (PreemptiveBattleEventTrigger battleEventTrigger in newStatusInstance.StatusEffect.PreemptiveBattleEventTriggers)
        {
            if (battleEventTrigger != null)
            {
                PreemptiveBattleEventTriggers.Add(battleEventTrigger);
            }
        }

        //universal mods
        foreach (UniversalModifier universalModifier in newStatusInstance.StatusEffect.UniversalModifiers)
        {
            UniversalModifiers[universalModifier.BattleEventType][universalModifier.UniversalModifierType].Add(universalModifier.ModifierValue);
        }

        //action mods
        foreach (ActionModifier actionModifier in newStatusInstance.StatusEffect.ActionModifiers)
        {
            ActionModifiers[actionModifier.BattleEventType][actionModifier.ActionModifierType].Add(actionModifier);
        }
    }

    public void RemoveStatusInstance(StatusEffectInstance statusEffectInstance)
    {
        StatusEffectInstances.Remove(statusEffectInstance);

        //clear bools
        foreach (CombatantBool combatantBool in statusEffectInstance.StatusEffect.BoolsToModify)
        {
            ModifyBool(combatantBool, false);
        }

        //clear effects
        foreach (BattleEventTrigger battleEventTrigger in statusEffectInstance.StatusEffect.BattleEventTriggers)
        {
            BattleEventTriggers[battleEventTrigger.BattleEventType].Remove(battleEventTrigger);
        }
        foreach (PreemptiveBattleEventTrigger battleEventTrigger in statusEffectInstance.StatusEffect.PreemptiveBattleEventTriggers)
        {
            PreemptiveBattleEventTriggers.Remove(battleEventTrigger);
        }

        //clear universal mods
        foreach (UniversalModifier universalModifier in statusEffectInstance.StatusEffect.UniversalModifiers)
        {
            UniversalModifiers[universalModifier.BattleEventType][universalModifier.UniversalModifierType].Remove(universalModifier.ModifierValue);
        }

        //clear action mods
        foreach (ActionModifier actionModifier in statusEffectInstance.StatusEffect.ActionModifiers)
        {
            ActionModifiers[actionModifier.BattleEventType][actionModifier.ActionModifierType].Remove(actionModifier);
        }
    }
}
