using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class InventoryItem
{
    [field: SerializeField] public Item Item { get; private set; }
    [field: SerializeField] public int Count { get; private set; }
    public InventoryItem(Item _item, int _count)
    {
        Item = _item;
        Count = _count;
    }
    public void ChangeCount(int difference)
    {
        Count += difference;
    }
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryItem> keyItems = new List<InventoryItem>();
    [SerializeField] private List<InventoryItem> useItems = new List<InventoryItem>();
    [SerializeField] private List<InventoryItem> weaponitems = new List<InventoryItem>();
    [SerializeField] private List<InventoryItem> accessoryitems = new List<InventoryItem>();
    public Dictionary<ItemType, List<InventoryItem>> ItemDict { get; private set; } = new Dictionary<ItemType, List<InventoryItem>>();

    private void OnEnable()
    {
        ItemDict.Add(ItemType.Key, keyItems);
        ItemDict.Add(ItemType.Usable, useItems);
        ItemDict.Add(ItemType.Weapon, weaponitems);
        ItemDict.Add(ItemType.Accessory, accessoryitems);
    }

    public List<InventoryItem> GetAll()
    {
        List<InventoryItem> itemsToReturn = ItemDict[ItemType.Key];
        itemsToReturn.AddRange(ItemDict[ItemType.Usable]);
        itemsToReturn.AddRange(ItemDict[ItemType.Weapon]);
        itemsToReturn.AddRange(ItemDict[ItemType.Accessory]);
        return itemsToReturn;
    }
    //public Dictionary<ItemObject, int> itemDict = new Dictionary<ItemObject, int>();

    //private void OnEnable()
    //{
    //    foreach (Item item in items)
    //    {
    //        if(itemDict.ContainsKey(item))
    //        {
    //            itemDict[item]++;
    //        }
    //        else
    //        {
    //            itemDict.Add(item, 1);
    //        }
    //    }
    //}

    // public Dictionary<int, ItemObject> GetFullInventory()
    // {
    //     Dictionary<int, ItemObject> tempDict = new Dictionary<int, ItemObject>();
    //     foreach(KeyValuePair<int,int> item in itemDict)
    //     {
    //         if(masterDict[item.Key].GetType() is Weapon)
    //         {
    //             tempDict.Add(item.Key, item.Value);
    //         }
    //     }
    //     return tempDict;
    // } 

    // public Dictionary<ItemObject> GetWeapons()
    // {
    //     Dictionary<int, ItemObject> tempDict = new Dictionary<int, ItemObject>();
    //     foreach(KeyValuePair<int, int> item in itemDict)
    //     {
    //         if(item.Value.GetType() is Weapon)
    //         {
    //             tempDict.Add(item);
    //         }
    //     }
    //     return tempDict;
    // } 

    // public List<ItemObject> GetArmor()
    // {
    //     Dictionary<int, ItemObject> tempDict = new Dictionary<int, ItemObject>();
    //     foreach(KeyValuePair<int, ItemObject> item in itemDict)
    //     {
    //         if(item.Value.GetType() is Armor)
    //         {
    //             tempDict.Add(item);
    //         }
    //     }
    //     return tempDict;
    // } 

    // public List<ItemObject> GetAccessories()
    // {
    //     Dictionary<int, ItemObject> tempDict = new Dictionary<int, ItemObject>();
    //     foreach(KeyValuePair<int, ItemObject> item in itemDict)
    //     {
    //         if(item.Value.GetType() is Accessory)
    //         {
    //             tempDict.Add(item);
    //         }
    //     }
    //     return tempDict;
    // } 

    //add item to inventory (or increase number held)
    public void AddItem(Item itemToAdd)
    {
        InventoryItem match = ItemDict[itemToAdd.ItemType].FirstOrDefault(i => i.Item == itemToAdd);
        if (match == null)
        {
            ItemDict[itemToAdd.ItemType].Add(new InventoryItem(itemToAdd, 1));
        }
        else
        {
            match.ChangeCount(1);
        }
    }

    //remove item from list
    public void RemoveItem(Item itemToRemove)
    {
        InventoryItem match = ItemDict[itemToRemove.ItemType].FirstOrDefault(i => i.Item == itemToRemove);
        if (match == null)
        {
            return;
        }
        else
        {
            if(match.Count == 1)
            {
                ItemDict[itemToRemove.ItemType].Remove(match);
            }
            else
            {
                match.ChangeCount(-1);
            }
        }
        //itemDict.Remove(itemId);
        // inventoryUI.DestroySlot(index);
    }

    // public void SwapItems(int itemId1, int itemId2)
    // {
    //     int index1 = itemList.IndexOf(item1);
    //     int index2 = itemList.IndexOf(item1);

    //     ItemObject temp = itemDict[itemId1];

    //     itemDict[itemId1] = itemList[index2];
    //     itemList[index2] = temp;
    //     // inventoryUI.UpdateSlot(index1);
    //     // inventoryUI.UpdateSlot(index2);
    // }

    //check if player has item in inventory list
    // public int ContainsItem(ItemObject newItem)
    // {
    //     for (var i = 0; i < itemList.Count; i++)
    //     {
    //         if(itemList[i].itemName == newItem.itemName)
    //         {
    //             return i;
    //         }       
    //     }
    //     return -1;
    // }
}
