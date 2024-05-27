using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class PauseMenuEquip : PauseSubmenu
{
    [Header("Character Tabs")]
    //[SerializeField] private PauseMenuPartyTabs partyTabs;
    private int displayedCharacterIndex = 0;
    private List<PlayableCharacterID> charactersInParty = new List<PlayableCharacterID>();
    private int previousInput = 0;

    [Header("Equip Slots")]
    [SerializeField] private EquipmentSlot weaponSlot;
    [SerializeField] private EquipmentSlot armorSlot;
    [SerializeField] private EquipmentSlot accessorySlot;
    private Dictionary<EquipmentType, EquipmentSlot> equipmentSlots;
    private EquipmentSlot clickedSlot;
    private EquippableItemInstance clickedEquippableItemInstance;

    [Header("Stats")]
    [SerializeField] private StatPreview statPreview;

    [Header("Equipment Info")]
    [SerializeField] private ItemInfo itemInfo;
    [SerializeField] private ScrollableList equippableItemList;

    [Header("Confirmation Prompt")]
    [SerializeField] private GameObject unequipPrompt;
    [SerializeField] private GameObject defaultPromptButton;



    protected override void Awake()
    {
        base.Awake();

        equipmentSlots = new Dictionary<EquipmentType, EquipmentSlot>()
        {
            { EquipmentType.Weapon, weaponSlot },
            { EquipmentType.Armor, armorSlot },
            { EquipmentType.Accessory, accessorySlot }
        };

        if (display.activeInHierarchy)
        {
            display.SetActive(false);
            isActive = false;
        }
    }

    public void Update()
    {
        if (!isActive || unequipPrompt.activeInHierarchy)
        {
            return;
        }

        if (equippableItemList.gameObject.activeInHierarchy)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                SwitchToEquipSlots();
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Hide();
                pauseMenu.OnExitSubmenu();
            }
            else
            {
                int moveX = (int)Input.GetAxisRaw("Horizontal");

                if (moveX != previousInput)
                {
                    previousInput = moveX;
                    if (moveX != 0)
                    {
                        CycleCharacters(moveX);
                    }
                }
            }
        }
    }

    public override void Display()
    {
        base.Display();

        clickedSlot = null;
        clickedEquippableItemInstance = null;

        //slot buttons + list
        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            entry.Value.ToggleButton(true);
        }
        equippableItemList.gameObject.SetActive(false);

        //generate party list + display info
        PartyData partyData = SaveManager.Instance.LoadedData.PlayerData.PartyData;

        //reset
        charactersInParty.Clear();
        displayedCharacterIndex = 0;

        //generate list
        //partyTabs.DisplayList(partyData.PartyMembers);
        foreach (PlayableCharacterID playableCharacterID in partyData.PartyMembers)
        {
            charactersInParty.Add(playableCharacterID);
        }

        //display
        //partyTabs.UpdateSelected(charactersInParty[displayedCharacterIndex]);
        DisplayCharacterInfo(charactersInParty[displayedCharacterIndex]);
    }

    public override void Hide()
    {
        base.Hide();

        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            entry.Value.OnDeselect(null);
        }
    }

    #region Character Tabs
    private void CycleCharacters(int direction)
    {
        if (charactersInParty.Count <= 1)
        {
            return;
        }
        int newIndex = displayedCharacterIndex + direction;
        if (newIndex >= charactersInParty.Count)
        {
            newIndex = 0;
        }
        if (newIndex < 0)
        {
            newIndex = charactersInParty.Count - 1;
        }
        displayedCharacterIndex = newIndex;

        //partyTabs.UpdateTabs(charactersInParty[displayedCharacterIndex]);
        DisplayCharacterInfo(charactersInParty[displayedCharacterIndex]);
    }
    #endregion

    #region Character Equipment Slots

    private void SwitchToEquipSlots()
    {
        //refresh character info
        DisplayCharacterInfo(charactersInParty[displayedCharacterIndex]);

        //clear list
        equippableItemList.gameObject.SetActive(false);

        //clear preview
        statPreview.HidePreview();

        //reactivate slot buttons
        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            entry.Value.ToggleButton(true);
        }

        //select last clicked equipment slot
        if (clickedSlot != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(clickedSlot.gameObject);
            clickedSlot.OnSelect(null);
            clickedSlot = null;
        }
        //default fallback 
        else if (equipmentSlots.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(equipmentSlots[0].gameObject);
            equipmentSlots[0].OnSelect(null);
        }
    }

    private void DisplayCharacterInfo(PlayableCharacterID playableCharacterID)
    {
        PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);

        //stats
        statPreview.DisplayStats(playableCombatantRuntimeData.CurrentHP, playableCombatantRuntimeData.CurrentMP, playableCombatantRuntimeData.GetStatDict());

        //equipment slots
        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            EquipmentItem equipmentItem = playableCombatantRuntimeData.Equipment[entry.Key];
            if (equipmentItem)
            {
                entry.Value.AssignSlot(equipmentItem);
            }
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(equipmentSlots[0].gameObject);
        equipmentSlots[0].OnSelect(null);
    }

    //public void OnSelectEquipmentSlot(EquipmentSlot equipmentSlot)
    //{
    //    EquipmentSlot selectedSlot = equipmentSlot;
    //    itemInfo.DisplayItem(selectedSlot.EquipmentItem);
    //}

    public void OnClickEquipmentSlot(EquipmentSlot equipmentSlot)
    {
        clickedSlot = equipmentSlot;
        SwitchToItemList(equipmentSlot.EquipmentType);
    }
    #endregion

    #region Item List
    private void SwitchToItemList(EquipmentType equipmentType)
    {
        Debug.Log("Slot Count: " + equipmentSlots.Keys.Count);
        //deactivate slot buttons
        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            entry.Value.ToggleButton(false);
        }

        //display list
        DisplayEquipableItems(equipmentType);
    }

    private void DisplayEquipableItems(EquipmentType equipmentType) 
    {
        equippableItemList.ClearList();

        List<EquippableItemInstance> itemInstances = new List<EquippableItemInstance>();

        List<PlayableCharacterID> sortedCharacterList = new List<PlayableCharacterID>();
        for (int i = 0; i < charactersInParty.Count; i++)
        {
            if (i == displayedCharacterIndex)
            {
                sortedCharacterList.Insert(0, charactersInParty[i]);
            }
            else
            {
                sortedCharacterList.Add(charactersInParty[i]);
            }
        }

        //equipped items
        foreach (PlayableCharacterID playableCharacterID in charactersInParty)
        {
            PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
            if (playableCombatantRuntimeData.Equipment[equipmentType] == null)
            {
                continue;
            }
            //string equipmentID = playableCombatantData.EquipmentIDs[equipmentType];
            EquipmentItem equipmentItem = playableCombatantRuntimeData.Equipment[equipmentType];
            
            //if it is compatable with the selected character:
            if (equipmentItem && (!equipmentItem.CharacterExclusive || equipmentItem.ExclusiveCharacters.Contains(charactersInParty[displayedCharacterIndex])))
            {
                itemInstances.Add(new EquippableItemInstance(equipmentItem, 1, playableCharacterID));
            }
        }

        //items in inventory
        List<InventoryItem> compatableEquipment = SaveManager.Instance.LoadedData.PlayerData.InventoryData.GetFilteredEquipment(equipmentType, charactersInParty[displayedCharacterIndex]);
        foreach (InventoryItem inventoryItem in compatableEquipment)
        {
            if (inventoryItem.Item is EquipmentItem)
            {
                EquipmentItem equipmentItem = (EquipmentItem)inventoryItem.Item;
                itemInstances.Add(new EquippableItemInstance(equipmentItem, inventoryItem.Count));
            }
        }
        //create list
        if (!equippableItemList.gameObject.activeInHierarchy)
        {
            equippableItemList.gameObject.SetActive(true);
        }
        equippableItemList.CreateList(itemInstances.Count);
        
        int slotIndex = 0;
        foreach (ScrollableListSlot listSlot in equippableItemList.SlotList)
        {
            if (listSlot is ScrollableListSlotEquippableItem)
            {
                ScrollableListSlotEquippableItem equipmentSlot = (ScrollableListSlotEquippableItem)listSlot;
                equipmentSlot.AssignItem(itemInstances[slotIndex]);
            }
            slotIndex++;
        }

        if (equippableItemList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(equippableItemList.SlotList[0].gameObject);
            equippableItemList.SlotList[0].OnSelect(null);
        }
    }

    public void OnSelectInventoryEquipment(GameObject gameObject)
    {
        ScrollableListSlotEquippableItem itemSlot = gameObject.GetComponent<ScrollableListSlotEquippableItem>();
        if (itemSlot != null)
        {
            EquippableItemInstance selectedItemInstance = itemSlot.EquippableItemInstance;
            if (selectedItemInstance.EquipmentItem != null)
            {
                itemInfo.DisplayItem(selectedItemInstance.EquipmentItem);
                if (clickedSlot == null)
                {
                    clickedSlot = equipmentSlots[selectedItemInstance.EquipmentItem.EquipmentType];
                }
                DisplayEquipmentStatPreview(clickedSlot.EquipmentType, selectedItemInstance.EquipmentItem);
            }
        }
    }

    public void OnClickInventoryEquipment(GameObject gameObject)
    {
        ScrollableListSlotEquippableItem itemSlot = gameObject.GetComponent<ScrollableListSlotEquippableItem>();
        if (itemSlot != null)
        {
            clickedEquippableItemInstance = itemSlot.EquippableItemInstance;
            if (clickedEquippableItemInstance != null) 
            {
                if (clickedEquippableItemInstance.IsEquipped)
                {
                    if (clickedEquippableItemInstance.EquipmentItem.EquipmentType != EquipmentType.Weapon)
                    {
                        ToggleUnequipPrompt(true);
                    }
                }
                else
                {
                    EquipClickedItem();
                }
            }
        }
    }

    public void DisplayEquipmentStatPreview(EquipmentType equipmentType, EquipmentItem equipmentItem)
    {
        PlayableCharacterID playableCharacterID = charactersInParty[displayedCharacterIndex];
        PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);

        StatComparisonData statComparisonData = new StatComparisonData(playableCombatantRuntimeData.GetStatDict(), playableCombatantRuntimeData.Equipment[equipmentType], equipmentItem);
        statPreview.DisplayPreview(statComparisonData);
    }
    #endregion

    #region Equip + Prompt
    public void ToggleUnequipPrompt(bool isDisplayed)
    {
        //lock equip slot buttons
        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            entry.Value.ToggleButton(false);
        }
        //display prompt
        unequipPrompt.SetActive(isDisplayed);
        if (isDisplayed)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultPromptButton);
        }
    }

    public void OnConfirmUnequipPrompt()
    {
        EquipClickedItem();
        ToggleUnequipPrompt(false);
    }

    public void OnCancelUnequipPrompt()
    {
        ToggleUnequipPrompt(false);
        if (equippableItemList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(equippableItemList.SlotList[0].gameObject);
            equippableItemList.SlotList[0].OnSelect(null);
        }
    }

    private void EquipClickedItem()
    {
        if (clickedEquippableItemInstance != null)
        {
            PlayableCharacterID playableCharacterID = charactersInParty[displayedCharacterIndex];
            PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
            
            //if selected item is already equipped on a character
            if (clickedEquippableItemInstance.IsEquipped)
            {
                //if equipped on you, unequip it + add to inventory (if not weapon)
                if (playableCharacterID == clickedEquippableItemInstance.EquippedCharacterID && clickedSlot.EquipmentType != EquipmentType.Weapon)
                {
                    playableCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, null);
                    SaveManager.Instance.LoadedData.PlayerData.InventoryData.AddItem(clickedEquippableItemInstance.EquipmentItem);
                }
                //if equipped on someone else, unequip from them + equip on yourself, don't touch inventory
                else
                {
                    PlayableCombatantRuntimeData equippedCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);

                    equippedCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, null);
                    playableCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, clickedEquippableItemInstance.EquipmentItem);
                }
            }
            //if item isn't equipped on anyone, remove from inventory + add old equippemnt to inventory
            else
            {
                SaveManager.Instance.LoadedData.PlayerData.InventoryData.AddItem(clickedSlot.EquipmentItem);
                
                SaveManager.Instance.LoadedData.PlayerData.InventoryData.RemoveItem(clickedEquippableItemInstance.EquipmentItem);
                playableCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, clickedEquippableItemInstance.EquipmentItem);
            }
        }
        SwitchToEquipSlots();
    }
    #endregion
}
