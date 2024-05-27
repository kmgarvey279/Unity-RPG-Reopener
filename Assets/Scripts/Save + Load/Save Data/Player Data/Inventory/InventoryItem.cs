using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public int Count;

    [Newtonsoft.Json.JsonIgnore] public Item Item;
    public string ItemID
    {
        get
        {
            return Item.ItemID;
        }
        set
        {
            Item = DatabaseDirectory.Instance.ItemDatabase.LookupDictionary[value];
        }
    }

    public InventoryItem(Item item, int count)
    {
        Item = item;
        Count = count;
    }
}
