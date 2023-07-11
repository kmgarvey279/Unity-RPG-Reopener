using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}

public class EquipmentItem : Item
{
    [Header("Stat Modifiers")]
    [SerializeField] protected int hpModifier = 0;
    [SerializeField] protected int mpModifier = 0;
    [SerializeField] protected int attackModifier = 0;
    [SerializeField] protected int defenseModifier = 0;
    [SerializeField] protected int mAttackModifier = 0;
    [SerializeField] protected int mDefenseModifier = 0;
    [SerializeField] protected int agilityModifier = 0;
    [SerializeField] protected int healingModifier = 0;

    [SerializeField] protected float critRateModifier = 0;
    [SerializeField] protected float critPowerModifier = 0;
    [SerializeField] protected float evadeRateModifier = 0;
    [SerializeField] protected float blockRateModifier = 0;
    [SerializeField] protected float blockPowerModifier = 0;
    [Header("Elemental Resistances")]
    [SerializeField] protected int fireResistance = 0;
    [SerializeField] protected int iceResistance = 0;
    [SerializeField] protected int electricResistance = 0;
    [SerializeField] protected int darkResistance = 0;

    [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
    [field: SerializeField] public bool CharacterExclusive { get; private set; }
    [field: SerializeField] public List<PlayableCharacterID> ExclusiveCharacters { get; private set; }
    public Dictionary<StatType, int> StatModifiers { get; protected set; }
    public Dictionary<SecondaryStatType, float> SecondaryStatModifiers { get; protected set; }
    public Dictionary<ElementalProperty, int> ResistanceModifiers { get; protected set; }
    
    public virtual void OnEnable()
    {
        //stat modifiers
        StatModifiers = new Dictionary<StatType, int>();
        StatModifiers.Add(StatType.HP, hpModifier);
        StatModifiers.Add(StatType.MP, mpModifier);
        StatModifiers.Add(StatType.Attack, attackModifier);
        StatModifiers.Add(StatType.Defense, defenseModifier);
        StatModifiers.Add(StatType.MAttack, mAttackModifier);
        StatModifiers.Add(StatType.MDefense, mDefenseModifier);
        StatModifiers.Add(StatType.Agility, agilityModifier);

        SecondaryStatModifiers = new Dictionary<SecondaryStatType, float>();
        SecondaryStatModifiers.Add(SecondaryStatType.CritRate, critRateModifier);
        SecondaryStatModifiers.Add(SecondaryStatType.CritPower, critPowerModifier);
        SecondaryStatModifiers.Add(SecondaryStatType.EvadeRate, evadeRateModifier);
        SecondaryStatModifiers.Add(SecondaryStatType.BlockRate, blockRateModifier);
        SecondaryStatModifiers.Add(SecondaryStatType.BlockPower, blockPowerModifier);
        //elemental resistance
        ResistanceModifiers = new Dictionary<ElementalProperty, int>();
        ResistanceModifiers.Add(ElementalProperty.None, 0);
        ResistanceModifiers.Add(ElementalProperty.Fire, fireResistance);
        ResistanceModifiers.Add(ElementalProperty.Ice, iceResistance);
        ResistanceModifiers.Add(ElementalProperty.Electric, electricResistance);
        ResistanceModifiers.Add(ElementalProperty.Dark, darkResistance);
    }
}


