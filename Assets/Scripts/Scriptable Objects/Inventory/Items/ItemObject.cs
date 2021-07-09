using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Usable,
    Equipment,
    Collectable,
    Key
}
public enum EquipmentType
{
    None,
    Weapon,
    Armor, 
    Accessory
}
[CreateAssetMenu(fileName = "New Item Object", menuName = "Inventory/Items/Generic Item")]
public class ItemObject: ScriptableObject
{
    public string itemId = "New Id";
    public string itemName = "New Item";
    [TextArea(5,10)]
    public string itemDescription = "New Item";
    public Sprite icon = null;
    public ItemType itemType;
    public EquipmentType equipmentType;

    public virtual void Use(){}
}
