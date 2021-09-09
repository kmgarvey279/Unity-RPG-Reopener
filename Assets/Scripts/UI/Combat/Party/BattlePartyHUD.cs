using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePartyHUD : MonoBehaviour
{
    [SerializeField] private GameObject partyPanelPrefab;
    private Dictionary<AllyCombatant, BattlePartyPanel> panelDict = new Dictionary<AllyCombatant, BattlePartyPanel>();

    public void CreatePartyPanel(AllyCombatant allyCombatant)
    {
        GameObject partyPanelObject = Instantiate(partyPanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        partyPanelObject.transform.SetParent(transform, false);
        BattlePartyPanel partyPanel = partyPanelObject.GetComponent<BattlePartyPanel>();
        partyPanel.AssignCombatant(allyCombatant);
        panelDict[allyCombatant] = partyPanel;

        allyCombatant.AssignBattlePartyPanel(partyPanel);
    }

    public void OnChangeHP(GameObject combatantObject, int newValue)
    {
        BattlePartyPanel panel = panelDict[combatantObject.GetComponent<AllyCombatant>()];
        if(panel)
            panel.UpdateHP(newValue);
    }

    public void onChangeMP(GameObject combatantObject, int newValue)
    {
        BattlePartyPanel panel = panelDict[combatantObject.GetComponent<AllyCombatant>()];
        if(panel)
            panel.UpdateMP(newValue);
    }
}

