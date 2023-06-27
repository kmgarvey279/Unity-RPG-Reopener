using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePartyHUD : MonoBehaviour
{ 
    [SerializeField] private GameObject partyPanelPrefab;
    private List<BattlePartyPanel> panels = new List<BattlePartyPanel>();

    public void CreatePartyPanel(PlayableCombatant playableCombatant)
    {
        GameObject partyPanelObject = Instantiate(partyPanelPrefab, transform.position, Quaternion.identity);
        partyPanelObject.transform.SetParent(transform, false);
        BattlePartyPanel partyPanel = partyPanelObject.GetComponent<BattlePartyPanel>();
        partyPanel.AssignCombatant(playableCombatant);
        panels.Add(partyPanel);

        playableCombatant.AssignBattlePartyPanel(partyPanel);
    }
}

