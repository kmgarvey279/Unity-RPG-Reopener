using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentManager : MonoBehaviour
{
    [Header("Inventory")]
    public InventoryManager inventoryManager;
    [Header("Character Equipment")]
    public PlayableCharacterInfo characterInfo;
    //events
    public delegate void OnEquipmentChanged(EquipmentObject newEquipment, EquipmentObject oldEquipment);
    public OnEquipmentChanged onEquipmentChanged;

    public void Equip(EquipmentObject newEquipment)
    {
        EquipmentType equipmentType = newEquipment.equipmentType;
        //check if anything is already equipped in slot, if so, return to inventory
        if(characterInfo.equipmentDict[equipmentType] != null)
        {
            inventoryManager.AddItem(characterInfo.equipmentDict[equipmentType]);
        }
        //invoke event
        characterInfo.ChangeEquipment(equipmentType, newEquipment);
    }

    public void Unequip(EquipmentType equipmentType)
    {
        //clear equipment from slot and return to inventory
        EquipmentObject oldEquipment = characterInfo.equipmentDict[equipmentType];
        inventoryManager.AddItem(oldEquipment);
        //invoke event
        characterInfo.ChangeEquipment(equipmentType, null);
    }

    public void UnequipAll()
    {
        foreach(var key in characterInfo.equipmentDict.Keys)
        {
            Unequip(key);
        }
    }

    private void Update()
    {
        //check for input
    }
}
