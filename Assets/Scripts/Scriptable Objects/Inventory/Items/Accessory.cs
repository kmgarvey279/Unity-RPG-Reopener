using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessory", menuName = "Inventory/Items/Equipment/Accessory")]
public class Accessory : EquipmentObject
{
    public override void OnEnable()
    {
        base.OnEnable();
        equipmentType = EquipmentType.Accessory;
    }
}
