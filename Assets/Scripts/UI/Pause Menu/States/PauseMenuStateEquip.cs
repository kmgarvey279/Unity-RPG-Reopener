using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatComparisonData
{
    public Dictionary<IntStatType, int> OriginalStats { get; private set; } = new Dictionary<IntStatType, int>();
    public Dictionary<IntStatType, int> ModifiedStats { get; private set; } = new Dictionary<IntStatType, int>();

    public StatComparisonData(Dictionary<IntStatType, int> originalStats, EquipmentItem originalEquipmentItem, EquipmentItem newEquipmentItem)
    {
        OriginalStats = originalStats;
        //create a copy of character's stats
        ModifiedStats = new Dictionary<IntStatType, int>();
        foreach (IntStatType statType in Enum.GetValues(typeof(IntStatType)))
        {
            ModifiedStats.Add(statType, originalStats[statType]);
        }

        //remove any stat modifiers tied to the equipment being removed ;
        if (originalEquipmentItem != null)
        {
            foreach (IntStatModifier modifier in originalEquipmentItem.IntStatModifiers)
            {
                ModifiedStats[modifier.IntStatType] -= modifier.Modifier;
            }
        }

        //add modifiers from new equipment
        if (newEquipmentItem != null)
        {
            foreach (IntStatModifier modifier in newEquipmentItem.IntStatModifiers)
            {
                ModifiedStats[modifier.IntStatType] += modifier.Modifier;
            }
        }
    }
}

public class PauseMenuStateEquip : PauseMenuState
{
    [SerializeField] private GameObject display;

    [Header("Character Tabs")]
    [SerializeField] private TextMeshProUGUI currentCharacterLabel;
    [SerializeField] private PartyTabDisplay partyTabDisplay;

    [Header("Equip Slots")]
    [SerializeField] private EquipmentSlot weaponSlot;
    [SerializeField] private EquipmentSlot armorSlot;
    [SerializeField] private EquipmentSlot accessorySlot;
    private Dictionary<EquipmentType, EquipmentSlot> equipmentSlots;
    private EquipmentSlot clickedSlot;
    private EquippableItemInstance clickedEquippableItemInstance;

    [Header("Stats")]
    [SerializeField] private StatPreview statPreview;

    [Header("Equipment Info/List")]
    [SerializeField] private ItemInfo itemInfo;
    [SerializeField] private ScrollableList equippableItemList;

    [Header("Confirmation Prompt")]
    [SerializeField] private GameObject unequipPrompt;
    [SerializeField] private GameObject defaultPromptButton;

    public override void Awake()
    {
        base.Awake();
        
        equipmentSlots = new Dictionary<EquipmentType, EquipmentSlot>()
        {
            { EquipmentType.Weapon, weaponSlot },
            { EquipmentType.Armor, armorSlot },
            { EquipmentType.Accessory, accessorySlot }
        };

        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Main Pause Menu State");

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

    private void Display()
    {
        display.SetActive(true);

        clickedSlot = null;
        clickedEquippableItemInstance = null;

        //slot buttons + list
        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            entry.Value.ToggleButton(true);
        }
        equippableItemList.gameObject.SetActive(false);

        PartyData partyData = SaveManager.Instance.LoadedData.PlayerData.PartyData;
        partyTabDisplay.DisplayParty(partyData.ActivePartyMembers, partyData.ReservePartyMembers);

        DisplayCharacterInfo(partyTabDisplay.CurrentCharacterID);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(equipmentSlots[0].gameObject);
        equipmentSlots[0].OnSelect(null);
    }

    private void Hide()
    {
        display.SetActive(false);

        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            entry.Value.OnDeselect(null);
        }
    }

    #region Input
    public void PressCancel(bool isPressed)
    {
        if (equippableItemList.gameObject.activeInHierarchy)
        {
            SwitchToEquipSlots();
        }
        else
        {
            stateMachine.ChangeState((int)PauseMenuStateType.Main);
        }
    }
    #endregion

    #region Character Equipment Slots

    private void SwitchToEquipSlots()
    {
        //refresh character info
        DisplayCharacterInfo(partyTabDisplay.CurrentCharacterID);

        //deactivate list
        equippableItemList.gameObject.SetActive(false);

        //clear preview
        statPreview.HidePreview();

        //activate character tabs
        partyTabDisplay.ToggleInteractivity(true);

        //activate slot buttons
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

    public void OnChangeCharacterTab()
    {
        DisplayCharacterInfo(partyTabDisplay.CurrentCharacterID);
    }

    private void DisplayCharacterInfo(PlayableCharacterID playableCharacterID)
    {
        currentCharacterLabel.text = playableCharacterID.ToString();
        PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);

        Debug.Log("Selected character: " + playableCharacterID);
        Debug.Log("HP: " + playableCombatantRuntimeData.CurrentHP);
        Debug.Log("MP: " + playableCombatantRuntimeData.CurrentMP);
        Debug.Log("Atk: " + playableCombatantRuntimeData.GetStatDict()[IntStatType.Attack]);

        //stats
        statPreview.DisplayStats(playableCombatantRuntimeData.CurrentHP, playableCombatantRuntimeData.CurrentMP, playableCombatantRuntimeData.GetStatDict());

        //equipment slots
        foreach (KeyValuePair<EquipmentType, EquipmentSlot> entry in equipmentSlots)
        {
            EquipmentItem equipmentItem = playableCombatantRuntimeData.Equipment[entry.Key];
            entry.Value.AssignSlot(equipmentItem);
            
        }
    }

    public void OnClickEquipmentSlot(EquipmentSlot equipmentSlot)
    {
        clickedSlot = equipmentSlot;
        SwitchToItemList(equipmentSlot.EquipmentType);
    }
    #endregion

    #region Item List
    private void SwitchToItemList(EquipmentType equipmentType)
    {
        //deactivate character tabs
        partyTabDisplay.ToggleInteractivity(false);

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

        List<PlayableCharacterID> partyMembers = SaveManager.Instance.LoadedData.PlayerData.PartyData.PartyMembers;
        List<EquippableItemInstance> itemInstances = new List<EquippableItemInstance>();

        //equipped items on other characters
        foreach (PlayableCharacterID playableCharacterID in partyMembers)
        {
            PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
            if (playableCombatantRuntimeData.Equipment[equipmentType] == null)
            {
                continue;
            }

            EquipmentItem equipmentItem = playableCombatantRuntimeData.Equipment[equipmentType];

            //if it is compatable with the selected character:
            if (equipmentItem && (!equipmentItem.CharacterExclusive || equipmentItem.ExclusiveCharacters.Contains(partyTabDisplay.CurrentCharacterID)))
            {
                //current char always first
                if (playableCharacterID == partyTabDisplay.CurrentCharacterID)
                {
                    itemInstances.Insert(0, new EquippableItemInstance(equipmentItem, 1, playableCharacterID));
                }
                else
                {
                    itemInstances.Add(new EquippableItemInstance(equipmentItem, 1, playableCharacterID));
                }
            }
        }

        //items in inventory
        List<InventoryItem> compatableEquipment = SaveManager.Instance.LoadedData.PlayerData.InventoryData.GetFilteredEquipment(equipmentType, partyTabDisplay.CurrentCharacterID);
        foreach (InventoryItem inventoryItem in compatableEquipment)
        {
            if (inventoryItem.Item is EquipmentItem)
            {
                EquipmentItem equipmentItem = (EquipmentItem)inventoryItem.Item;
                itemInstances.Add(new EquippableItemInstance(equipmentItem, inventoryItem.Count));
            }
        }

        //equip nothing option
        if (equipmentType != EquipmentType.Weapon)
        {
            itemInstances.Add(null);
        }

        //create list
        if (!equippableItemList.gameObject.activeInHierarchy)
        {
            equippableItemList.gameObject.SetActive(true);
        }
        equippableItemList.CreateList(itemInstances.Count);

        int slotIndex = 0;
        //equipment slots
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

            if (selectedItemInstance == null)
            {
                itemInfo.DisplayItem(null);

                DisplayEquipmentStatPreview(clickedSlot.EquipmentType, null);
            }
            else
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
            if (clickedEquippableItemInstance == null)
            {
                SetSlotAsEmpty();
            }
            else
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
        PlayableCharacterID playableCharacterID = partyTabDisplay.CurrentCharacterID;
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
        else
        {
            SwitchToEquipSlots();
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
            PlayableCharacterID playableCharacterID = partyTabDisplay.CurrentCharacterID;
            PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);

            //if selected item is already equipped on a character
            if (clickedEquippableItemInstance.IsEquipped)
            {
                //if equipped on you, unequip it + add to inventory (if not weapon)
                //if (playableCharacterID == clickedEquippableItemInstance.EquippedCharacterID && clickedSlot.EquipmentType != EquipmentType.Weapon)
                //{
                //    playableCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, null);
                //    SaveManager.Instance.LoadedData.PlayerData.InventoryData.AddItem(clickedEquippableItemInstance.EquipmentItem);
                //}
                //if equipped on someone else, unequip from them + equip on yourself, don't touch inventory
                //else
                //{
                    PlayableCombatantRuntimeData equippedCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);

                    equippedCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, null);
                    playableCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, clickedEquippableItemInstance.EquipmentItem);
                //}
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

    private void SetSlotAsEmpty()
    {
        PlayableCharacterID playableCharacterID = partyTabDisplay.CurrentCharacterID;
        PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);

        //unequip whatever is in this equipment slot + add to inventory (if not weapon)
        if (clickedSlot.EquipmentType != EquipmentType.Weapon)
        {
            if (playableCombatantRuntimeData.Equipment[clickedSlot.EquipmentType] != null)
            {
                SaveManager.Instance.LoadedData.PlayerData.InventoryData.AddItem(playableCombatantRuntimeData.Equipment[clickedSlot.EquipmentType]);
            }
            playableCombatantRuntimeData.ChangeEquipment(clickedSlot.EquipmentType, null);
        }
        SwitchToEquipSlots();
    }
    #endregion
}
