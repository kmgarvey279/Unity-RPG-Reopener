using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>();

    private void OnEnable()
    {
        itemDict.Add(0, 1);
        itemDict.Add(1, 2);
        itemDict.Add(2, 1);
        itemDict.Add(3, 1);
        itemDict.Add(4, 1);
        itemDict.Add(5, 1);
    }

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
    public void AddItem(int itemId)
    {   
        if(itemDict.ContainsKey(itemId))
        {
            itemDict.Add(itemId, 1);
            // inventoryUI.CreateSlot(inventory.itemList.Count + 1);
        }
        else 
        {
            itemDict[itemId]++;
        }
    }

    //remove item from list
    public void RemoveItem(int itemId)
    {
        itemDict.Remove(itemId);
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
