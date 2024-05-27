using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuStateInventory : PauseMenuState
{
    [SerializeField] private GameObject display;

    [Header("Item Tabs")]
    [SerializeField] private ItemFilterTabs itemFilterTabs;

    [Header("Skill Info")]
    [SerializeField] private ScrollableList itemsList;
    [SerializeField] private ItemInfo itemInfo;

    public override void Awake()
    {
        base.Awake();

        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Items Pause Menu State");

        InputManager.Instance.OnPressCancel.AddListener(PressCancel);

        Display();
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();

        InputManager.Instance.OnPressCancel.RemoveListener(PressCancel);

        Hide();
    }

    public void PressCancel(bool isPressed)
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Main);
    }

    private void Display()
    {
        display.SetActive(true);

        DisplayFilteredItems();
    }

    private void DisplayFilteredItems()
    {
        ClearAllData();

        ItemFilterType currentFilterType = itemFilterTabs.CurrentItemFilterType;
        List<InventoryItem> itemsToDisplay = SaveManager.Instance.LoadedData.PlayerData.InventoryData.GetItems(currentFilterType);
        itemsList.CreateList(itemsToDisplay.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot scrollableListSlot in itemsList.SlotList)
        {
            if (scrollableListSlot is ScrollableListSlotItem)
            {
                ScrollableListSlotItem itemSlot = (ScrollableListSlotItem)scrollableListSlot;
                InventoryItem inventoryItem = itemsToDisplay[slotIndex];
                itemSlot.AssignItem(inventoryItem.Item, inventoryItem.Count);
            }
            slotIndex++;
        }

        if (itemsList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(itemsList.SlotList[0].gameObject);
            itemsList.SlotList[0].OnSelect(null);
        }
    }

    private void Hide()
    {
        ClearAllData();
        display.SetActive(false);
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
        }
    }

    public void OnChangeItemFilterTab()
    {
        DisplayFilteredItems(); 
    }

    private void ClearAllData()
    {
        itemInfo.Clear();
        itemsList.ClearList();
    }
}
