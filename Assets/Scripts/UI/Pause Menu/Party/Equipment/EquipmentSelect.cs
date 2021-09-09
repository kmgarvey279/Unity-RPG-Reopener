using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//creates a list of equipment based on selected category
public class EquipmentSelect : MonoBehaviour
{
    //player inventory
    public ItemMasterList itemMasterList;
    public Inventory inventory;
    public PlayableCharacterInfo playableCharacterInfo;

    //filter
    private EquipmentType equipmentType;
    
    //item slots
    public GameObject slotPrefab;
    public List<InventorySlot> slots = new List<InventorySlot>();

    //selected equipment
    public EquipmentSlot selectedEquipmentSlot;
    public InventorySlot selectedInventorySlot; 
    public MenuTextbox menuTextbox;

    private void Start()
    {
        playableCharacterInfo = GetComponentInParent<CharacterInfoUI>().playableCharacterInfo;
    }

    //create list of item slots
    public void PopulateList(EquipmentSlot equipmentSlot)
    {   
        selectedEquipmentSlot = equipmentSlot;
        //clear previous list (if it exists) and item description
        // if(slots.Count > 0)
        // {
        //     ClearList();
        //     menuTextbox.UpdateText("");
        // }
        //create slots for equipment in category
        foreach(KeyValuePair<string,int> item in inventory.itemDict)
        {
            ItemObject itemObject = itemMasterList.masterDict[item.Key];
            if(itemObject.itemType == ItemType.Equipment && itemObject.equipmentType == selectedEquipmentSlot.equipmentType)
            {
                CreateSlot(itemMasterList.masterDict[item.Key], item.Value);
            }
        }
        //select first item in list (if it isn't empty)
        if(slots.Count > 0)
        {
            selectedInventorySlot = slots[0];
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(selectedInventorySlot.gameObject);
            menuTextbox.UpdateText(selectedInventorySlot.item.itemDescription);
        }
    }

    //create new slot
    public void CreateSlot(ItemObject itemObject, int numHeld)
    {
        //create a new slot gameobject
        GameObject newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
        InventorySlot inventorySlot = newSlot.GetComponent<InventorySlot>();
        slots.Add(inventorySlot);
        //assign the item to it
        inventorySlot.AssignSlot(itemObject, numHeld);
    }

    public void OnSelectionChange()
    {
        selectedInventorySlot = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>();
        menuTextbox.UpdateText(selectedInventorySlot.item.itemDescription);
    }

    public void OnConfirmEquipmentChange()
    {
        selectedInventorySlot = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>();
        playableCharacterInfo.ChangeEquipment((EquipmentObject)selectedInventorySlot.item);
        selectedEquipmentSlot.AssignSlot(selectedInventorySlot.item);
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ClearList();
    }

    private void ClearList()
    {
        selectedInventorySlot = null;
        foreach (InventorySlot slot in slots)
        {
            slot.ClearSlot();           
        }
        slots.Clear();
        menuTextbox.UpdateText("");
    }
}
