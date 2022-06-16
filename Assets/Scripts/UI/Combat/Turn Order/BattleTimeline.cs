using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTimeline : MonoBehaviour
{
    [Header("Current Turn")]
    [SerializeField] private Transform currentLocation;
    [SerializeField] private TurnPanel currentTurnPanel;
    [Header("Preview")]
    [SerializeField] private GameObject panelsParent; 
    // private List<TurnPanel> turnPanels = new List<TurnPanel>();
    private Dictionary<Combatant, TurnPanel> turnPanels = new Dictionary<Combatant, TurnPanel>();
    [Header("Misc")]
    [SerializeField] private GameObject turnPanelPrefab;

    [SerializeField] private List<Transform> slotLocations;

    public void UpdateTurnPanels(List<Combatant> turnForecast)
    {
        foreach(KeyValuePair<Combatant, TurnPanel> entry in turnPanels)
        {
            Vector3 updatedPosition = slotLocations[turnForecast.IndexOf(entry.Key)].position;
            if(entry.Value.transform.position != updatedPosition)
            {
                entry.Value.Move(updatedPosition);
            }
        }
    }

    public void CreateTurnPanel(int index, Combatant combatant)
    {
        //create panel
        GameObject turnPanelObject = Instantiate(turnPanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        turnPanelObject.transform.SetParent(panelsParent.transform, false);
        TurnPanel turnPanel = turnPanelObject.GetComponent<TurnPanel>();
        //assign value 
        turnPanel.AssignCombatant(combatant);
        //set position
        turnPanel.transform.position = slotLocations[index].position;
        //store in list
        turnPanels.Add(combatant, turnPanel);
    }

    public void DestroyTurnPanel(Combatant combatant)
    {
        TurnPanel turnPanel = turnPanels[combatant];
        turnPanels.Remove(combatant);
        Destroy(turnPanel.gameObject);
    }

    public void ChangePanelName(Combatant combatant, string newString)
    {
        turnPanels[combatant].AssignCombatant(combatant);
    }

    public void ChangeCurrentTurn(Combatant combatant)
    {
        if(currentTurnPanel == null)
        {
            GameObject turnPanelObject = Instantiate(turnPanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            turnPanelObject.transform.SetParent(currentLocation, false);
            currentTurnPanel = turnPanelObject.GetComponent<TurnPanel>();
        }
        currentTurnPanel.AssignCombatant(combatant);
    }

    public void ClearAllTargetingPreviews()
    {
        foreach(KeyValuePair<Combatant, TurnPanel> entry in turnPanels)
        {
           entry.Value.ToggleTargetingPreview(false); 
        }
    }

    public void OnTargetSelect(GameObject gameObject)
    {
        Combatant target = gameObject.GetComponent<Combatant>();
        turnPanels[target].ToggleTargetingPreview(true);
    }

    public void OnTargetDeselect(GameObject gameObject)
    {
        Combatant target = gameObject.GetComponent<Combatant>();
        turnPanels[target].ToggleTargetingPreview(false);
    }
}
