using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuStateParty : PauseMenuState
{
    [SerializeField] private GameObject display;

    [SerializeField] private List<PauseMenuPartySlot> partySlots = new List<PauseMenuPartySlot>();
    [SerializeField] private PauseMenuStatDisplay pauseMenuStatDisplay;

    public override void Awake()
    {
        base.Awake();
        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Main Pause Menu State");

        InputManager.Instance.OnPressCancel.AddListener(Cancel);

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

        InputManager.Instance.OnPressCancel.RemoveListener(Cancel);

        Hide();
    }

    private void Display()
    {
        display.SetActive(true);

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

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(partySlots[0].gameObject);
                partySlots[0].OnSelect(null);
            }
            else if (partySlots[i].gameObject.activeInHierarchy)
            {
                partySlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickPartySlot(PauseMenuPartySlot partySlot)
    {
        Debug.Log("On click party slot");
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

    private void Hide()
    {
        display.SetActive(false);
    }

    private void Cancel(bool isPressed)
    {
        Debug.Log("Pressed Cancel in party menu");
        stateMachine.ChangeState((int)PauseMenuStateType.Main);
    }

}
