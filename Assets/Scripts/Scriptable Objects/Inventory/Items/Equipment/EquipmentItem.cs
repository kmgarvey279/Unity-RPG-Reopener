using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory/Items/Equipment")]
public class EquipmentItem : Item
{
    [field: SerializeField ] public EquipmentType EquipmentType { get; protected set; }
    
    [field: Header("Characters")]
    [field: SerializeField] public bool CharacterExclusive;
    [field: SerializeField] public List<PlayableCharacterID> ExclusiveCharacters { get; protected set; } = new List<PlayableCharacterID>();

    [field: Header("Stats")]
    [field: SerializeField] public List<IntStatModifier> IntStatModifiers { get; protected set; } = new List<IntStatModifier>();

    [Header("Elemental Resistances")]
    [SerializeField] protected int slashResistance = 0;
    [SerializeField] protected int strikeResistance = 0;
    [SerializeField] protected int pierceResistance = 0;
    [SerializeField] protected int fireResistance = 0;
    [SerializeField] protected int iceResistance = 0;
    [SerializeField] protected int electricResistance = 0;
    [SerializeField] protected int darkResistance = 0;
    public Dictionary<ElementalProperty, int> ResistanceModifiers { get; protected set; } = new Dictionary<ElementalProperty, int>();

    public void OnEnable()
    {
        ItemType = ItemType.Equipment;

        ResistanceModifiers = new Dictionary<ElementalProperty, int>();
        ResistanceModifiers.Add(ElementalProperty.None, 0);
        ResistanceModifiers.Add(ElementalProperty.Slash, slashResistance);
        ResistanceModifiers.Add(ElementalProperty.Strike, strikeResistance);
        ResistanceModifiers.Add(ElementalProperty.Pierce, pierceResistance);
        ResistanceModifiers.Add(ElementalProperty.Fire, fireResistance);
        ResistanceModifiers.Add(ElementalProperty.Ice, iceResistance);
        ResistanceModifiers.Add(ElementalProperty.Electric, electricResistance);
        ResistanceModifiers.Add(ElementalProperty.Dark, darkResistance);
    }
}


