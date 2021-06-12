using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    [SerializeField] private int healthModifier;
    [SerializeField] private int manaModifier;
    [SerializeField] private int attackModifier;
    [SerializeField] private int defenseModifier;
    [SerializeField] private int specialModifier;
    public Dictionary<StatType, int> modifierDict = new Dictionary<StatType, int>();
    
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


