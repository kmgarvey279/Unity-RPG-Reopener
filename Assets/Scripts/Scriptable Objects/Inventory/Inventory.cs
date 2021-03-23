using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public List<ItemObject> itemList = new List<ItemObject>();

    //add item to inventory (or increase number held)
    public void AddItem(ItemObject item)
    {
        int matchInInventory = ContainsItem(item);
        if(matchInInventory < 0)
        {
            itemList.Add(item);
            matchInInventory = itemList.IndexOf(item);
            // inventoryUI.CreateSlot(inventory.itemList.Count + 1);
        }
        itemList[matchInInventory].numHeld++;
    }

    //remove item from list
    public void RemoveItem(ItemObject item)
    {
        int index = itemList.IndexOf(item);
        itemList.Remove(item);
        // inventoryUI.DestroySlot(index);
    }

    public void SwapItems(ItemObject item1, ItemObject item2)
    {
        int index1 = itemList.IndexOf(item1);
        int index2 = itemList.IndexOf(item1);

        ItemObject temp = itemList[index1];

        itemList[index1] = itemList[index2];
        itemList[index2] = temp;


        // inventoryUI.UpdateSlot(index1);
        // inventoryUI.UpdateSlot(index2);
    }

    //check if player has item in inventory list
    public int ContainsItem(ItemObject newItem)
    {
        for (var i = 0; i < itemList.Count; i++)
        {
            if(itemList[i].itemName == newItem.itemName)
            {
                return i;
            }       
        }
        return -1;
    }
}
