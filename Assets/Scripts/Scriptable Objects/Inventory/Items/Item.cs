using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Usable,
    Equipment,
    Key
}

public class Item: ScriptableObject
{
    public string ItemID { private set; get; } = System.Guid.NewGuid().ToString();

    [HideInInspector] public ItemType ItemType { get; protected set; }
    [field: SerializeField] public string ItemName { get; protected set; } = "Item Name";
    [field: SerializeField, TextArea(2, 6)] public string Description { get; protected set; }
    [field: SerializeField, TextArea(2, 6)] public string SecondaryDescription { get; protected set; }
    [field: SerializeField] public Sprite ItemIcon { get; protected set; } = null;
    [field: SerializeField] public bool CanSell { get; protected set; } = false;
    [field: SerializeField] public int SellValue { get; protected set; } = 0;
    [field: SerializeField] public bool CanDiscard { get; protected set; } = false;
    [field: SerializeField] public int CarryMax { get; protected set; } = 99;
}
