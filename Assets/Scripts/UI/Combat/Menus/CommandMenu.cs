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
    [SerializeField] private GameObject attackButton;
    [Header("Actions")]
    [SerializeField] private Action item;
    [SerializeField] private Action defend;
    [Header("Events")]
    [SerializeField] private SignalSenderString onUpdateBattleLog;

    public void DisplayMenu()
    {
        turnData = battleManager.turnData;
        display.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(attackButton);
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
        else
        {
            display.SetActive(false);
            battleManager.stateMachine.ChangeState((int)BattleStateType.Move);
        }
    }

    public void SelectAttack()
    {
        PlayableCombatant playableCombatant = (PlayableCombatant)turnData.combatant;
        if(playableCombatant.rangedAttack)
        {
            battleManager.SetAction(playableCombatant.rangedAttack);
        }
        else 
        {
            battleManager.SetAction(playableCombatant.meleeAttack);
        }
        battleManager.stateMachine.ChangeState((int)BattleStateType.TileSelect);
    }

    public void DisplaySkillList()
    {
        PlayableCombatant playableCombatant = (PlayableCombatant)turnData.combatant;
        submenu.DisplaySkills(playableCombatant.skills);
    }

    private void SelectSkill(GameObject skillSlotObject)
    {
        BattleSkillSlot skillSlot = skillSlotObject.GetComponent<BattleSkillSlot>();
        PlayableCombatant playableCombatant = (PlayableCombatant)turnData.combatant;
        if(playableCombatant.rangedAttack)
        {
            battleManager.SetAction(playableCombatant.rangedAttack);
        }
        else 
        {
            battleManager.SetAction(playableCombatant.meleeAttack);
        }
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

    public void SelectDefend()
    {
        Debug.Log("Defend!");
        battleManager.SetAction(defend);
        battleManager.stateMachine.ChangeState((int)BattleStateType.Execute);
    }
}
