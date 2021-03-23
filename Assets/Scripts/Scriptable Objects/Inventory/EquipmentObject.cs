using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public EquipmentType equipmentType;
    public float attackModifier;
    public float defenseModifier;
    public float specialModifier;

    private void Start()
    {
        // equipmentManager = gameObject.GetComponent<EquipmentManager>();
    }

    public override void Use()
    {
        base.Use();
        // EquipmentManager.Equip(this);
    }
}


