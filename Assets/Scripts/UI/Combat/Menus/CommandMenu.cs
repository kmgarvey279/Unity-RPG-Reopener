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

    [SerializeField] private Action attack;
    [SerializeField] private Action item;
    [SerializeField] private Action guard;

    public void DisplayMenu()
    {
        turnData = battleManager.turnData;
        commandPanel.SetActive(true);
        
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
        battleManager.SetAction(attack);
        battleManager.stateMachine.ChangeState((int)BattleStateType.Move);
    }

    public void DisplaySkillList()
    {
        secondaryPanel.DisplaySkills(turnData.combatant);
    }

    public void SelectSkill(GameObject skillSlot)
    {
        battleManager.SetAction(skillSlot.GetComponent<BattleSkillSlot>().skill);
        battleManager.stateMachine.ChangeState((int)BattleStateType.Move);
    }

    public void DisplayItemList()
    {
        secondaryPanel.DisplayItems();
    }

    public void SelectItem()
    {

    }
}
