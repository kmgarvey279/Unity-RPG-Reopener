using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    //[SerializeField] private Inventory inventory; 
    //[SerializeField] private GameObject slotPrefab;
    //private List<InventorySlot> slots = new List<InventorySlot>();
    //[SerializeField] private TextMeshProUGUI itemText;

    //public void Start()
    //{   
    //    //for each item in inventory
    //    foreach(InventoryItem inventoryItem in inventory.GetAll())
    //    {
    //        CreateSlot(inventoryItem.Item, inventoryItem.Count);
    //    }
    //    EventSystem.current.SetSelectedGameObject(null);
    //    EventSystem.current.SetSelectedGameObject(slots[0].gameObject);
    //    itemText.text = slots[0].Item.ItemDescription;
    //}

    //public void CreateSlot(Item item, int numHeld)
    //{
    //    //create a new slot gameobject
    //    GameObject newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
    //    InventorySlot inventorySlot = newSlot.GetComponent<InventorySlot>();
    //    slots.Add(inventorySlot);
    //    //assign the item to it
    //    inventorySlot.AssignSlot(item, numHeld);
    //}

    //public void FilterSlots(int enumIndex)
    //{
    //    ClearFilter();
    //    ItemType itemType = (ItemType)enumIndex;
    //    foreach (InventorySlot slot in slots)
    //    {
    //        if(slot.Item.ItemType != itemType)
    //        {
    //            slot.gameObject.SetActive(false);
    //        }
    //    }
    //}

    //public void ClearFilter()
    //{
    //    foreach (InventorySlot slot in slots)
    //    {
    //        if(!slot.gameObject.activeInHierarchy)
    //        {
    //            slot.gameObject.SetActive(true);
    //        }
    //    }
    //}
}
