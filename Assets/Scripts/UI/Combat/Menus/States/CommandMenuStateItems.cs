using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class CommandMenuStateItems : CommandMenuState
{
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private GameObject display;
    [Header("Item Info")]
    [SerializeField] private ItemInfo itemInfo;
    [Header("Item List")]
    [SerializeField] private ScrollableList itemList;

    private void EnableListeners()
    {
        InputManager.Instance.OnPressCancel.AddListener(Cancel);
    }

    private void DisableListeners()
    {
        InputManager.Instance.OnPressCancel.RemoveListener(Cancel);
    }

    public override void Awake()
    {
        base.Awake();
        Hide();
    }

    private void OnEnable()
    {
        if (lastButton != null)
        {
            ScrollableListSlot slotToSelect = lastButton.GetComponent<ScrollableListSlot>();
            if (slotToSelect != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(slotToSelect.gameObject);
                slotToSelect.OnSelect(null);
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Items Command Menu State");

        //battleManager.BattleData.SetLastCommandMenuStateType(commandMenuStateType);
        Display();

        EnableListeners();
    }

    public override void StateUpdate()
    {
        //if (Input.GetButtonDown("Cancel"))
        //{
        //    onChangeCommandMenuState.Raise((int)CommandMenuStateType.Main);
        //}
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();

        DisableListeners();

        Hide();
    }

    private void Display()
    {
        display.SetActive(true);
        
        itemInfo.Clear();

        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        if (actor != null)
        {
            battlePartyHUD.ApplyFilter(actor.PlayableCharacterID);
        }

        List<InventoryItem> inventoryItems = SaveManager.Instance.LoadedData.PlayerData.InventoryData.GetItems(ItemFilterType.Usable);
        itemList.CreateList(inventoryItems.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot listSlot in itemList.SlotList)
        {
            if (listSlot is ScrollableListSlotItem)
            {
                ScrollableListSlotItem itemSlot = (ScrollableListSlotItem)listSlot;
                InventoryItem inventoryItem = inventoryItems[slotIndex];
                if (inventoryItem != null)
                {
                    Item item = inventoryItem.Item;
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

    private void Hide()
    {
        display.SetActive(false);

        battlePartyHUD.RemoveFilters();
    }

    public void OnSelectSlot(GameObject slotObject)
    {
        ScrollableListSlotItem itemSlot = slotObject.GetComponent<ScrollableListSlotItem>();
        if (itemSlot != null && itemSlot.Item != null)
        {
            itemInfo.DisplayItem(itemSlot.Item);
        }
    }

    public void OnClickSlot(GameObject slotObject)
    {
        ScrollableListSlotItem itemSlot = slotObject.GetComponent<ScrollableListSlotItem>();
        if (itemSlot != null)
        {
            UsableItem usableItem = (UsableItem)itemSlot.Item;
            if (usableItem != null && usableItem.UseAction != null)
            {
                lastButton = slotObject;

                battleTimeline.CurrentTurn.SetAction(usableItem.UseAction);
                battleTimeline.CurrentTurn.SetItemToUse(usableItem);
                onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        itemList.ClearList();
    }

    private void Cancel(bool isPressed)
    {
        stateMachine.ChangeState((int)CommandMenuStateType.Main);
    }
}
