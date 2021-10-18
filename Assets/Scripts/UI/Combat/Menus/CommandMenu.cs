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
    [SerializeField] private GameObject commandPanel;
    [SerializeField] private SecondaryBattlePanel secondaryPanel;
    [Header("Buttons")]
    [SerializeField] private GameObject defaultButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button moveButton;

    [Header("Actions")]
    [SerializeField] private Action item;
    [SerializeField] private Action defend;

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
        PlayableCombatant playableCombatant = (PlayableCombatant)battleManager.turnData.combatant;
        Action attack;
        if(playableCombatant.rangedAttack)
        {
            attack = playableCombatant.rangedAttack;
        }
        else 
        {
            attack = playableCombatant.meleeAttack;
        }
        if(attack != null)
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

    public void SelectMove()
    {
        Debug.Log("Defend!");
        battleManager.SetAction(defend);
        battleManager.stateMachine.ChangeState((int)BattleStateType.Move);
    }

    public void SelectDefend()
    {
        Debug.Log("Defend!");
        battleManager.SetAction(defend);
        battleManager.stateMachine.ChangeState((int)BattleStateType.Move);
    }
}
