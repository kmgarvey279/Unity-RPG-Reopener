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

    public override void OnEnable()
    {
        base.OnEnable();
        equipmentDict.Add(EquipmentType.Weapon, weapon);
        equipmentDict.Add(EquipmentType.Armor, armor);
        equipmentDict.Add(EquipmentType.Accessory, accessory);
    }

    public void ChangeEquipment(EquipmentType equipmentType, EquipmentObject newEquipment)
    {
        //check if anything is currently equiped
        if(equipmentDict[equipmentType] != null)
        {
            attack.RemoveModifier(newEquipment.attackModifier);
            defense.RemoveModifier(newEquipment.defenseModifier);
            special.RemoveModifier(newEquipment.specialModifier);
        }
        //assign new equipment to selected slot (or set to null if just removing equipment)
        equipmentDict[equipmentType] = newEquipment;
        if(newEquipment != null)
        {
            attack.AddModifier(newEquipment.attackModifier);
            defense.AddModifier(newEquipment.defenseModifier);
            special.AddModifier(newEquipment.specialModifier);
        }
    }
}
