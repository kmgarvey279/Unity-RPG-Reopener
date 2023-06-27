using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BattleTimeline : MonoBehaviour
{
    public BattleManager battleManager;
    public Turn CurrentTurn { get; private set; }
    [field: SerializeField] public List<Turn> TurnQueue { get; private set; } = new List<Turn>();
    public bool InterventionInQueue { get; private set; } = false;

    [Header("Panels")]
    [SerializeField] private GameObject panelsParent;
    [SerializeField] private GameObject turnPanelPrefab;
    
    [Header("Slots")]
    [SerializeField] private GameObject slotLocationsParent;
    [SerializeField] private GameObject slotLocationPrefab;
    private List<RectTransform> slotLocations = new List<RectTransform>();
    private List<Turn> snapshot;
    private Turn castTemp;
    private Dictionary<Combatant, List<Turn>> combatantTurns = new Dictionary<Combatant, List<Turn>>();

    private WaitForSeconds wait02 = new WaitForSeconds(0.2f);
    private float baseCastMultiplier = 0.5f;
    //private List<TurnPanel> turnPanels = new List<TurnPanel>();    

    public void OnEnable()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject slotLocation = Instantiate(slotLocationPrefab, new Vector3(0, 0), Quaternion.identity);
            slotLocation.transform.SetParent(slotLocationsParent.transform, false);
            slotLocations.Add(slotLocation.GetComponent<RectTransform>());
        }
    }

    public IEnumerator AdvanceCo()
    {
        DisplayTurnOrder();

        while (TurnQueue[0].Counter > 0)
        {
            foreach (Turn turn in TurnQueue)
            {
                turn.Tick();
            }
            yield return null;
        }

        if(TurnQueue[0].TurnType == TurnType.Standard)
        {
            //add a new copy of combatant's turn counter to queue
            AddTurn(TurnType.Standard, TurnQueue[0].Actor, 3f);
        }
        CurrentTurn = TurnQueue[0];
        CurrentTurn.SetAsCurrentTurn();
        
        DisplayTurnOrder();
    }

    public void DisplayTurnOrder()
    {
        TurnQueue = TurnQueue.OrderBy(turn => turn.Counter).ToList();

        //go through turn queue in order and check if their panel need to change position
        for (int i = 0; i < TurnQueue.Count; i++)
        {
            bool wasChanged = TurnQueue[i].WasChanged;
            if(wasChanged)
            {
                TurnQueue[i].RemoveChangedState();
                if(TurnQueue[i].QueueIndex == i)
                {
                    wasChanged = false;
                }
            }

            //debug
            TurnQueue[i].TurnPanel.UpdateCounter(i, TurnQueue[i].QueueIndex, TurnQueue[i].Counter);
            //
            //if (TurnQueue[i].QueueIndex != i || TurnQueue[i].QueueIndex == -1)
            //{
                //is a preview modifier has been applied
                if(wasChanged && TurnQueue[i].TempModifier != 0)
                {
                    TurnQueue[i].TurnPanel.DisplayTurnPosChange(TurnQueue[i].TempModifier);
                }
                //play "swap" animation for actor or targets
                if (wasChanged)
                {
                    TurnQueue[i].TurnPanel.TriggerActorAnimation("Exit");

                }
                TurnQueue[i].SetQueueIndex(i);
                Vector2 updatedPosition = slotLocations[i].localPosition;
                StartCoroutine(MovePanelCo(TurnQueue[i], updatedPosition, wasChanged));
            //}
        }
    }

    public void TakeSnapshot()
    {
        snapshot = TurnQueue;
    }

    public void LoadSnapshot()
    {
        TurnQueue = snapshot;
        //for (int i = 0; i < TurnQueue.Count; i++)
        //{
        //    if (TurnQueue[i].QueueIndex != i)
        //    {
        //        TurnQueue[i].SetQueueIndex(i);
        //    }

        //}
        //DisplayTurnOrder();
    }

    private IEnumerator MovePanelCo(Turn turn, Vector2 newPosition, bool wasChanged)
    {
        if(turn != null)
        {
            yield return wait02;

            float counter = 0f;
            float duration = 0.2f;
            RectTransform panelRect = turn.TurnPanel.GetComponent<RectTransform>();
            //Vector2 start = panelRect.anchoredPosition;

            if (wasChanged)
            {
                yield return wait02;
                panelRect.anchoredPosition = slotLocations[turn.QueueIndex].localPosition;
            }
            else
            {
                while (turn != null && counter < duration)
                {
                    Vector2 start = panelRect.anchoredPosition;
                    panelRect.anchoredPosition = Vector3.Lerp(start, slotLocations[turn.QueueIndex].localPosition, (counter / duration));
                    counter += Time.deltaTime;
                    yield return null;
                }
                panelRect.anchoredPosition = newPosition;
            }

            if (turn != null)
            {   
                if (wasChanged)
                {
                    turn.TurnPanel.TriggerActorAnimation("Enter");
                }
                if (turn.TurnPanel.IsNew)
                {
                    turn.TurnPanel.TriggerActorAnimation("Enter");
                    turn.TurnPanel.RemoveNewStatus();
                }
                yield return null;
            }
        }
    }

    public void AddCombatant(Combatant combatant)
    {
        AddTurn(TurnType.Standard, combatant, 1f);
        AddTurn(TurnType.Standard, combatant, 2f);
        AddTurn(TurnType.Standard, combatant, 3f);
    }

    public Turn AddTurn(TurnType turnType, Combatant actor, float turnMultiplier = 1f)
    {
        //create new turn
        Turn newTurn = new Turn(turnType, actor, turnMultiplier);
        //add to queue
        TurnQueue.Add(newTurn);
        if(turnType == TurnType.Intervention)
        {
            InterventionInQueue = true;
        }
        else
        {
            //add to actor's list of turns in queue
            if (actor != null)
            {
                if (!combatantTurns.ContainsKey(actor))
                {
                    combatantTurns.Add(actor, new List<Turn>());
                }
                combatantTurns[actor].Add(newTurn);
            }
        }
        //create turn panel
        CreateTurnPanel(TurnQueue.Count - 1, newTurn);
        DisplayTurnOrder();
        return newTurn;
    }

    public void CreateTurnPanel(int index, Turn turn)
    {
        //create panel
        GameObject turnPanelObject = Instantiate(turnPanelPrefab, new Vector3(0, 0), Quaternion.identity);
        turnPanelObject.transform.SetParent(panelsParent.transform, false);
        TurnPanel turnPanel = turnPanelObject.GetComponent<TurnPanel>();
        //display actor
        if(turn.TurnType == TurnType.Intervention)
        {
            turnPanel.DisplayAsIntervention();
        }
        else
        {
            turnPanel.DisplayActor(turn.Actor);
        }
        //set position
        turnPanel.transform.position = slotLocations[index].position;
        //add to panel dictionary
        turn.SetTurnPanel(turnPanel);
    }


    public void RemoveTurn(Turn turnToRemove, bool isDead)
    {
        if(turnToRemove.Actor)
        {
            combatantTurns[turnToRemove.Actor].Remove(turnToRemove);
            if(combatantTurns[turnToRemove.Actor].Count == 0)
            {
                combatantTurns.Remove(turnToRemove.Actor);
            }
        }
        TurnQueue.Remove(turnToRemove);
        StartCoroutine(TurnPanelExit(turnToRemove.TurnPanel));
        if(turnToRemove.TurnType == TurnType.Intervention)
        {
            InterventionInQueue = false;
        }
        DisplayTurnOrder();
    }

    public void RemoveCombatant(Combatant combatant, bool isDead)
    {
        foreach (Turn turn in combatantTurns[combatant])
        {
            RemoveTurn(turn, isDead);
        }
        DisplayTurnOrder();
    }

    public Turn AddCastToQueue(Combatant actor, Action action, List<Combatant> targets)
    {
        Turn castTurn = AddTurn(TurnType.Cast, actor, baseCastMultiplier);
        castTurn.SetAction(action);
        castTurn.SetTargets(targets);

        return castTurn;
    }

    public void ResetInterventionPanel()
    {
        if (TurnQueue[0].TurnType == TurnType.Intervention)
        {
            TurnQueue[0].TurnPanel.DisplayAsIntervention();
        }
    }

    //public void AddInterventionToQueue(Combatant actor)
    //{
    //    if (!InterventionInQueue)
    //    {
    //        InterventionInQueue = true;

    //        Turn interventionTurn = AddTurn(TurnType.Intervention);
    //        CreateTurnPanel(TurnQueue.Count - 1, interventionTurn);
    //        if (actor)
    //            UpdateTurnActor(interventionTurn, actor);

    //        DisplayTurnOrder();
    //    }
    //}

    public void DisplayActionPreview(Turn turn, Action action, List<Combatant> targets, bool targetUnknown)
    {
        StartCoroutine(turn.TurnPanel.DisplayActionPreviewCo(action, targets, targetUnknown));
    }

    public void HideCurrentTurnPreview()
    {
        CurrentTurn.TurnPanel.HideActionPreview();
    }

    //called on turn creation + when selecting a character for an intervention
    public void UpdateTurnActor(Turn turn, Combatant actor)
    {
        turn.SetActor(actor);
        turn.TurnPanel.DisplayActor(actor);
    }

    //called when selecting an action in the "menu" state (or during enemy turn state)
    public void UpdateTurnAction(Turn turn, Action action)
    {
        turn.SetAction(action);
    }

    //called when selecting targets in the "target select" state (or during enemy turn state)
    public void UpdateTurnTargets(Turn turn, List<Combatant> targets)
    { 
        turn.SetTargets(targets);
    }

    public void CancelAction(Turn turn)
    {
        turn.SetAction(null);
        turn.Targets.Clear();

        HideCurrentTurnPreview();
    }

    public void UpdateCasts()
    {
        //get all casts in timeline
        List<Turn> casts = new List<Turn>();
        foreach (Turn turn in TurnQueue)
        {
            if (turn.TurnType == TurnType.Cast)
            {
                casts.Add(turn);
            }
        }
        //update casts
        for (int i = casts.Count - 1; i >= 0; i--)
        {
            List<Combatant> originalTargets = casts[i].Targets;
            List<Combatant> availableTargets = battleManager.GetCombatants(casts[i].TargetedCombatantType);

            if (availableTargets.Count == 0)
            {
                RemoveTurn(casts[i], false);
                continue;
            }
            if (casts[i].Action.AOEType == AOEType.All)
            {
                UpdateTurnTargets(casts[i], availableTargets);
                continue;
            }
            //update primary target if no longer avalable 
            if (!availableTargets.Contains(originalTargets[0]))
            {
                if (casts[i].Actor.CombatantType == CombatantType.Enemy)
                {
                    //set new target at random
                    int roll = Mathf.FloorToInt(UnityEngine.Random.Range(0, availableTargets.Count));
                    UpdateTurnTargets(casts[i], new List<Combatant>() { availableTargets[roll] });
                    DisplayActionPreview(casts[i], casts[i].Action, casts[i].Targets, false);
                }
                else
                {
                    //set first available target + trigger target select on cast turn if there is more than one option
                    UpdateTurnTargets(casts[i], new List<Combatant>() { availableTargets[0] });
                    bool canPickNewTarget = false;
                    if(availableTargets.Count > 1)
                    {
                        canPickNewTarget = true;
                        casts[i].SetReselectTargets(true);
                    }
                    DisplayActionPreview(casts[i], casts[i].Action, casts[i].Targets, canPickNewTarget);
                }
            }
        }
        DisplayTurnOrder();
    }


    private IEnumerator TurnPanelExit(TurnPanel turnPanel)
    {
        turnPanel.TriggerActorAnimation("Exit");
        yield return wait02;
        Destroy(turnPanel.gameObject);
    }

    private IEnumerator TurnPanelKill(TurnPanel turnPanel)
    {
        turnPanel.TriggerActorAnimation("Kill");
        yield return wait02;
        Destroy(turnPanel.gameObject);
    }

    public void HighlightTarget(Combatant target, bool shouldHighlight)
    {
        foreach (Turn turn in combatantTurns[target])
        {
            turn.TurnPanel.Highlight(shouldHighlight);
        }
    }

    public void ApplyTurnModifier(Combatant target, float modifier, bool isTemp, bool ignoreCasts)
    {
        foreach (Turn turn in combatantTurns[target])
        {
            if (turn.TurnType != TurnType.Intervention && turn != CurrentTurn && !(turn.TurnType == TurnType.Cast && ignoreCasts))
            {
                turn.ApplyModifier(modifier, isTemp);
            }
        }
        //DisplayTurnOrder();
    }

    public void RemoveTempTurnModifier(Combatant target)
    {
        foreach (Turn turn in combatantTurns[target])
        {
            turn.RemoveTempModifier();
            turn.TurnPanel.ClearTurnPosChange();
        }
        //DisplayTurnOrder();
    }
}
