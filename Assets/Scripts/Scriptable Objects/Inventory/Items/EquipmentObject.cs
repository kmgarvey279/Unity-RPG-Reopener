using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}

public class EquipmentObject : ItemObject
{
    public float attackModifier;
    public float defenseModifier;
    public float specialModifier;
    public EquipmentType equipmentType;
    
    public virtual void OnEnable()
    {
        itemType = ItemType.Equipment;
    }

    public override void Use()
    {
        base.Use();
        // EquipmentManager.Equip(this);
    }
}


