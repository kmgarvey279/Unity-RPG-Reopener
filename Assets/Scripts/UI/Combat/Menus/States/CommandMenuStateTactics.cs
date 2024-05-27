using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachineNamespace;
using UnityEngine.EventSystems;

public class CommandMenuStateTactics : CommandMenuState
{
    [SerializeField] private GameObject display;
    [Header("Buttons")]
    [SerializeField] private SelectableButton defendButton;
    [SerializeField] private SelectableButton escapeButton;
    [SerializeField] private SelectableButton swapButton;
    [Header("Text")]
    [SerializeField] private TextBox textbox;
    private readonly string defendText = "Reduce damage by 50% until your next turn. Doubles intervention points gained this turn.";
    private readonly string escapeText = "Attempt to flee the current battle.";
    private readonly string swapText = "Change party members. Can only be used once per turn";

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
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(lastButton);
            SelectableButton selectableButton = lastButton.GetComponent<SelectableButton>();
            selectableButton.OnSelect(null);

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
        if (CheckDefend())
        {
            defendButton.ToggleEnabled(true);
        }
        else
        {
            defendButton.ToggleEnabled(false);
        }

        if (CheckEscape())
        {
            escapeButton.ToggleEnabled(true);
        }
        else
        {
            escapeButton.ToggleEnabled(false);
        }

        if (CheckParty())
        {
            swapButton.ToggleEnabled(true);
        }
        else
        {
            swapButton.ToggleEnabled(false);
        }

        display.SetActive(true);

        if (lastButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(lastButton.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defendButton.gameObject);
        }
    }

    private void Hide()
    {
        display.SetActive(false);
    }

    private void Cancel(bool isPressed)
    {
        EventSystem.current.SetSelectedGameObject(null);

        stateMachine.ChangeState((int)CommandMenuStateType.Main);
    }

    #region Handle Selection
    public void OnSelectDefend()
    {
        textbox.SetText(defendText);
    }

    public void OnSelectEscape()
    {
        textbox.SetText(escapeText);
    }

    public void OnSelectSwap()
    {
        textbox.SetText(swapText);
    }
    #endregion

    #region Check Valid Commands
    private bool CheckDefend()
    {
        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        if (actor.CheckBool(CombatantBool.CannotDefend))
        {
            return false;
        }
        return true;
    }

    private bool CheckEscape()
    {
        if (battleManager.BattleData.CheckBool(BattleDataBool.CannotEscape))
        {
            return false;
        }
        return true;
    }

    private bool CheckParty()
    {
        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        if (actor == null)
        {
            return false;
        }

        if (battleTimeline.CurrentTurn.SwappedThisTurn || actor.CheckBool(CombatantBool.CannotSwap) || battleManager.ReservePlayableCombatants.Count == 0)
        {
            return false;
        }
        return true;
    }
    #endregion

    #region Handle Clicks
    public void OnClickDefend()
    {
        lastButton = defendButton.gameObject;

        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        Action defend = actor.Defend;

        if (defend != null)
        {
            battleTimeline.CurrentTurn.SetAction(defend);

            EventSystem.current.SetSelectedGameObject(null);
            onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
        }
    }

    public void OnClickEscape()
    {
        lastButton = escapeButton.gameObject;

        EventSystem.current.SetSelectedGameObject(null);
        //stateMachine.ChangeState((int)CommandMenuStateType.Escape);

        battleManager.ExitBattle(true);
    }

    public void OnClickSwap()
    {
        lastButton = swapButton.gameObject;

        EventSystem.current.SetSelectedGameObject(null);
        stateMachine.ChangeState((int)CommandMenuStateType.Party);
    }
    #endregion
}
