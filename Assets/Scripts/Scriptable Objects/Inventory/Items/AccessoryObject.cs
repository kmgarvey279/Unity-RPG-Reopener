using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryObject : EquipmentObject
{
    public override void OnEnable()
    {
        equipmentType = EquipmentType.Accessory;
    }
}
