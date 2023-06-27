using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Key,
    Usable,
    Weapon, 
    Accessory
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items/Generic Item")]
public class Item: ScriptableObject
{
    [HideInInspector] public ItemType ItemType { get; protected set; }
    public string ItemName { get; protected set; } = "Item Name";
    public string ItemDescription { get; protected set; } = "Item Description";
    public Sprite ItemIcon { get; protected set; } = null;
    public bool CanSell { get; protected set; } = false;
    public int SellValue { get; protected set; } = 0;
    public bool CanDiscard { get; protected set; } = false;
}
