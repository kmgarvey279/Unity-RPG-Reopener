using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Items/Equipment/Armor")]
public class Armor : EquipmentObject
{
    public override void OnEnable()
    {
        base.OnEnable();
        equipmentType = EquipmentType.Armor;
    }
}