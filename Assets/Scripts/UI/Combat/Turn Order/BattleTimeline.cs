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

    public void UpdateTurnPanels(List<TurnSlot> turnForecast)
    {
        foreach(KeyValuePair<TurnSlot, TurnPanel> entry in turnPanels)
        {
            Vector3 updatedPosition = slotLocations[turnForecast.IndexOf(entry.Key)].position;
            if(entry.Value.transform.position != updatedPosition)
            {
                entry.Value.Move(updatedPosition);
            }
        }
    }

    public void CreateTurnPanel(int index, TurnSlot turnSlot)
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

    public void DestroyTurnPanel(TurnSlot turnSlot)
    {
        TurnPanel turnPanel = turnPanels[turnSlot];
        turnPanels.Remove(turnSlot);
        Destroy(turnPanel.gameObject);
    }

    public void ChangeCurrentTurn(TurnSlot newCurrentSlot)
    {
        currentTurnPanel.AssignTurnSlot(newCurrentSlot);
    }

    public void ToggleTargetingPreview(TurnSlot turnSlot, bool isTargeted)
    {
        turnPanels[turnSlot].ToggleTargetingPreview(isTargeted);
    }

    public void ClearAllTargetingPreviews()
    {
        foreach(KeyValuePair<TurnSlot, TurnPanel> entry in turnPanels)
        {
           entry.Value.ToggleTargetingPreview(false); 
        }
    }
}
