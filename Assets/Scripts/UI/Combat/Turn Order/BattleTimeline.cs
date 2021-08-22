using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTimeline : MonoBehaviour
{
    [Header("Current Turn")]
    [SerializeField] private TurnPanel currentTurnPanel;
    [Header("Preview")]
    [SerializeField] private GameObject panelsParent; 
    // private List<TurnPanel> turnPanels = new List<TurnPanel>();
    private Dictionary<TurnSlot, TurnPanel> turnPanels = new Dictionary<TurnSlot, TurnPanel>();
    [Header("Misc")]
    [SerializeField] private GameObject turnPanelPrefab;

    [SerializeField] private List<Transform> slotLocations;

    public void CreateTurnPanels(List<TurnSlot> turnForecast)
    {
        for(int i = 0; i < turnForecast.Count; i++)
        {
            CreateTurnPanel(i, turnForecast[i]);
        }
    }

    public void UpdateTurnPanels(List<TurnSlot> turnForecast)
    {
        foreach(KeyValuePair<TurnSlot, TurnPanel> entry in turnPanels)
        {
            entry.Value.UpdateSliderValue();
            Vector3 updatedPosition = slotLocations[turnForecast.IndexOf(entry.Key)].position;
            if(entry.Value.transform.position != updatedPosition)
            {
                entry.Value.Move(updatedPosition);
            }
        }
    }

    private void CreateTurnPanel(int index, TurnSlot turnSlot)
    {
        //create panel
        GameObject turnPanelObject = Instantiate(turnPanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        turnPanelObject.transform.SetParent(panelsParent.transform, false);
        TurnPanel turnPanel = turnPanelObject.GetComponent<TurnPanel>();
        //assign value 
        turnPanel.AssignTurnSlot(turnSlot);
        //set position
        turnPanel.transform.position = slotLocations[index].position;
        //store in list
        turnPanels.Add(turnSlot, turnPanel);
    }

    private void MoveTurnPanel(TurnPanel turnPanel)
    {

    }

    public void RemoveTurnPanel()
    {
    }

    public void ChangeCurrentTurn(TurnSlot currentTurnSlot)
    {
        currentTurnPanel.AssignTurnSlot(currentTurnSlot);
    }

    public void DisplayAccuracyPreview(TurnSlot turnSlot, int accuracy)
    {
        turnPanels[turnSlot].DisplayAccuracyPreview(accuracy);
    }

    public void ClearAccuracyPreview(TurnSlot turnSlot)
    {
        turnPanels[turnSlot].ClearAccuracyPreview();
    }

    public void ClearAllTargeted()
    {
        foreach(KeyValuePair<TurnSlot, TurnPanel> entry in turnPanels)
        {
           entry.Value.ClearAccuracyPreview(); 
        }
    }
}
