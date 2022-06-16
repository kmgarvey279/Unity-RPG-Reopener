using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayableCharacterID
{
    Claire,
    Mutiny,
    Shad,
    Blaine,
    Lucy,
    Oshi
}

[CreateAssetMenu(fileName = "New Playable Character Info", menuName = "Character Info/Playable")]
public class PlayableCharacterInfo : CharacterInfo
{
    [Header("ID")]
    public PlayableCharacterID playableCharacterID;
    [Header("Skills")]
    public List<Action> skills;
    [Header("Equipment")]
    [SerializeField] private EquipmentObject weapon;
    [SerializeField] private EquipmentObject armor;
    [SerializeField] private EquipmentObject accessory;
    public Dictionary<EquipmentType, EquipmentObject> equipmentDict = new Dictionary<EquipmentType, EquipmentObject>()
    {
        {EquipmentType.Weapon, null},
        {EquipmentType.Armor, null},
        {EquipmentType.Accessory, null}
    };    
    
    protected override void OnEnable()
    {
        base.OnEnable();

        if(weapon != null)
            ChangeEquipment(weapon);
        if(armor != null)
            ChangeEquipment(armor);
        if(accessory != null)
            ChangeEquipment(accessory);
    }

    public void ChangeEquipment(EquipmentObject newEquipment)
    {
        EquipmentType equipmentType = newEquipment.equipmentType;
        //update stat modifiers
        if(equipmentDict[equipmentType] != null)
        {
            //clear stat modifiers
            foreach(KeyValuePair<StatType, Stat> statEntry in statDict)
            { 
                statEntry.Value.RemoveAdditive(equipmentDict[equipmentType].modifierDict[statEntry.Key]);
            }
            //clear elemental modifiers
            foreach(KeyValuePair<ElementalProperty, ElementalResistance> resistanceEntry in resistDict)
            { 
                resistanceEntry.Value.resistance += equipmentDict[equipmentType].resistDict[resistanceEntry.Key];
            }
            equipmentDict.Remove(equipmentType);
        }
        foreach(KeyValuePair<StatType, Stat> statEntry in statDict)
        { 
            statEntry.Value.AddAdditive(newEquipment.modifierDict[statEntry.Key]);
        }            
        foreach(KeyValuePair<ElementalProperty, ElementalResistance> resistanceEntry in resistDict)
        { 
            resistanceEntry.Value.resistance += newEquipment.resistDict[resistanceEntry.Key];
        }
        equipmentDict[equipmentType] = newEquipment;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        foreach(KeyValuePair<EquipmentType, EquipmentObject> equipmentEntry in equipmentDict)
        {
            if(equipmentEntry.Value != null)
            {
                foreach(KeyValuePair<StatType, Stat> statEntry in statDict)
                { 
                    statEntry.Value.RemoveAdditive(equipmentEntry.Value.modifierDict[statEntry.Key]);
                }
                foreach(KeyValuePair<ElementalProperty, ElementalResistance> resistanceEntry in resistDict)
                { 
                    resistanceEntry.Value.resistance -= equipmentEntry.Value.resistDict[resistanceEntry.Key];
                }
            }
        }  
    }
}
