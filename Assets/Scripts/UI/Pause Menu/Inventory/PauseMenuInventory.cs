using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuInventory : PauseSubmenu
{
    [SerializeField] private ItemDatabase itemDatabase;
    [Header("Items")]
    [SerializeField] private ScrollableList itemList;
    [SerializeField] private ItemInfo itemInfo;
    [Header("Tabs")]
    [SerializeField] private Image useableTab;
    [SerializeField] private Image equipmentTab;
    [SerializeField] private Image keyItemTab;
    private Dictionary<ItemType, Image> tabs = new Dictionary<ItemType, Image>();
    [SerializeField] private Color activeTabColor;
    [SerializeField] private Color inactiveTabColor;

    private ItemType displayedItemType;

    protected override void Awake()
    {
        base.Awake();

        tabs.Add(ItemType.Usable, useableTab);
        tabs.Add(ItemType.Equipment, equipmentTab); 
        tabs.Add(ItemType.Key, keyItemTab);
    }

    private void Update()
    {
        if (isActive)
        {
            if (Input.GetButtonDown("R2"))
            {
                ChangeTab(1);
            }
            else if (Input.GetButtonDown("L2"))
            {
                ChangeTab(-1);
            }
        }
    }

    public override void Display()
    {
        base.Display();

        displayedItemType = ItemType.Usable;
        DisplayItems();
    }

    public override void Hide()
    {
        base.Hide();

        ClearAllData();
    }

    private void ChangeTab(int direction)
    {
        int newIndex = (int)displayedItemType + direction;
        if (newIndex < Enum.GetNames(typeof(ItemType)).Length && newIndex >= 0)
        {
            displayedItemType = (ItemType)newIndex;
            DisplayItems();
        }
    }

    private void DisplayItems()
    {
        ClearAllData();

        if(tabs[displayedItemType] != null)
        {
            tabs[displayedItemType].color = activeTabColor;
        }

        List<InventoryItem> itemsToDisplay = new List<InventoryItem>();
        //itemsToDisplay = SaveManager.Instance.LoadedData.PlayerData.InventoryData.GetItems(displayedItemType);

        itemList.CreateList(itemsToDisplay.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot scrollableListSlot in itemList.SlotList)
        {
            if (scrollableListSlot is ScrollableListSlotItem)
            {
                ScrollableListSlotItem itemSlot = (ScrollableListSlotItem)scrollableListSlot;
                InventoryItem inventoryItem = itemsToDisplay[slotIndex];
                if (itemSlot == null || inventoryItem == null)
                {
                    Item item = itemDatabase.LookupDictionary[inventoryItem.ItemID];
                    if (item != null)
                    {
                        itemSlot.AssignItem(item, inventoryItem.Count);
                    }
                }
            }
            slotIndex++;
        }

        if (itemList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(itemList.SlotList[0].gameObject);
            itemList.SlotList[0].OnSelect(null);
        }
    }

    public void OnSelectItem(GameObject gameObject)
    {
        ScrollableListSlotItem itemSlot = gameObject.GetComponent<ScrollableListSlotItem>();
        if (itemSlot != null)
        {
            Item item = itemSlot.Item;
            if (item != null)
            {
                itemInfo.DisplayItem(item);
            }
            else
            {
                //itemInfo.Hide();
            }
        }
    }

    private void ClearAllData()
    {
        itemList.ClearList();
        foreach (KeyValuePair<ItemType, Image> tab in tabs)
        {
            if (tab.Value != null)
            {
                tab.Value.color = inactiveTabColor;
            }
        }
    }
}
