using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommandMenuStateMain : CommandMenuState
{
    [SerializeField] private GameObject display;
    [Header("Buttons")]
    [SerializeField] private SelectableButton attackButton;
    [SerializeField] private SelectableButton skillsButton;
    [SerializeField] private SelectableButton itemsButton;
    [SerializeField] private SelectableButton tacticsButton;
    [Header("Text")]
    [SerializeField] private TextBox textbox;
    private readonly string attackText = "Deal x1.0 damage to the selected target. Restores MP.";
    private readonly string skillsText = "Use special abilities.";
    private readonly string itemsText = "Use items.";
    private readonly string tacticsText = "Defend, change party members, or attempt to flee.";

    public override void Awake()
    {
        base.Awake();
        //Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Main Command Menu State");

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

        Hide();
    }

    private void Display()
    {
        if (CheckAttack())
        {
            attackButton.ToggleEnabled(true);
        }
        else
        {
            attackButton.ToggleEnabled(false);
        }

        if (CheckSkills())
        {
            skillsButton.ToggleEnabled(true);
        }
        else
        {
            skillsButton.ToggleEnabled(false);
        }

        if (CheckItems())
        {
            itemsButton.ToggleEnabled(true);
        }
        else
        {
            itemsButton.ToggleEnabled(false);
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
            EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
        }

        Debug.Log(EventSystem.current);
    }

    private void Hide()
    {
        display.SetActive(false);
    }

    #region Check Valid Commands
    private bool CheckAttack()
    {
        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        Action attack = actor.Attack;

        if (attack == null || actor.CheckBool(CombatantBool.CannotTargetHostile) || (attack.IsMelee && actor.CheckBool(CombatantBool.CannotUseMelee)))
        {
            return false;
        }
        return true;
    }

    private bool CheckSkills()
    {
        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        if (actor.CheckBool(CombatantBool.CannotUseSkills))
        {
            return false;
        }
        return true;
    }

    private bool CheckItems()
    {
        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        if (actor.CheckBool(CombatantBool.CannotUseItems))
        {
            return false;
        }
        return true;
    }
    #endregion

    #region Handle Selection
    public void OnSelectAttack()
    {
        textbox.SetText(attackText);
    }

    public void OnSelectSkills()
    {
        textbox.SetText(skillsText);
    }

    public void OnSelectItems()
    {
        textbox.SetText(itemsText);
    }

    public void OnSelectTactics()
    {
        textbox.SetText(tacticsText);
    }
    #endregion

    #region Handle Clicks
    public void OnClickAttack()
    {
        lastButton = attackButton.gameObject;

        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        Action attack = actor.Attack;

        if (attack != null)
        {
            battleTimeline.CurrentTurn.SetAction(attack);

            EventSystem.current.SetSelectedGameObject(null);
            onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
        }
    }

    public void OnClickSkills()
    {
        lastButton = skillsButton.gameObject;

        EventSystem.current.SetSelectedGameObject(null);
        stateMachine.ChangeState((int)CommandMenuStateType.Skills);
    }

    public void OnClickItems()
    {
        lastButton = itemsButton.gameObject;

        EventSystem.current.SetSelectedGameObject(null);
        stateMachine.ChangeState((int)CommandMenuStateType.Items);
    }

    public void OnClickTactics()
    {
        lastButton = tacticsButton.gameObject;

        EventSystem.current.SetSelectedGameObject(null);
        stateMachine.ChangeState((int)CommandMenuStateType.Tactics);
    }
    #endregion
}
