using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Equipment", menuName = "Inventory/Character Equipment")]
public class CharacterEquipment : ScriptableObject
{
    public EquipmentObject weapon;
    public EquipmentObject armor;
    public EquipmentObject accessory;
    public Dictionary<EquipmentType, EquipmentObject> equipmentDict = new Dictionary<EquipmentType, EquipmentObject>();
    
    private void OnEnable()
    {
        equipmentDict.Add(EquipmentType.Weapon, weapon);
        equipmentDict.Add(EquipmentType.Armor, armor);
        equipmentDict.Add(EquipmentType.Accessory, accessory);
    }
}
