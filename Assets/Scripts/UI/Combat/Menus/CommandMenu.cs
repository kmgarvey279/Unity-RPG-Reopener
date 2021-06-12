using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandMenu : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    private TurnData turnData;

    [SerializeField] private GameObject commandPanel;
    [SerializeField] private GameObject defaultButton;
    [SerializeField] private Button moveButton;

    [SerializeField] private SecondaryBattlePanel secondaryPanel;

    [SerializeField] private Action genericAttack;
    [SerializeField] private Action useItem;

    public void DisplayMenu()
    {
        turnData = battleManager.turnData;
        commandPanel.SetActive(true);
        
        if(turnData.hasMoved)
        {
            moveButton.interactable = false;
        }
        else
        {
            moveButton.interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }

    public void HideMenus()
    {
        commandPanel.SetActive(false);
        secondaryPanel.Hide();
    }

    public void SelectAttack()
    {
        turnData.action = genericAttack;
        if(turnData.hasMoved)
        {
            battleManager.stateMachine.ChangeState("TargetSelect");
        }
        else
        {
            battleManager.stateMachine.ChangeState("PlayerMove");
        }
    }

    public void SelectMove()
    {
        battleManager.stateMachine.ChangeState("PlayerMove");
    }

    public void DisplaySkillList()
    {
        secondaryPanel.DisplaySkills(turnData.combatant);
    }

    public void SelectSkill(GameObject skillSlot)
    {
        turnData.action = skillSlot.GetComponent<BattleSkillSlot>().skill;
        if(turnData.hasMoved)
        {
            if(turnData.action.fixedTarget)
            {
                battleManager.stateMachine.ChangeState("TargetSelect");
            }
            else
            {
                battleManager.stateMachine.ChangeState("TileSelect");
            }
        }
        else
        {
            battleManager.stateMachine.ChangeState("PlayerMove");
        }
    }

    public void DisplayItemList()
    {
        secondaryPanel.DisplayItems(turnData.combatant);
    }

    public void SelectItem()
    {

    }
}
