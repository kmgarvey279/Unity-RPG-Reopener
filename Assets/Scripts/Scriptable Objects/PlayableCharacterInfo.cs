using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character Info", menuName = "Character Info/Playable")]
public class PlayableCharacterInfo : CharacterInfo
{
    [Header("Equipment")]
    public Weapon weapon;
    public Armor armor;
    public Accessory accessory;
    public Dictionary<EquipmentType, EquipmentObject> equipmentDict;

    public override void OnEnable()
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
            attack.RemoveModifier(equipmentDict[equipmentType].attackModifier);
            defense.RemoveModifier(equipmentDict[equipmentType].defenseModifier);
            special.RemoveModifier(equipmentDict[equipmentType].specialModifier);
            
            equipmentDict[equipmentType] = null;
        }
        //assign new equipment to selected slot (or set to null if just removing equipment)
        if(newEquipment != null)
        {
            // maxHealth.AddModifier(newEquipment.healthModifier)
            // maxMana.AddModifier(newEquipment.manaModifier)
            attack.AddModifier(newEquipment.attackModifier);
            defense.AddModifier(newEquipment.defenseModifier);
            special.AddModifier(newEquipment.specialModifier);
            
            equipmentDict[equipmentType] = newEquipment;
        }
    }
}
