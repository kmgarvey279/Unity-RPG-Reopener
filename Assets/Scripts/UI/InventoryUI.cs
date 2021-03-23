using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; 
    public GameObject slotPrefab;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public GameObject firstButton;

    private void Start()
    {   
        //for each item in inventory
        for(int i = 0; i < inventory.itemList.Count; i++)
        {
            CreateSlot(i);
        }
        firstButton = slots[0].gameObject;
    }

    public void CreateSlot(int index)
    {
        //create a new slot gameobject
        GameObject newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
        InventorySlot inventorySlot = newSlot.GetComponent<InventorySlot>();
        slots.Add(inventorySlot);
        // //assign the item to it
        inventorySlot.AssignSlot(inventory.itemList[index]);
    }

    public void UpdateSlot(int index)
    {
        slots[index].AssignSlot(inventory.itemList[index]);
    }

    public void DestroySlot(int index)
    {
        slots[index].ClearSlot();
    }
}
