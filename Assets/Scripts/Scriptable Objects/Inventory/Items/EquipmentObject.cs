using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    [SerializeField] private float healthModifier;
    [SerializeField] private float manaModifier;
    [SerializeField] private float attackModifier;
    [SerializeField] private float defenseModifier;
    [SerializeField] private float specialModifier;
    public Dictionary<StatType, float> modifierDict = new Dictionary<StatType, float>();
    
    public virtual void OnEnable()
    {
        itemType = ItemType.Equipment;
        modifierDict.Add(StatType.Health, healthModifier);
        modifierDict.Add(StatType.Mana, manaModifier);
        modifierDict.Add(StatType.Attack, attackModifier);
        modifierDict.Add(StatType.Defense, defenseModifier);
        modifierDict.Add(StatType.Special, specialModifier);
    }

    public override void Use()
    {
        base.Use();
        // EquipmentManager.Equip(this);
    }
}


