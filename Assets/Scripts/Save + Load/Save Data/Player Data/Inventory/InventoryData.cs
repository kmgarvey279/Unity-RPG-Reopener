using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemFilterType
{
    All,
    Usable,
    Equipment,
    Key
}

[System.Serializable]
public class InventoryData
{
    public List<InventoryItem> EquipmentItems = new List<InventoryItem>();
    public List<InventoryItem> KeyItems = new List<InventoryItem>();
    public List<InventoryItem> UsableItems = new List<InventoryItem>();

    private Dictionary<ItemType, List<InventoryItem>> inventoryItems
    {
        get 
        {
            return new Dictionary<ItemType, List<InventoryItem>>()
            {
                { ItemType.Equipment, EquipmentItems },
                { ItemType.Usable, KeyItems },
                { ItemType.Key, UsableItems }
            };
        }
    }

    public void AddItem(Item itemToAdd)
    {
        if (itemToAdd == null)
        {
            return;
        }

        InventoryItem match = inventoryItems[itemToAdd.ItemType].FirstOrDefault(i => i.Item == itemToAdd);
        if (match == null)
        {
            inventoryItems[itemToAdd.ItemType].Add(new InventoryItem(itemToAdd, 1));
        }
        else
        {
            match.Count++;
        }
    }

    //remove item from list
    public void RemoveItem(Item itemToRemove)
    {
        InventoryItem match = inventoryItems[itemToRemove.ItemType].FirstOrDefault(i => i.Item == itemToRemove);
        if (match == null)
        {
            return;
        }
        else
        {
            if (match.Count == 1)
            {
                inventoryItems[itemToRemove.ItemType].Remove(match);
            }
            else
            {
                match.Count--;
            }
        }
    }

    public List<InventoryItem> GetItems(ItemFilterType itemFilterType)
    {
        List<InventoryItem> itemsToReturn = new List<InventoryItem>();
        if (itemFilterType == ItemFilterType.All || itemFilterType == ItemFilterType.Usable)
        {
            itemsToReturn.AddRange(UsableItems);
        }
        if (itemFilterType == ItemFilterType.All || itemFilterType == ItemFilterType.Equipment)
        {
            itemsToReturn.AddRange(EquipmentItems);
        }
        if (itemFilterType == ItemFilterType.All || itemFilterType == ItemFilterType.Key)
        {
            itemsToReturn.AddRange(KeyItems);
        }
        return itemsToReturn;
    }

    public List<InventoryItem> GetFilteredEquipment(EquipmentType equipmentType, PlayableCharacterID playableCharacterID)
    {
        List<InventoryItem> itemsToReturn = new List<InventoryItem>();

        foreach (InventoryItem inventoryItem in EquipmentItems)
        {
            if (inventoryItem.Item && inventoryItem.Item is EquipmentItem)
            {
                EquipmentItem equipmentItem = (EquipmentItem)inventoryItem.Item;
                if (equipmentItem.EquipmentType == equipmentType && (!equipmentItem.CharacterExclusive || equipmentItem.ExclusiveCharacters.Contains(playableCharacterID)))
                {
                    itemsToReturn.Add(inventoryItem);
                }
            }
        }
        return itemsToReturn;
    }

    public bool PickupCheck(Item itemToAdd)
    {
        InventoryItem match = inventoryItems[itemToAdd.ItemType].FirstOrDefault(i => i.ItemID == itemToAdd.ItemID);
        if (match == null || match.Count < itemToAdd.CarryMax)
        {
            return true;
        }
        return false;
    }

    //public void OnDeserialize()
    //{
    //    if (InventoryItems == null)
    //    {
    //        InventoryItems = new Dictionary<ItemType, List<SerializableInventoryItem>>()
    //        {
    //            { ItemType.Equipment, new List<SerializableInventoryItem>() },
    //            { ItemType.Usable, new List<SerializableInventoryItem>() },
    //            { ItemType.Key, new List<SerializableInventoryItem>() }
    //        };
    //    }
    //}

    //public void SetData(Inventory inventory)
    //{
    //    InventoryItems = inventory.GetSerializedInventoryItems();
    //}

    //public SerializableInventoryData()
    //{
    //    nventoryItems = new List<SerializableInventoryItem>();
    //}
}
