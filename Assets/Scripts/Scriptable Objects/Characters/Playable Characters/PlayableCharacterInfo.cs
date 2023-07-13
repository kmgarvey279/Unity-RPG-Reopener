using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayableCharacterID
{
    Claire,
    Mutiny,
    Shad,
    Blaine,
    Lucy,
    Oshi,
    None
}

[CreateAssetMenu(fileName = "New Playable Character Info", menuName = "Character Info/Playable")]
public class PlayableCharacterInfo : CharacterInfo
{
    [field: SerializeField] private EquipmentItem weapon;
    [field: SerializeField] private EquipmentItem armor;
    [field: SerializeField] private EquipmentItem accessory;
    [field: SerializeField] public PlayableCharacterID PlayableCharacterID { get; private set; }
    public Dictionary<EquipmentType, EquipmentItem> Equipment { get; private set; }
    [field: SerializeField] public Action Attack { get; private set; }
    [field: SerializeField] public Action Defend { get; private set; }
    [field: SerializeField] public List<Action> Skills { get; private set; }

    private float baseCritRate = 0.07f;
    private float baseCritPower = 0.5f;
    private float baseEvadeRate = 0.07f;
    private float baseBlockPower = 0.2f;

    protected override void OnEnable()
    {
        base.OnEnable();

        SecondaryStats[SecondaryStatType.CritRate].UpdateBaseValue(baseCritRate);
        SecondaryStats[SecondaryStatType.CritPower].UpdateBaseValue(baseCritPower);
        SecondaryStats[SecondaryStatType.EvadeRate].UpdateBaseValue(baseEvadeRate);
        SecondaryStats[SecondaryStatType.BlockPower].UpdateBaseValue(baseBlockPower);

        Equipment = new Dictionary<EquipmentType, EquipmentItem>();
        foreach (EquipmentType equipmentType in System.Enum.GetValues(typeof(EquipmentType)))
        {
            Equipment.Add(equipmentType, null);
        }
        if(weapon)
            ChangeEquipment(EquipmentType.Weapon, weapon);
        if(armor)
            ChangeEquipment(EquipmentType.Armor, armor);
        if (accessory)
            ChangeEquipment(EquipmentType.Accessory, accessory);
    }

    public void ChangeEquipment(EquipmentType equipmentType, EquipmentItem newEquipment)
    {
        //return if equipment is wrong type
        if(newEquipment.EquipmentType != equipmentType || (newEquipment.CharacterExclusive && !newEquipment.ExclusiveCharacters.Contains(PlayableCharacterID))) 
        {
            return;
        }
        //update stat modifiers
        if(Equipment[equipmentType] != null)
        {
            //clear modifiers from old equipment
            foreach(StatModifier statModifier in Equipment[equipmentType].StatModifiers)
            {
                Stats[statModifier.StatType].RemoveModifier(statModifier.Modifier, statModifier.ModifierType);
            }
            foreach (SecondaryStatModifier secondaryStatModifier in Equipment[equipmentType].SecondaryStatModifiers)
            {
                SecondaryStats[secondaryStatModifier.SecondaryStatType].RemoveModifier(secondaryStatModifier.Modifier, secondaryStatModifier.ModifierType);
            }
            foreach (KeyValuePair<ElementalProperty, int> modifier in Equipment[equipmentType].ResistanceModifiers)
            {
                ResistanceModifiers[modifier.Key].Remove(modifier.Value);
            }
        }
        //add equipment to dictionary
        Equipment[equipmentType] = newEquipment;
        //add modifiers from new equipment
        foreach (StatModifier statModifier in Equipment[equipmentType].StatModifiers)
        {
            Stats[statModifier.StatType].AddModifier(statModifier.Modifier, statModifier.ModifierType);
        }
        foreach (SecondaryStatModifier secondaryStatModifier in Equipment[equipmentType].SecondaryStatModifiers)
        {
            SecondaryStats[secondaryStatModifier.SecondaryStatType].AddModifier(secondaryStatModifier.Modifier, secondaryStatModifier.ModifierType);
        }
        foreach (KeyValuePair<ElementalProperty,int> modifier in Equipment[equipmentType].ResistanceModifiers)
        {
            ResistanceModifiers[modifier.Key].Add(modifier.Value);
        }
    }
}
