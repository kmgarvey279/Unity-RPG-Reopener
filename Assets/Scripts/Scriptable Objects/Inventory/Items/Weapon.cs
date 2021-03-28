using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Items/Equipment/Weapon")]
public class Weapon : EquipmentObject
{
    public override void OnEnable()
    {
        base.OnEnable();
        equipmentType = EquipmentType.Weapon;
    }
}
