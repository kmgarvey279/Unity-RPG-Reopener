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
    [Header("Elemental Resistances")]
    [SerializeField] protected int fireResistance = 0;
    [SerializeField] protected int iceResistance = 0;
    [SerializeField] protected int electricResistance = 0;
    [SerializeField] protected int darkResistance = 0;

    [field: SerializeField] public int HPModifier { get; private set; }
    [field: SerializeField] public int MPModifier { get; private set; }
    [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
    [field: SerializeField] public bool CharacterExclusive { get; private set; }
    [field: SerializeField] public List<PlayableCharacterID> ExclusiveCharacters { get; private set; }
    [field: SerializeField] public List<StatModifier> StatModifiers { get; private set; } = new List<StatModifier>();
    [field: SerializeField] public List<SecondaryStatModifier> SecondaryStatModifiers { get; protected set; } = new List<SecondaryStatModifier>();
    public Dictionary<ElementalProperty, int> ResistanceModifiers { get; protected set; }
    
    public virtual void OnEnable()
    {
        //elemental resistance
        ResistanceModifiers = new Dictionary<ElementalProperty, int>();
        ResistanceModifiers.Add(ElementalProperty.None, 0);
        ResistanceModifiers.Add(ElementalProperty.Fire, fireResistance);
        ResistanceModifiers.Add(ElementalProperty.Ice, iceResistance);
        ResistanceModifiers.Add(ElementalProperty.Electric, electricResistance);
        ResistanceModifiers.Add(ElementalProperty.Dark, darkResistance);
    }
}


