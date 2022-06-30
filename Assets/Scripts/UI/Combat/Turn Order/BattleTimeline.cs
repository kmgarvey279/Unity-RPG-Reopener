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

    public IEnumerator RemoveCombatants(List<Combatant> combatantsToRemove, List<Combatant> turnForecast)
    {
        foreach(Combatant combatant in combatantsToRemove)
        {
            turnPanels[combatant].animatorMovement.SetTrigger("Out");
        }

        yield return new WaitForSeconds(0.5f);

        foreach(Combatant combatant in combatantsToRemove)
        {
            DestroyTurnPanel(combatant);
        }

        UpdateTurnPanels(turnForecast);
    }
    
    public IEnumerator AddCombatants(List<Combatant> combatantsToAdd, List<Combatant> turnForecast)
    {
        UpdateTurnPanels(turnForecast);
        
        yield return new WaitForSeconds(0.5f);
        
        foreach(Combatant combatant in combatantsToAdd)
        {
            CreateTurnPanel(turnForecast.IndexOf(combatant), combatant);
            turnPanels[combatant].animatorMovement.SetTrigger("In");
        }
    }
    
    public IEnumerator SwapCombatants(List<Combatant> combatantsToSwap, List<Combatant> turnForecast)
    {
        List<Combatant> removedCombatants = new List<Combatant>();
        foreach(Combatant combatant in combatantsToSwap)
        {
            Vector3 updatedPosition = slotLocations[turnForecast.IndexOf(combatant)].position;
            if(turnPanels[combatant].transform.position != updatedPosition)
            {
                turnPanels[combatant].animatorMovement.SetTrigger("Out");
                removedCombatants.Add(combatant);
            }
        }

        yield return new WaitForSeconds(0.5f);

        UpdateTurnPanels(turnForecast);

        // yield return new WaitForSeconds(0.1f);

        foreach(Combatant combatant in removedCombatants)
        {            
            turnPanels[combatant].animatorMovement.SetTrigger("In");
        }
    }

    //cancel previous animation when toggling through previews
    public void CancelAnimations(List<Combatant> turnForecast)
    {
        foreach(KeyValuePair<Combatant, TurnPanel> entry in turnPanels)
        {
            entry.Value.animatorMovement.SetTrigger("Default");
            Vector3 updatedPosition = slotLocations[turnForecast.IndexOf(entry.Key)].position;
            if(entry.Value.transform.position != updatedPosition)
            {
                entry.Value.transform.position = updatedPosition;
            }
        }
    }

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
        GameObject turnPanelObject = Instantiate(turnPanelPrefab, new Vector3(0, 0), Quaternion.identity);
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
        currentTurnPanel.ToggleTargetingPreview(false);
        foreach(KeyValuePair<Combatant, TurnPanel> entry in turnPanels)
        {
           entry.Value.ToggleTargetingPreview(false); 
        }
    }

    public void OnCombatantSelect(GameObject gameObject)
    {
        Combatant target = gameObject.GetComponent<Combatant>();
        turnPanels[target].ToggleSelectCursor(true);
        if(currentTurnPanel.combatant == target)
        {
            currentTurnPanel.ToggleSelectCursor(true);
        }
    }

    public void OnCombatantDeselect(GameObject gameObject)
    {
        Combatant target = gameObject.GetComponent<Combatant>();
        turnPanels[target].ToggleSelectCursor(false);
        if(currentTurnPanel.combatant == target)
        {
            currentTurnPanel.ToggleSelectCursor(false);
        }
    }

    public void HighlightTargets(List<Combatant> targets)
    {
        ClearAllTargetingPreviews();
        if(targets.Contains(currentTurnPanel.combatant))
        {
            currentTurnPanel.ToggleTargetingPreview(true);
        }
        foreach(Combatant target in targets)
        {
            turnPanels[target].ToggleTargetingPreview(true);
        }
    }
}
