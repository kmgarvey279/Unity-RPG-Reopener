using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuParty : PauseSubmenu
{
    [SerializeField] private List<PauseMenuPartySlot> partySlots = new List<PauseMenuPartySlot>();

    [SerializeField] private PauseMenuStatDisplay pauseMenuStatDisplay;

    public void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Hide();
            pauseMenu.OnExitSubmenu();
        }
    }

    public override void Display()
    {
        base.Display();

        PartyData partyData = SaveManager.Instance.LoadedData.PlayerData.PartyData;

        for (int i = 0; i < partySlots.Count; i++)
        {
            if (SaveManager.Instance.LoadedData.PlayerData.PartyData.PartyMembers.Count > i)
            {
                if (!partySlots[i].gameObject.activeInHierarchy)
                {
                    partySlots[i].gameObject.SetActive(true);
                }
                PlayableCharacterID playableCharacterID = partyData.PartyMembers[i];
                bool inActiveParty = false;
                {
                    if (partyData.ActivePartyMembers.Contains(playableCharacterID))
                    {
                        inActiveParty = true;
                    }
                }
                partySlots[i].AssignCharacter(playableCharacterID, inActiveParty);
            }
            else if(partySlots[i].gameObject.activeInHierarchy)
            {
                partySlots[i].gameObject.SetActive(false);
            }
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(partySlots[0].gameObject);
        partySlots[0].OnSelect(null);
    }

    public void OnClickPartySlot(PauseMenuPartySlot partySlot)
    {
        PartyData partyData = SaveManager.Instance.LoadedData.PlayerData.PartyData;
        PlayableCharacterID playableCharacterID = partySlot.PlayableCombatantRuntimeData.PlayableCharacterID;

        //remove
        if (partyData.ActivePartyMembers.Contains(playableCharacterID) && partyData.ActivePartyMembers.Count > 1)
        {
            partyData.RemoveFromActiveParty(playableCharacterID);
            partySlot.SwitchToReserve();
        }
        //add
        else if (!partyData.ActivePartyMembers.Contains(playableCharacterID) && partyData.ActivePartyMembers.Count < 3)
        {
            partyData.AddToActiveParty(playableCharacterID);
            partySlot.SwitchToActive();
        }
    }

    public override void Hide()
    {
        base.Hide();    
    }
}
