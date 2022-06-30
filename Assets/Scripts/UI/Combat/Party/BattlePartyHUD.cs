using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePartyHUD : MonoBehaviour
{
    [SerializeField] private GameObject partyPanelPrefab;
    private Dictionary<PlayableCombatant, BattlePartyPanel> panelDict = new Dictionary<PlayableCombatant, BattlePartyPanel>();

    public void CreatePartyPanel(PlayableCombatant playableCombatant)
    {
        GameObject partyPanelObject = Instantiate(partyPanelPrefab, transform.position, Quaternion.identity);
        partyPanelObject.transform.SetParent(transform, false);
        BattlePartyPanel partyPanel = partyPanelObject.GetComponent<BattlePartyPanel>();
        partyPanel.AssignCombatant(playableCombatant);
        panelDict[playableCombatant] = partyPanel;

        playableCombatant.AssignBattlePartyPanel(partyPanel);
    }

    public void OnChangeHP(GameObject combatantObject, int newValue)
    {
        BattlePartyPanel panel = panelDict[combatantObject.GetComponent<PlayableCombatant>()];
        if(panel)
            panel.UpdateHP(newValue);
    }

    public void onChangeMP(GameObject combatantObject, int newValue)
    {
        BattlePartyPanel panel = panelDict[combatantObject.GetComponent<PlayableCombatant>()];
        if(panel)
            panel.UpdateMP(newValue);
    }
}

