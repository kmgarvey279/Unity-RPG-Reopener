using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public Inventory inventory;

    public void AddItem(ItemObject item)
    {
        //check if player already has at least one of the same item
        int matchInInventory = ContainsItem(item);
        //add to number held in same slot & update in UI
        if(matchInInventory > 0)
        {
            int index = inventory.itemList.IndexOf(item);
            inventory.itemList[matchInInventory].numHeld++;
            inventoryUI.UpdateSlot(index);
        } 
        //otherwise, assign item to new slot & create UI
        else
        {
            inventory.itemList.Add(item);
            inventoryUI.CreateSlot(inventory.itemList.Count + 1);
        }
    }

    public void RemoveItem(ItemObject item)
    {
        //get index of item to remove to find counterpart slot
        int index = inventory.itemList.IndexOf(item);
        //remove item from items list
        inventory.itemList.Remove(item);
        //update UI
        inventoryUI.DestroySlot(index);
    }

    public void SwapItems(ItemObject item1, ItemObject item2)
    {
        //get index of items
        int index1 = inventory.itemList.IndexOf(item1);
        int index2 = inventory.itemList.IndexOf(item1);
        //store value of item 1
        ItemObject temp = inventory.itemList[index1];
        //swap values
        inventory.itemList[index1] = inventory.itemList[index2];
        inventory.itemList[index2] = temp;
        //update UI
        inventoryUI.UpdateSlot(index1);
        inventoryUI.UpdateSlot(index2);
    }

    //check if player has item in inventory list
    public int ContainsItem(ItemObject newItem)
    {
        for (var i = 0; i < inventory.itemList.Count; i++)
        {
            if(inventory.itemList[i].itemName == newItem.itemName)
            {
                return i;
            }       
        }
        return -1;
    }
}
