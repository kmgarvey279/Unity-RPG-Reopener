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

[CreateAssetMenu(fileName = "New Playable Character Info", menuName = "Character Info/Playable")]
public class PlayableCharacterInfo : CharacterInfo
{
    public PlayableCharacterID characterID;
    [Header("Equipment")]
    [SerializeField] private WeaponObject weapon;
    [SerializeField] private ArmorObject armor;
    [SerializeField] private AccessoryObject accessory;
    public Dictionary<EquipmentType, EquipmentObject> equipment = new Dictionary<EquipmentType, EquipmentObject>();

    protected override void OnEnable()
    {
        base.OnEnable();
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
        if(equipment[equipmentType] != null)
        {
            foreach(KeyValuePair<StatType, Stat> stat in statDict)
            { 
                stat.Value.RemoveModifier(equipment[equipmentType].modifierDict[stat.Key]);
            }
            equipment.Remove(equipmentType);
        }
        foreach(KeyValuePair<StatType, Stat> stat in statDict)
        { 
            stat.Value.AddModifier(newEquipment.modifierDict[stat.Key]);
        }            
        equipment[equipmentType] = newEquipment;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        foreach(KeyValuePair<EquipmentType, EquipmentObject> entry in equipment)
        {
            foreach(KeyValuePair<StatType, Stat> stat in statDict)
            { 
                stat.Value.RemoveModifier(entry.Value.modifierDict[stat.Key]);
            }
        }  
        equipment.Clear(); 
    }
}
