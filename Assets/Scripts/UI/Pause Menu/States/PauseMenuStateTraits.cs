using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuStateTraits : PauseMenuState
{
    [SerializeField] private GameObject display;

    [Header("Character Tabs")]
    [SerializeField] private TextMeshProUGUI currentCharacterLabel;
    [SerializeField] private PartyTabDisplay partyTabDisplay;

    [Header("Skill Info")]
    [SerializeField] private ScrollableList traitsList;
    [SerializeField] private TraitInfo traitInfo;

    public override void Awake()
    {
        base.Awake();

        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Traits Pause Menu State");

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

        PartyData partyData = SaveManager.Instance.LoadedData.PlayerData.PartyData;
        partyTabDisplay.DisplayParty(partyData.ActivePartyMembers, partyData.ReservePartyMembers);

        //display
        DisplayCharacterInfo(partyTabDisplay.CurrentCharacterID);
    }

    private void Hide()
    {
        ClearAllData();
        display.SetActive(false);
    }

    #region Input
    public void PressCancel(bool isPressed)
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Main);
    }
    #endregion
    public void OnChangeCharacterTab()
    {
        DisplayCharacterInfo(partyTabDisplay.CurrentCharacterID);
    }

    private void DisplayCharacterInfo(PlayableCharacterID playableCharacterID)
    {
        //name
        currentCharacterLabel.text = playableCharacterID.ToString();
        DisplayTraits(playableCharacterID);
    }

    private void DisplayTraits(PlayableCharacterID playableCharacterID)
    {
        ClearAllData();

        PlayableCombatantRuntimeData playableCombatantData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
        List<Trait> unlockedTraits = playableCombatantData.UnlockedTraits;

        traitsList.CreateList(unlockedTraits.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot scrollableListSlot in traitsList.SlotList)
        {
            if (scrollableListSlot is ScrollableListSlotTrait)
            {
                ScrollableListSlotTrait traitSlot = (ScrollableListSlotTrait)scrollableListSlot;
                Trait trait = unlockedTraits[slotIndex];

                traitSlot.AssignTrait(trait);
            }
            slotIndex++;
        }
        if (traitsList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(traitsList.SlotList[0].gameObject);
            traitsList.SlotList[0].OnSelect(null);
        }
    }

    public void OnSelectTrait(GameObject gameObject)
    {
        Debug.Log("found list gameobject trait");
        ScrollableListSlotTrait traitSlot = gameObject.GetComponent<ScrollableListSlotTrait>();
        if (traitSlot != null)
        {
            Debug.Log("found slot");
            Trait trait = traitSlot.Trait;
            if (trait != null)
            {
                Debug.Log("found trait");
                traitInfo.DisplayTrait(trait);
            }
        }
    }

    private void ClearAllData()
    {
        traitInfo.Clear();
        traitsList.ClearList();
    }
}
