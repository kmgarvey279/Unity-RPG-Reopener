using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    [Header("Stat Modifiers")]
    public Dictionary<StatType, int> modifierDict = new Dictionary<StatType, int>();
    [SerializeField] private int hpModifier;
    [SerializeField] private int mpModifier;
    [SerializeField] private int attackModifier;
    [SerializeField] private int skillModifier;
    [SerializeField] private int defenseModifier;
    [SerializeField] private int specialModifier;
    [SerializeField] private int agilityModifier;
    [Header("Who can equip it?")]
    public Dictionary<PlayableCharacterID, bool> equipableDict = new Dictionary<PlayableCharacterID, bool>();
    [SerializeField] private bool claireEquip;
    [SerializeField] private bool mutinyEquip;
    [SerializeField] private bool shadEquip;
    [SerializeField] private bool blaineEquip;
    [SerializeField] private bool lucyEquip;

    
    public virtual void OnEnable()
    {
        itemType = ItemType.Equipment;
        modifierDict.Add(StatType.HP, hpModifier);
        modifierDict.Add(StatType.MP, mpModifier);
        modifierDict.Add(StatType.Attack, attackModifier);
        modifierDict.Add(StatType.Skill, skillModifier);
        modifierDict.Add(StatType.Defense, defenseModifier);
        modifierDict.Add(StatType.Special, specialModifier);
        modifierDict.Add(StatType.Agility, agilityModifier);
    }

    public override void Use()
    {
        base.Use();
        // EquipmentManager.Equip(this);
    }
}


