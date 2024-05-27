using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[System.Serializable]
public class PlayableCombatantRuntimeData    
{
    [field: SerializeField] public PlayableCharacterID PlayableCharacterID { get; private set; }
    [field: SerializeField] public PlayableCombatantStaticInfo StaticInfo { get; private set; }

    [field: SerializeField] public int Level { get; private set; } 
    [field: SerializeField] public int CurrentEXP { get; private set; } 

    [field: SerializeField] public int CurrentHP { get; private set; }
    [field: SerializeField] public int CurrentMP { get; private set; }
    [field: SerializeField] public int CurrentIP { get; private set; }

    [field: SerializeReference] public EquipmentItem EquippedWeapon;
    [field: SerializeReference] public EquipmentItem EquippedArmor;
    [field: SerializeReference] public EquipmentItem EquippedAccessory;
    public Dictionary<EquipmentType, EquipmentItem> Equipment 
    {
        get
        {
            return new Dictionary<EquipmentType, EquipmentItem>()
            {
                { EquipmentType.Weapon, EquippedWeapon },
                { EquipmentType.Armor, EquippedArmor },
                { EquipmentType.Accessory, EquippedAccessory }
            };
        }
    }
    [field: SerializeField] public List<Action> UnlockedSkills { get; private set; }
    [field: SerializeField] public List<Trait> UnlockedTraits { get; private set; }

    public PlayableCombatantRuntimeData(PlayableCombatantData playableCombatantData)
    {
        PlayableCharacterID = playableCombatantData.PlayableCharacterID;
        
        StaticInfo = DatabaseDirectory.Instance.PlayableCombatantDatabase.LookupTable[PlayableCharacterID];

        Level = playableCombatantData.Level;
        CurrentEXP = playableCombatantData.CurrentEXP;

        CurrentHP = playableCombatantData.CurrentHP;
        CurrentMP = playableCombatantData.CurrentMP;
        CurrentIP = playableCombatantData.CurrentIP;

        foreach (KeyValuePair<EquipmentType, string> entry in playableCombatantData.EquipmentIDs)
        {
            if (entry.Value == "")
            {
                continue;
            }


            DatabaseDirectory.Instance.ItemDatabase.LookupDictionary.TryGetValue(entry.Value, out Item item);
            if (item)
            {
                EquipmentItem equipmentItem = (EquipmentItem)item;
                if (!equipmentItem)
                {
                    continue;
                }

                if (entry.Key == EquipmentType.Weapon && equipmentItem.EquipmentType == EquipmentType.Weapon)
                {
                    EquippedWeapon = equipmentItem;
                }
                else if (entry.Key == EquipmentType.Armor && equipmentItem.EquipmentType == EquipmentType.Armor)
                {
                    EquippedArmor = equipmentItem;
                }
                else if (entry.Key == EquipmentType.Accessory && equipmentItem.EquipmentType == EquipmentType.Accessory)
                {
                    EquippedAccessory = equipmentItem;
                }
            }
        }

        UnlockedSkills = new List<Action>();
        foreach (Action skill in StaticInfo.Skills)
        {
            if (playableCombatantData.UnlockedSkillIDs.Contains(skill.ActionID))
            {
                UnlockedSkills.Add(skill);
            }
        }

        UnlockedTraits = new List<Trait>();
        foreach (Trait trait in StaticInfo.Traits)
        {
            if (playableCombatantData.UnlockedTraitIDs.Contains(trait.TraitID))
            {
                UnlockedTraits.Add(trait);
            }
        }
    }

    public PlayableCombatantData GetSerializableData()
    {
        PlayableCombatantData playableCombatantData = new PlayableCombatantData(PlayableCharacterID);
        
        playableCombatantData.Level = Level;
        playableCombatantData.CurrentEXP = CurrentEXP;

        playableCombatantData.CurrentHP = CurrentHP;
        playableCombatantData.CurrentMP = CurrentMP;
        playableCombatantData.CurrentIP = CurrentIP;

        playableCombatantData.SetEquipment(Equipment);
        playableCombatantData.SetUnlockedSkills(UnlockedSkills);
        playableCombatantData.SetUnlockedTraits(UnlockedTraits);

        return playableCombatantData;
    }

    public int GetStat(IntStatType intStatType)
    {
        return GetDerivedStatValue(intStatType);
    }

    public Dictionary<IntStatType, int> GetStatDict()
    {
        Dictionary<IntStatType, int> valuesToReturn = new Dictionary<IntStatType, int>();
        foreach (IntStatType intStatType in Enum.GetValues(typeof(IntStatType)))
        {
            valuesToReturn.Add(intStatType, GetDerivedStatValue(intStatType));
        }
        return valuesToReturn;
    }

    private int GetDerivedStatValue(IntStatType statType)
    {
        int hpBaseValue = 200;
        int mpBaseValue = 50;
        int standardBaseValue = 5;

        int baseValueToUse = standardBaseValue;
        if (statType == IntStatType.MaxHP)
        {
            baseValueToUse = hpBaseValue;
        }
        else if (statType == IntStatType.MaxMP)
        {
            baseValueToUse = mpBaseValue;
        }

        int value = Mathf.FloorToInt(baseValueToUse + (Level * StaticInfo.StatGrowth[statType]));
        
        //equipment
        foreach (KeyValuePair<EquipmentType, EquipmentItem> entry in Equipment)
        {
            if (entry.Value)
            {
                foreach (IntStatModifier statModifier in entry.Value.IntStatModifiers)
                {
                    if (statModifier.IntStatType == statType)
                    {
                        value += statModifier.Modifier;
                    }
                }
            }
        }

        //traits
        foreach (Trait trait in UnlockedTraits)
        {
            foreach (IntStatModifier statModifier in trait.IntStatModifiers)
            {
                if (statModifier.IntStatType == statType)
                {
                    value += (int)statModifier.Modifier;
                }
            }
        }

        int maxValue = 99;
        if (statType == IntStatType.MaxHP)
        {
            maxValue = 9999;
        }

        return Mathf.Clamp(value, 1, maxValue);
    }

    public void ChangeEquipment(EquipmentType equipmentType, EquipmentItem equipmentItem)
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                EquippedWeapon = equipmentItem;
                break;
            case EquipmentType.Armor:
                EquippedArmor = equipmentItem;
                break;
            case EquipmentType.Accessory:
                EquippedAccessory = equipmentItem;
                break;
            default:
                break;
        }
    }

    public void GainEXP(int exp)
    {
        int totalEXP = exp + CurrentEXP;

        for (int i = Level; i < 99; i++)
        {
            int requiredEXP = DatabaseDirectory.Instance.GeneralConsts.EXPRequirements[i];
            if (totalEXP / requiredEXP > 0)
            {
                totalEXP -= requiredEXP;
                Level++;
            }
        }
        CurrentEXP = totalEXP;
    }
}
