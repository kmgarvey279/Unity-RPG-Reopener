using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAccessory", menuName = "Inventory/Items/Equipable/Accessory")]
public class AccessoryItem : EquipmentItem
{
    public override void OnEnable()
    {
        base.OnEnable();
        ItemType = ItemType.Accessory;
    }
}
