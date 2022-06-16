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
    [Header("Stat Modifiers")]
    [SerializeField] private int hpModifier = 0;
    [SerializeField] private int mpModifier = 0;
    [SerializeField] private int attackModifier = 0;
    [SerializeField] private int defenseModifier = 0;
    [SerializeField] private int magicAttackModifier = 0;
    [SerializeField] private int magicDefenseModifier = 0;
    [SerializeField] private int skillModifier = 0;
    [SerializeField] private int agilityModifier = 0;
    public Dictionary<StatType, int> modifierDict = new Dictionary<StatType, int>();
    [Header("Elemental Resistances")]
    [SerializeField] private int fireResistance = 0;
    [SerializeField] private int iceResistance = 0;
    [SerializeField] private int electricResistance = 0;
    [SerializeField] private int darkResistance = 0;
    [SerializeField] private int lightResistance = 0;
    public Dictionary<ElementalProperty, int> resistDict = new Dictionary<ElementalProperty, int>();
    [Header("Who can equip it?")]
    [SerializeField] private bool claireEquip;
    [SerializeField] private bool mutinyEquip;
    [SerializeField] private bool shadEquip;
    [SerializeField] private bool blaineEquip;
    [SerializeField] private bool lucyEquip;
    [SerializeField] private bool oshiEquip;
    public Dictionary<PlayableCharacterID, bool> equipableDict = new Dictionary<PlayableCharacterID, bool>();

    
    public virtual void OnEnable()
    {
        itemType = ItemType.Equipment;
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
        resistDict.Add(ElementalProperty.None, 0);
        resistDict.Add(ElementalProperty.Fire, fireResistance);
        resistDict.Add(ElementalProperty.Ice, iceResistance);
        resistDict.Add(ElementalProperty.Electric, electricResistance);
        resistDict.Add(ElementalProperty.Dark, darkResistance);
        resistDict.Add(ElementalProperty.Light, lightResistance);
        //characters who can equip
        equipableDict.Add(PlayableCharacterID.Claire, claireEquip);
        equipableDict.Add(PlayableCharacterID.Mutiny, mutinyEquip);
        equipableDict.Add(PlayableCharacterID.Shad, shadEquip);
        equipableDict.Add(PlayableCharacterID.Blaine, blaineEquip);
        equipableDict.Add(PlayableCharacterID.Lucy, lucyEquip);
        equipableDict.Add(PlayableCharacterID.Oshi, oshiEquip);
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


