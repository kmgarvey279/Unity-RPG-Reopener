using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    MeleeWeapon,
    RangedWeapon,
    Armor, 
    Accessory
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public EquipmentType equipmentType;
    [Header("Equipment Stats")]
    [SerializeField] private int equipmentAttack;
    [SerializeField] private int equipmentDefense;
    [SerializeField] private int equipmentMagicAttack;
    [SerializeField] private int equipmentMagicDefense;
    [Header("Stat Modifiers")]
    [SerializeField] private int hpModifier;
    [SerializeField] private int mpModifier;
    [SerializeField] private int attackModifier;
    [SerializeField] private int defenseModifier;
    [SerializeField] private int magicAttackModifier;
    [SerializeField] private int magicDefenseModifier;
    [SerializeField] private int skillModifier;
    [SerializeField] private int agilityModifier;
    public Dictionary<StatType, int> modifierDict = new Dictionary<StatType, int>();
    [Header("Elemental Resistances")]
    [SerializeField] private int fireResistance;
    [SerializeField] private int iceResistance;
    [SerializeField] private int electricResistance;
    [SerializeField] private int darkResistance;
    public Dictionary<ElementalProperty, int> resistDict = new Dictionary<ElementalProperty, int>();
    [Header("Who can equip it?")]
    [SerializeField] private bool claireEquip;
    [SerializeField] private bool mutinyEquip;
    [SerializeField] private bool shadEquip;
    [SerializeField] private bool blaineEquip;
    [SerializeField] private bool lucyEquip;
    public Dictionary<PlayableCharacterID, bool> equipableDict = new Dictionary<PlayableCharacterID, bool>();

    
    public virtual void OnEnable()
    {
        itemType = ItemType.Equipment;
        //equipment stats
        modifierDict.Add(StatType.EquipmentAttack, equipmentAttack);
        modifierDict.Add(StatType.EquipmentDefense, equipmentDefense);
        modifierDict.Add(StatType.EquipmentMagicAttack, equipmentMagicAttack);
        modifierDict.Add(StatType.EquipmentMagicDefense, equipmentMagicDefense);
        //stat modifiers
        modifierDict.Add(StatType.HP, hpModifier);
        modifierDict.Add(StatType.MP, mpModifier);
        modifierDict.Add(StatType.Attack, attackModifier);
        modifierDict.Add(StatType.Defense, defenseModifier);
        modifierDict.Add(StatType.MagicAttack, magicAttackModifier);
        modifierDict.Add(StatType.MagicDefense, magicDefenseModifier);
        modifierDict.Add(StatType.Skill, skillModifier);
        modifierDict.Add(StatType.Agility, agilityModifier);
        //elemental resistance
        resistDict.Add(ElementalProperty.Fire, fireResistance);
        resistDict.Add(ElementalProperty.Ice, iceResistance);
        resistDict.Add(ElementalProperty.Electric, electricResistance);
        resistDict.Add(ElementalProperty.Dark, darkResistance);
        //characters who can equip
        equipableDict.Add(PlayableCharacterID.Claire, claireEquip);
        equipableDict.Add(PlayableCharacterID.Mutiny, mutinyEquip);
        equipableDict.Add(PlayableCharacterID.Shad, shadEquip);
        equipableDict.Add(PlayableCharacterID.Blaine, blaineEquip);
        equipableDict.Add(PlayableCharacterID.Lucy, lucyEquip);
    }

    public override void Use()
    {
        base.Use();
        // EquipmentManager.Equip(this);
    }

    private void OnDisable()
    {
        modifierDict.Clear();
        resistDict.Clear();
        equipableDict.Clear(); 
    }
}


