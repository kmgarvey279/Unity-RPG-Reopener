using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

[System.Serializable]
public class PlayableCombatantData 
{
    public PlayableCharacterID PlayableCharacterID;
    
    public int Level = 1;
    public int CurrentEXP = 0;

    public int CurrentHP = 100;
    public int CurrentMP = 20;
    public int CurrentIP = 0;

    public Dictionary<EquipmentType, string> EquipmentIDs = new Dictionary<EquipmentType, string>();

    public List<string> UnlockedSkillIDs = new List<string>();

    public List<string> UnlockedTraitIDs = new List<string>();


    public PlayableCombatantData(PlayableCharacterID playableCharacterID)
    {
        PlayableCharacterID = playableCharacterID;
    }

    public void SetEquipment(Dictionary<EquipmentType, EquipmentItem> equipment)
    {
        foreach (KeyValuePair<EquipmentType, EquipmentItem> entry in equipment)
        {
            if (entry.Value)
            {
                EquipmentIDs[entry.Key] = entry.Value.ItemID;
            }
            else
            {
                EquipmentIDs[entry.Key] = "";
            }
        }
    }

    public void SetUnlockedSkills(List<Action> skills)
    {
        List<string> idTemp = new List<string>();
        foreach (Action action in skills)
        {
            idTemp.Add(action.ActionID);
        }
        UnlockedSkillIDs = idTemp;
    }

    public void SetUnlockedTraits(List<Trait> traits)
    {
        List<string> idTemp = new List<string>();
        foreach (Trait trait in traits)
        {
            idTemp.Add(trait.TraitID);
        }
        UnlockedTraitIDs = idTemp;
    }
}
