using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayableCharacterID
{
    Claire,
    Mutiny,
    Shad,
    Blaine,
    Lucy
}

public enum DamageType
{
    Physical,
    Magic
}

[CreateAssetMenu(fileName = "New Playable Character Info", menuName = "Character Info/Playable")]
public class PlayableCharacterInfo : CharacterInfo
{
    [Header("Standard Attacks")]
    public Action meleeAttack;
    public Action rangedAttack;

    [Header("Skills")]
    public List<Action> skills;

    [Header("Equipment")]
    [SerializeField] private WeaponObject weapon;
    [SerializeField] private ArmorObject armor;
    [SerializeField] private AccessoryObject accessory;
    public Dictionary<EquipmentType, EquipmentObject> equipmentDict = new Dictionary<EquipmentType, EquipmentObject>();
    
    [Header("ID")]
    public PlayableCharacterID characterID;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        statDict.Add(StatType.EquipmentMeleeAttack, new Stat(0));
        statDict.Add(StatType.EquipmentRangedAttack, new Stat(0));
        statDict.Add(StatType.EquipmentMagicAttack, new Stat(0));
        statDict.Add(StatType.EquipmentPhysicalDefense, new Stat(0));
        statDict.Add(StatType.EquipmentMagicDefense, new Stat(0));

        if(weapon)
            ChangeEquipment((EquipmentObject)weapon);
        if(armor)
            ChangeEquipment((EquipmentObject)armor);
        if(accessory)
            ChangeEquipment((EquipmentObject)accessory);
    }

    public void ChangeEquipment(EquipmentObject newEquipment)
    {
        EquipmentType equipmentType = newEquipment.equipmentType;
        //update stat modifiers
        if(equipmentDict[equipmentType] != null)
        {
            //clear stat modifiers
            foreach(KeyValuePair<StatType, Stat> stat in statDict)
            { 
                stat.Value.RemoveModifier(equipmentDict[equipmentType].modifierDict[stat.Key]);
            }
            //clear elemental modifiers
            foreach(KeyValuePair<ElementalProperty, Stat> stat in resistDict)
            { 
                stat.Value.RemoveModifier(equipmentDict[equipmentType].resistDict[stat.Key]);
            }
            equipmentDict.Remove(equipmentType);
        }
        foreach(KeyValuePair<StatType, Stat> stat in statDict)
        { 
            stat.Value.AddModifier(newEquipment.modifierDict[stat.Key]);
        }            
        equipmentDict[equipmentType] = newEquipment;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        foreach(KeyValuePair<EquipmentType, EquipmentObject> entry in equipmentDict)
        {
            foreach(KeyValuePair<StatType, Stat> stat in statDict)
            { 
                stat.Value.RemoveModifier(entry.Value.modifierDict[stat.Key]);
            }
            foreach(KeyValuePair<ElementalProperty, Stat> stat in resistDict)
            { 
                stat.Value.RemoveModifier(entry.Value.resistDict[stat.Key]);
            }
        }  
        equipmentDict.Clear(); 
    }
}
