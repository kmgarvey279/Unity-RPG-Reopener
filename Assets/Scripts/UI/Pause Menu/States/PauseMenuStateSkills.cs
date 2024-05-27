using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuStateSkills : PauseMenuState
{
    [SerializeField] private GameObject display;

    [Header("Character Tabs")]
    [SerializeField] private TextMeshProUGUI currentCharacterLabel;
    [SerializeField] private PartyTabDisplay partyTabDisplay;

    [Header("Skill Info")]
    [SerializeField] private ScrollableList skillsList;
    [SerializeField] private SkillInfo skillInfo;

    public override void Awake()
    {
        base.Awake();

        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Skills Pause Menu State");

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
        display.SetActive(false);

        ClearAllData();
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
        DisplaySkills(playableCharacterID);
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

                skillSlot.AssignAction(skill, skill.MPCost);                
            }
            slotIndex++;
        }
        if (skillsList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(skillsList.SlotList[0].gameObject);
            skillsList.SlotList[0].OnSelect(null);
        }
    }

    public void OnSelectSkill(GameObject gameObject)
    {
        ScrollableListSlotSkill skillSlot = gameObject.GetComponent<ScrollableListSlotSkill>();
        if (skillSlot != null)
        {
            Action action = skillSlot.Action;
            skillInfo.DisplayAction(action);
        }
    }

    private void ClearAllData()
    {
        skillInfo.Clear();
        skillsList.ClearList();
    }
}
