using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandMenuStateParty : CommandMenuState
{
    [SerializeField] private CommandMenuManager commandMenuManager;
    [SerializeField] private GameObject display;
    [Header("Reserve Members Status")]
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [Header("Reserve List")]
    [SerializeField] private ScrollableList reserveList;
    private PlayableCombatant combatant1;
    private PlayableCombatant combatant2;

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

    public override void OnEnter()
    {
        base.OnEnter();
        battleManager.ToggleCanQueueInterventions(false);
        Debug.Log("Now entering Party Command Menu State");

        //battleManager.BattleData.SetLastCommandMenuStateType(commandMenuStateType);
        combatant1 = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        combatant2 = null;
        Display();

        EnableListeners();
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

        DisableListeners();

        battleManager.ToggleCanQueueInterventions(true);

        if (combatant2)
            battleTimeline.UnhighlightTarget(combatant2);
        combatant1 = null;
        combatant2 = null;

        Hide();
    }

    private void Display()
    {
        display.SetActive(true);

        //create list
        List<PlayableCombatant> reserveParty = battleManager.ReservePlayableCombatants;
        reserveList.CreateList(reserveParty.Count);
            
        //assign values to slots
        int slotIndex = 0;
        foreach (ScrollableListSlot listSlot in reserveList.SlotList)
        {
            if (listSlot is ScrollableListSlotParty)
            {
                ScrollableListSlotParty partySlot = (ScrollableListSlotParty)listSlot;

                if (partySlot != null)
                {
                    partySlot.AssignCharacter(reserveParty[slotIndex]);
                }

                slotIndex++;
            }
        }

        //create character info panels
        for (int i = 0; i < reserveParty.Count; i++)
        {
            battlePartyHUD.CreatePartyPanel(reserveParty[i], i);
        }

        if (reserveList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(reserveList.SlotList[0].gameObject);
            reserveList.SlotList[0].OnSelect(null);
        }
    }

    private void Hide()
    {
        battlePartyHUD.Clear();
        reserveList.ClearList();
        display.SetActive(false);
    }

    public void OnSelectSlot(GameObject slotObject)
    {
        ClearPreview();

        ScrollableListSlotParty partySlot = slotObject.GetComponent<ScrollableListSlotParty>();
        if (partySlot != null)
        {
            PlayableCombatant playableCombatant = partySlot.PlayableCombatant;
            if (playableCombatant != null)
            {
                combatant2 = playableCombatant;
                BattlePartyPanel battlePartyPanel = battlePartyHUD.GetPanel(playableCombatant.PlayableCharacterID);
                if (battlePartyPanel != null)
                {
                    battlePartyPanel.OnTurnStart();
                }

                battleTimeline.DisplaySwapPreview(combatant1, combatant2);
                battleTimeline.HighlightTarget(combatant2);
            }
        }
    }

    public void OnClickSlot(GameObject slotObject)
    {
        Debug.Log("clicked on reserve character");
        ScrollableListSlotParty partySlot = slotObject.GetComponent<ScrollableListSlotParty>();
        if (partySlot != null)
        {
            Debug.Log("found reserve character slot");
            PlayableCombatant playableCombatant = partySlot.PlayableCombatant;
            if (playableCombatant != null)
            {
                Debug.Log("found reserve character gameobject");
                Hide();

                battleTimeline.PublishSwapPreview(combatant1, playableCombatant);
                StartCoroutine(SwapCharactersCo());
            }
        }
    }

    private IEnumerator SwapCharactersCo()
    {
        Debug.Log("starting swapping reserve character coroutine");
        yield return battleManager.SwapPlayableCombatants(combatant1, combatant2);

        commandMenuManager.ResetMenu();
        stateMachine.ChangeState((int)CommandMenuStateType.Main);
    }

    public override void Reset()
    {
        base.Reset();
    }

    private void ClearPreview()
    {
        battlePartyHUD.ClearAllHighlights();
        if (combatant2 != null)
        {
            battleTimeline.CancelSwapPreview(combatant1, combatant2);
        }
    }

    private void Cancel(bool isPressed)
    {
        ClearPreview();

        battleTimeline.DisplayTurnOrder();
        stateMachine.ChangeState((int)CommandMenuStateType.Tactics);
    }
}
