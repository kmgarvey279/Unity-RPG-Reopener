using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePartyHUD : MonoBehaviour
{ 
    [SerializeField] private GameObject partyPanelPrefab;
    private List<BattlePartyPanel> panels = new List<BattlePartyPanel>();

    public void CreatePartyPanel(PlayableCombatant playableCombatant, int panelNum)
    {
        GameObject partyPanelObject = Instantiate(partyPanelPrefab, transform.position, Quaternion.identity);
        partyPanelObject.transform.SetParent(transform, false);
        BattlePartyPanel partyPanel = partyPanelObject.GetComponent<BattlePartyPanel>();
        partyPanel.SetIndex(panelNum);
        partyPanel.AssignCombatant(playableCombatant);
        panels.Add(partyPanel);

        playableCombatant.AssignBattlePartyPanel(partyPanel);
    }

    public void ApplyFilter(PlayableCharacterID playableCharacterID)
    {
        foreach (BattlePartyPanel battlePartyPanel in panels)
        {
            if (battlePartyPanel.PlayableCombatant.PlayableCharacterID != playableCharacterID)
            {
                battlePartyPanel.gameObject.SetActive(false);
            }
        }
    }

    public void RemoveFilters()
    {
        foreach (BattlePartyPanel battlePartyPanel in panels)
        {
            battlePartyPanel.gameObject.SetActive(true);
        }
    }

    public BattlePartyPanel GetPanel(PlayableCharacterID playableCharacterID)
    {
        foreach (BattlePartyPanel battlePartyPanel in panels)
        {
            if (battlePartyPanel.PlayableCombatant.PlayableCharacterID == playableCharacterID)
            {
                return battlePartyPanel;
            }
        }
        return null;
    }

    public void Clear()
    {
        for (int i = panels.Count - 1; i >= 0; i--)
        {
            Destroy(panels[i].gameObject);
        }
        panels.Clear();
    }

    public void ClearAllHighlights()
    {
        foreach (BattlePartyPanel battlePartyPanel in panels)
        {
            battlePartyPanel.OnTurnEnd();
        }
    }
}

