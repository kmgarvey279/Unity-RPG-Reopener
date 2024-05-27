using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayableCharacterID
{
    Claire,
    Mutiny,
    Blaine,
    Shad,
    Lucy,
    Oshi
}

[CreateAssetMenu(fileName = "New Playable Character Info", menuName = "Character Info/Playable")]
public class PlayableCharacterInfo : CharacterInfo
{
    //[field: SerializeField] public int GuardLimit { get; protected set; }
    [field: SerializeField] public int InterventionLimit { get; private set; }
    //private List<int> expToNextLookupPlaceholder = new List<int>();
    [field: SerializeField] public int EXP { get; protected set; } 
    private int placeholderExpToNext = 10;

    [SerializeField] private EquipmentItem defaultWeapon;
    [SerializeField] private EquipmentItem defaultArmor;
    [SerializeField] private EquipmentItem defaultAccessory;
    public Dictionary<EquipmentType, EquipmentItem> EquipDict;
    [field: SerializeField] public PlayableCharacterID PlayableCharacterID { get; private set; }
    [field: SerializeField] public Color CharacterColor { get; private set; }
    [field: SerializeField] public Attack Attack { get; private set; }
    [field: SerializeField] public Action Defend { get; private set; }
    [field: SerializeField] public List<Action> Skills { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        EquipDict = new Dictionary<EquipmentType, EquipmentItem>()
        {
            { EquipmentType.Weapon, null},
            { EquipmentType.Armor, null},
            { EquipmentType.Accessory, null}
        };

        if (defaultWeapon != null)
        {
            ChangeEquipment(EquipmentType.Weapon, defaultWeapon);
        }
        if (defaultArmor != null)
        {
            ChangeEquipment(EquipmentType.Armor, defaultArmor);
        }
        if (defaultAccessory != null)
        {
            ChangeEquipment(EquipmentType.Accessory, defaultAccessory);
        }
        //int expForThisLevel = baseExpToNext;
        //for (int i = 0 i < 100; i++)
        //{

        //    expForThisLevel =  Mathf.CeilToInt(expForThisLevel + (expForThisLevel * 1.5f));
        //}
    }

    public int GetEXPToNextLevel()
    {
        return Mathf.Clamp(placeholderExpToNext - EXP, 0, placeholderExpToNext);
    }

    public void ChangeEquipment(EquipmentType slotType, EquipmentItem newEquipment)
    {
        //clear previous
        //if (EquipDict[slotType] != null)
        //{
        //    foreach (IntStatModifier intStatModifier in EquipDict[slotType].IntStatModifiers)
        //    {
        //        RemoveIntStatModifier(intStatModifier.IntStatType, intStatModifier.Modifier, intStatModifier.ModifierType);
        //    }
        //}

        //if (newEquipment != null)
        //{
        //    //add new
        //    foreach (IntStatModifier intStatModifier in newEquipment.IntStatModifiers)
        //    {
        //        ApplyIntStatModifier(intStatModifier.IntStatType, intStatModifier.Modifier, intStatModifier.ModifierType);
        //    }
        //}

        ////assign to slot
        //EquipDict[slotType] = newEquipment;
    }
}
