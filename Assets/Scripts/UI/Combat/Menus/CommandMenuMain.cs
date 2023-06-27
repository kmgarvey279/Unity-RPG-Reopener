using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuMain : CommandMenu
{
    [Header("Other Menus")]
    [SerializeField] private CommandMenuListActions skillsMenu;
    [SerializeField] private CommandMenuListItems itemsMenu;
    [SerializeField] private CommandMenuParty partyMenu;

    public void HideAll()
    {
        Hide();
        skillsMenu.HideList();
        itemsMenu.HideList();
        partyMenu.Hide();
    }

    public void OnClickAttack()
    {
        Hide();

        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        battleTimeline.UpdateTurnAction(battleTimeline.CurrentTurn, actor.Attack);
        onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
    }

    public void OnClickSkills()
    {
        Hide();

        skillsMenu.DisplayList();
    }

    public void OnClickItems()
    {
        Hide();

        itemsMenu.DisplayList();
    }

    public void OnClickParty()
    {
        Hide();

        partyMenu.Display();
    }

    public void OnClickDefend()
    {
        Hide();

        PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        battleTimeline.UpdateTurnAction(battleTimeline.CurrentTurn, playableCombatant.Defend);
        onChangeBattleState.Raise((int)BattleStateType.TargetSelect);
    }

    public void OnClickEscape()
    {
        Hide();
    }
}
