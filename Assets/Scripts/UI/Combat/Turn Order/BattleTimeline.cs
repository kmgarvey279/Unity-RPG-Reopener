using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTimeline : MonoBehaviour
{
    public GameObject turnPanelPrefab;
    public TurnPanel currentTurnPanel;
    public List<TurnPanel> turnPanels = new List<TurnPanel>();
    public Color currentTurnColor;

    public void UpdateTurnList(TurnSlot currentTurnSlot, List<TurnSlot> turnOrder)
    {
        currentTurnPanel.AssignTurnSlot(currentTurnSlot);

        while(turnOrder.Count > turnPanels.Count)
        {
            GameObject turnPanelObject = Instantiate(turnPanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            turnPanelObject.transform.SetParent(this.transform, false);
            TurnPanel turnPanel = turnPanelObject.GetComponent<TurnPanel>();
            turnPanels.Add(turnPanel);
        }

        while(turnOrder.Count < turnPanels.Count)
        {
            TurnPanel panelToRemove = turnPanels[-1];
            turnPanels.Remove(panelToRemove);
            Destroy(panelToRemove.gameObject);
        }

        for(int i = 0; i < turnPanels.Count; i++)
        {
            turnPanels[i].AssignTurnSlot(turnOrder[i]);
        }
    }
}
