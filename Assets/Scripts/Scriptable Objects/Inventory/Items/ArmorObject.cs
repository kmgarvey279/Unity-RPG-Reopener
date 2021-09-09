using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorObject : EquipmentObject
{
    public int meleeDefense;
    public int rangedDefense;
    public int magicDefense;
    [Header("Elemental Resistances")]
    [SerializeField] private int fireResistance;
    [SerializeField] private int iceResistance;
    [SerializeField] private int electricResistance;
    [SerializeField] private int voidResistance;
    public Dictionary<ElementalProperty, int> resistDict = new Dictionary<ElementalProperty, int>();

    public override void OnEnable()
    {
        equipmentType = EquipmentType.Armor;
    }

    void Start()
    {
        resistDict.Add(ElementalProperty.Fire, fireResistance);
        resistDict.Add(ElementalProperty.Ice, iceResistance);
        resistDict.Add(ElementalProperty.Electric, electricResistance);
        resistDict.Add(ElementalProperty.Void, voidResistance);
    }
}
