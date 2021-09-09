using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : EquipmentObject
{
    public bool isMeleeWeapon;
    public int meleePower;
    public bool isRangedWeapon;
    public int rangedPower;
    public int magicPower;
    public AttackProperty attackProperty;

    public override void OnEnable()
    {
        equipmentType = EquipmentType.Weapon;
    }
}
