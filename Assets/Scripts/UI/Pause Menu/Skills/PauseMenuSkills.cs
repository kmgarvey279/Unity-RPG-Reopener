using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuSkills : PauseSubmenu
{
    [SerializeField] private PlayableCombatantDatabase playableCombatantDatabase;

    [Header("Character Tabs")]
    //[SerializeField] private PauseMenuPartyTabs partyTabs;
    private int displayedCharacterIndex = 0;
    //ensure characters are always displayed in same realative order
    private List<PlayableCharacterID> charactersInParty = new List<PlayableCharacterID>();
    private int previousInput = 0;

    [Header("Skill Info")]
    [SerializeField] private ScrollableList skillsList;
    [SerializeField] private SkillInfo skillInfo;

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

        List<PlayableCharacterID> partyMembers = SaveManager.Instance.LoadedData.PlayerData.PartyData.PartyMembers;

        //generate list
        //partyTabs.DisplayList(partyMembers);
        foreach (PlayableCharacterID playableCharacterID in partyMembers)
        {
            charactersInParty.Add(playableCharacterID);
        }

        //display
        //partyTabs.UpdateSelected(charactersInParty[displayedCharacterIndex]);
        DisplaySkills(charactersInParty[displayedCharacterIndex]);
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

        //partyTabs.UpdateSelected(charactersInParty[displayedCharacterIndex]);
        DisplaySkills(charactersInParty[displayedCharacterIndex]);
    }

    private void DisplaySkills(PlayableCharacterID playableCharacterID)
    {
        ClearAllData();

        PlayableCombatantRuntimeData playableCombatantData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
        List<Action> unlockedSkills = playableCombatantData.UnlockedSkills;

        skillsList.CreateList(unlockedSkills.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot scrollableListSlot in skillsList.SlotList)
        {
            if (scrollableListSlot is ScrollableListSlotSkill)
            {
                ScrollableListSlotSkill skillSlot = (ScrollableListSlotSkill)scrollableListSlot;
                Action skill = unlockedSkills[slotIndex];

                if (skillSlot == null || skill == null)
                {
                    skillSlot.AssignAction(skill, skill.MPCost);

                    if (slotIndex == 0)
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        EventSystem.current.SetSelectedGameObject(skillSlot.gameObject);
                        skillSlot.OnSelect(null);
                    }
                }
            }
            slotIndex++;
        }
    }

    public void OnSelectSkill(GameObject gameObject)
    {
        ScrollableListSlotSkill skillSlot = gameObject.GetComponent<ScrollableListSlotSkill>();
        if (skillSlot != null)
        {
            Action action = skillSlot.Action;
            if (action != null)
            {
                skillInfo.DisplayAction(action);
            }
            else
            {
                skillInfo.Clear();
            }
        }
    }

    private void ClearAllData()
    {
        skillInfo.Clear();
        skillsList.ClearList();
    }
}
