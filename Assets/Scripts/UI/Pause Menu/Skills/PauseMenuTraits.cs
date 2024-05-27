using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuTraits : PauseSubmenu
{
    [SerializeField] private PlayableCombatantDatabase playableCombatantDatabase;

    [Header("Trait Info")]
    [SerializeField] private ScrollableList traitsList;
    [SerializeField] private TraitInfo traitInfo;

    [Header("Character Tabs")]
    //[SerializeField] private PauseMenuPartyTabs partyTabs;
    private int displayedCharacterIndex = 0;
    private List<PlayableCharacterID> charactersInParty = new List<PlayableCharacterID>();
    private int previousInput = 0;

    public void Update()
    {
        if (!isActive)
        {
            return;
        }

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

    public override void Display()
    {
        base.Display();

        //reset
        charactersInParty.Clear();
        displayedCharacterIndex = 0;

        //create tabs for all characters in party

        //generate list of traits for first character
        //-- get full list from database
        //-- add all unlocked traits to a new list 

        PartyData partyData = SaveManager.Instance.LoadedData.PlayerData.PartyData;

        //partyTabs.DisplayList(partyData.PartyMembers);
        charactersInParty.AddRange(partyData.PartyMembers);

        //display
        //partyTabs.UpdateSelected(charactersInParty[displayedCharacterIndex]);
        DisplayTraits(charactersInParty[displayedCharacterIndex]);
    }

    public override void Hide()
    {
        base.Hide();

        ClearAllData();
    }

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
        DisplayTraits(charactersInParty[displayedCharacterIndex]);
    }

    private void DisplayTraits(PlayableCharacterID playableCharacterID)
    {
        ClearAllData();

        PlayableCombatantRuntimeData playableCombatantRuntimeData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
        List<Trait> unlockedTraits = playableCombatantRuntimeData.UnlockedTraits;

        traitsList.CreateList(unlockedTraits.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot scrollableListSlot in traitsList.SlotList)
        {
            if (scrollableListSlot is ScrollableListSlotTrait)
            {
                ScrollableListSlotTrait traitSlot = (ScrollableListSlotTrait)scrollableListSlot;
                Trait trait = unlockedTraits[slotIndex];

                if (traitSlot == null || trait == null)
                {
                    break;
                }
                traitSlot.AssignTrait(trait);

                if (slotIndex == 0)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(traitSlot.gameObject);
                    traitSlot.OnSelect(null);
                }
            }
            slotIndex++;
        }
    }

    public void OnSelectTrait(GameObject gameObject)
    {
        ScrollableListSlotTrait traitSlot = gameObject.GetComponent<ScrollableListSlotTrait>();
        if (traitSlot != null)
        {
            Trait trait = traitSlot.Trait;
            if (trait != null)
            {
                traitInfo.DisplayTrait(trait);
            }
            else
            {
                //traitInfo.Clear();
            }
        }
    }

    private void ClearAllData()
    {
        //traitInfo.Clear();
        traitsList.ClearList();
    }
}
