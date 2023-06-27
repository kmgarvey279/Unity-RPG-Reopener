using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Items/Equipable/Weapon")]
public class WeaponItem : EquipmentItem
{
    [Header("Who can equip it?")]
    [SerializeField] private bool claireEquip;
    [SerializeField] private bool mutinyEquip;
    [SerializeField] private bool shadEquip;
    [SerializeField] private bool blaineEquip;
    [SerializeField] private bool lucyEquip;
    [SerializeField] private bool oshiEquip;
    public Dictionary<PlayableCharacterID, bool> equipableDict { get; private set; } = new Dictionary<PlayableCharacterID, bool>();

    public override void OnEnable()
    {
        base.OnEnable();
        ItemType = ItemType.Weapon;
        //characters who can equip
        equipableDict.Add(PlayableCharacterID.Claire, claireEquip);
        equipableDict.Add(PlayableCharacterID.Mutiny, mutinyEquip);
        equipableDict.Add(PlayableCharacterID.Shad, shadEquip);
        equipableDict.Add(PlayableCharacterID.Blaine, blaineEquip);
        equipableDict.Add(PlayableCharacterID.Lucy, lucyEquip);
        equipableDict.Add(PlayableCharacterID.Oshi, oshiEquip);
    }
}
