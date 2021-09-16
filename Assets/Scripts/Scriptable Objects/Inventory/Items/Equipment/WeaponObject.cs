using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : EquipmentObject
{
    public override void OnEnable()
    {
        equipmentType = EquipmentType.Weapon;
    }
}
