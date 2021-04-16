using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character Info", menuName = "Character Info/Playable")]
public class PlayableCharacterInfo : CharacterInfo
{
    [Header("Equipment")]
    public EquipmentObject weapon;
    public EquipmentObject armor;
    public EquipmentObject accessory;
    public Dictionary<EquipmentType, EquipmentObject> equipmentDict = new Dictionary<EquipmentType, EquipmentObject>();

    protected override void OnEnable()
    {
        base.OnEnable();
        equipmentDict.Add(EquipmentType.Weapon, weapon);
        equipmentDict.Add(EquipmentType.Armor, armor);
        equipmentDict.Add(EquipmentType.Accessory, accessory);
    }

    public void ChangeEquipment(EquipmentType equipmentType, EquipmentObject newEquipment)
    {
        //check if anything is currently equipped
        if(equipmentDict[equipmentType] != null)
        {
            //go through each stat stored in character's stat dictionary
            foreach(KeyValuePair<StatType, Stat> stat in statDict)
            { 
                stat.Value.RemoveModifier(equipmentDict[equipmentType].modifierDict[stat.Key]);
            }
            
            equipmentDict[equipmentType] = null;
        }
        //assign new equipment to selected slot (or set to null if just removing equipment)
        if(newEquipment != null)
        {
            //go through each stat stored in character's stat dictionary
            foreach(KeyValuePair<StatType, Stat> stat in statDict)
            { 
                stat.Value.AddModifier(newEquipment.modifierDict[stat.Key]);
            }
            
            equipmentDict[equipmentType] = newEquipment;
        }
    }
}
