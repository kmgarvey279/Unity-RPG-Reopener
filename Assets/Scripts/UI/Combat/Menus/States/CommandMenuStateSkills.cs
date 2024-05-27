using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandMenuStateSkills : CommandMenuState
{
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private GameObject display;
    [Header("List")]
    [SerializeField] private ScrollableList skillList;
    [Header("Skill Info")]
    [SerializeField] private SkillInfo skillInfo;

    private void EnableListeners()
    {
        InputManager.Instance.OnPressCancel.AddListener(Cancel);
    }

    private void DisableListeners()
    {
        InputManager.Instance.OnPressCancel.RemoveListener(Cancel);
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

    public override void Awake()
    {
        base.Awake();
        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Skills Command Menu State");

        //battleManager.BattleData.SetLastCommandMenuStateType(commandMenuStateType);
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

        Hide();
    }

    private void Display()
    {
        display.SetActive(true);

        ClearAllData();

        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        bool isIntervention = battleTimeline.CurrentTurn.IsIntervention;

        if (actor != null)
        {
            battlePartyHUD.ApplyFilter(actor.PlayableCharacterID);

            skillList.CreateList(actor.Skills.Count);

            int slotIndex = 0;
            foreach (ScrollableListSlot scrollableListSlot in skillList.SlotList)
            {
                if (scrollableListSlot is ScrollableListSlotSkill)
                {
                    ScrollableListSlotSkill skillSlot = (ScrollableListSlotSkill) scrollableListSlot;
                    Action skill = actor.Skills[slotIndex];

                    if (skillSlot == null || skill == null)
                    {
                        break;
                    }
                    int mpCost = skill.MPCost;
                    if (mpCost > 0)
                    {
                        ApplyMPCostModifiers(skill, actor, isIntervention);
                    }
                    skillSlot.AssignAction(skill, mpCost);
                    skillSlot.CheckMP(actor.MP);
                }
                slotIndex++;
            }
        }

        if (skillList.SlotList.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(skillList.SlotList[0].gameObject);
            //skillList.SlotList[0].OnSelect(null);
        }
    }

    private void Hide()
    {
        display.SetActive(false);

        battlePartyHUD.RemoveFilters();
    }

    public void OnSelectSlot(GameObject slotObject)
    {
        Debug.Log("On select slot");
        ScrollableListSlotSkill skillSlot = slotObject.GetComponent<ScrollableListSlotSkill>();
        if (skillSlot != null)
        {
            Action action = skillSlot.Action;
            if (action != null)
            {
                //display skill
                skillInfo.DisplayAction(action);
            }
            else
            {
                Debug.LogError("No skill has been set for this list item.");
                skillInfo.Clear();
            }
        }
    }

    public void OnClickSlot(GameObject slotObject)
    {
        ScrollableListSlotSkill skillSlot = slotObject.GetComponent<ScrollableListSlotSkill>();
        if (skillSlot != null)
        {
            Action action = skillSlot.Action;
            if (action != null)
            {
                lastButton = slotObject;

                //set action
                battleTimeline.CurrentTurn.SetAction(action);

                //change battle/menu states
                onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        skillList.ClearList();
        skillInfo.Clear();
    }

    private int ApplyMPCostModifiers(Action action, Combatant actor, bool isIntervention)
    {
        float baseValue = (float)action.MPCost;
        ActionSummary actionSummary = new ActionSummary(action, isIntervention);
        //action
        foreach (ActionModifier actionModifier in action.ActionModifierDict[ActionModifierType.MPCost])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, null, actionSummary);
        }
        //action (custom)
        foreach (CustomActionModifier actionModifier in action.CustomActionModifierDict[ActionModifierType.MPCost])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, null, actionSummary);
        }
        //actor
        foreach (ActionModifier actionModifier in actor.ActionModifiers[BattleEventType.Acting][ActionModifierType.MPCost])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, null, actionSummary);
        }
        return Mathf.Clamp(Mathf.FloorToInt(baseValue), 1, 99);
    }

    private void Cancel(bool isPressed)
    {
        stateMachine.ChangeState((int)CommandMenuStateType.Main);
    }

    private void ClearAllData()
    {
        skillInfo.Clear();
        //skillsList.ClearList();
    }
}
