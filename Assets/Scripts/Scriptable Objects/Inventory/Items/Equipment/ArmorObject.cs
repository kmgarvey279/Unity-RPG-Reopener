using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorObject : EquipmentObject
{
    public override void OnEnable()
    {
        equipmentType = EquipmentType.Armor;
    }
}
