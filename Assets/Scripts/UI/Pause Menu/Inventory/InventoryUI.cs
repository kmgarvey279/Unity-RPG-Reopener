using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public ItemMasterList itemMasterList;
    public Inventory inventory; 
    public GameObject slotPrefab;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public TextMeshProUGUI itemText;

    public virtual void Start()
    {   
        //for each item in inventory
        foreach(KeyValuePair<string,int> item in inventory.itemDict)
        {
            CreateSlot(itemMasterList.masterDict[item.Key], item.Value);
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(slots[0].gameObject);
        itemText.text = slots[0].item.itemDescription;
    }

    public void CreateSlot(ItemObject itemObject, int numHeld)
    {
        //create a new slot gameobject
        GameObject newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
        InventorySlot inventorySlot = newSlot.GetComponent<InventorySlot>();
        slots.Add(inventorySlot);
        //assign the item to it
        inventorySlot.AssignSlot(itemObject, numHeld);
    }

    public virtual void FilterSlots(int enumIndex)
    {
        ClearFilter();
        ItemType itemType = (ItemType)enumIndex;
        foreach (InventorySlot slot in slots)
        {
            if(slot.item.itemType != itemType)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    public void ClearFilter()
    {
        foreach (InventorySlot slot in slots)
        {
            if(!slot.gameObject.activeInHierarchy)
            {
                slot.gameObject.SetActive(true);
            }
        }
    }
}
