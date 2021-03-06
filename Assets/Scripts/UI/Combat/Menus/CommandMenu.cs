using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandMenu : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    private TurnData turnData;
    [Header("Components")]
    [SerializeField] private GameObject display;
    [SerializeField] private CommandSubmenu submenu;
    [Header("Buttons")]
    [SerializeField] private GameObject actionsButton;
    [Header("Other Actions")]
    [SerializeField] private Button moveButton;
    [SerializeField] private Action move;
    [SerializeField] private Action item;
    [SerializeField] private Action defend;
    [Header("Events")]
    [SerializeField] private SignalSenderString onUpdateBattleLog;

    public void DisplayMenu()
    {
        turnData = battleManager.turnData;
        display.SetActive(true);

        if(turnData.hasMoved)
        {
            moveButton.interactable = false;
        }
        else
        {
            moveButton.interactable = true;
        }
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(actionsButton);
    }

    public void HideMenus()
    {
        if(submenu.isDisplayed)
        {
            submenu.Hide();
        }
        display.SetActive(false);
    }

    public void ExitCurrentMenu()
    {
        if(submenu.isDisplayed)
        {
            submenu.Hide();
        }
    }

    public void SelectAttack()
    {
        battleManager.SetAction(turnData.combatant.attack);
        battleManager.stateMachine.ChangeState((int)BattleStateType.TileSelect);
    }

    public void DisplayActions()
    {
        PlayableCombatant playableCombatant = (PlayableCombatant)turnData.combatant;
        submenu.DisplaySkills(playableCombatant);
    }

    public void SelectAction(GameObject skillSlotObject)
    {
        Action skill = skillSlotObject.GetComponent<BattleSkillSlot>().action;
        battleManager.SetAction(skill);
        battleManager.stateMachine.ChangeState((int)BattleStateType.TileSelect);
    }

    public void DisplayItemList()
    {
        submenu.DisplayItems();
    }

    public void HideSubMenu()
    {
        submenu.Hide();
    }

    public void SelectMove()
    {
        battleManager.stateMachine.ChangeState((int)BattleStateType.PlayerMove);
    }

    public void SelectDefend()
    {
        Debug.Log("Defend!");
        battleManager.SetAction(defend);
        battleManager.stateMachine.ChangeState((int)BattleStateType.TileSelect);
    }

    public void ChangeFocus()
    {
        if(display.activeInHierarchy)
        {
            HideMenus();
            battleManager.ToggleTargetSelect(true);
        }
        else
        {
            battleManager.ToggleTargetSelect(false); 
            DisplayMenu();
        }
    }
}
