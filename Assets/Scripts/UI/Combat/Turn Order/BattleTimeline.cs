using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BattleTimeline : MonoBehaviour
{
    public BattleManager battleManager;
    [field: Header("Queue"), SerializeField] public List<Turn> TurnQueue { get; private set; } = new List<Turn>();
    public Turn CurrentTurn { get; private set; }
    private Dictionary<Combatant, List<Turn>> combatantTurns = new Dictionary<Combatant, List<Turn>>();
    private Dictionary<Turn, int> indexSnapshot = null;

    [Header("Panels")]
    [SerializeField] private GameObject panelsParent;
    [SerializeField] private GameObject turnPanelPrefab;
    
    [Header("Slots")]
    [SerializeField] private GameObject slotLocationsParent;
    [SerializeField] private GameObject slotLocationPrefab;
    private List<RectTransform> slotLocations = new List<RectTransform>();

    [Header("Chains")]
    private float ChainBonus = 1f;
    public CombatantType chainType { get; private set; } = CombatantType.None;
    public Combatant lastChainActor { get; private set; }
    [SerializeField] private ChainBonusDisplay chainBonusDisplay;
    [Header("Intervention")]
    [SerializeField] private InterventionPoints interventionPoints;
    //misc.
    private WaitForSeconds wait02 = new WaitForSeconds(0.2f);
    private float baseCastMultiplier = 0.5f;
  

    public void OnEnable()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject slotLocation = Instantiate(slotLocationPrefab, new Vector3(0, 0), Quaternion.identity);
            slotLocation.transform.SetParent(slotLocationsParent.transform, false);
            slotLocations.Add(slotLocation.GetComponent<RectTransform>());
        }
    }

    private void Start()
    {
        UpdateInterventionPoints(100);
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

        //check chains
        if (CurrentTurn.TurnType != TurnType.Intervention)
        {
            //increase bonus
            if (CurrentTurn.Actor.CombatantType == chainType && CurrentTurn.Actor != lastChainActor)
            {
                UpdateChainBonus(ChainBonus += 0.1f, CurrentTurn.Actor.CombatantType);
            }
            //reset bonus
            else
            {
                UpdateChainBonus(1f, CurrentTurn.Actor.CombatantType);
            }
        }
        lastChainActor = CurrentTurn.Actor;

        DisplayTurnOrder();
    }

    public void DisplayTurnOrder()
    {
        TurnQueue = TurnQueue.OrderBy(turn => turn.Counter).ToList();

        //go through turn queue in order and check if their panel need to change position
        for (int i = 0; i < TurnQueue.Count; i++)
        {
            bool wasChanged = false;

            if (TurnQueue[i].WasChanged)
            {
                TurnQueue[i].RemoveChangedState();
                if(TurnQueue[i].QueueIndex != i && i < 8)
                {
                    wasChanged = true;
                    TurnQueue[i].TurnPanel.TriggerActorAnimation("Exit");
                }
            }
            TurnQueue[i].SetQueueIndex(i);
            //debug
            TurnQueue[i].TurnPanel.UpdateCounter(i, TurnQueue[i].QueueIndex, TurnQueue[i].Counter);
            //
            StartCoroutine(MovePanelCo(TurnQueue[i], wasChanged));
        }
    }

    public void TakeSnapshot()
    {
        Debug.Log("saving snapshot");
        indexSnapshot = new Dictionary<Turn, int>();
        for (int i = 0; i < TurnQueue.Count; i++)
        {
            indexSnapshot.Add(TurnQueue[i], i);
        }
    }

    public void LoadSnapshot()
    {
        Debug.Log("Loading snapshot");
        TurnQueue = TurnQueue.OrderBy(turn => indexSnapshot[turn]).ToList();
        DisplayTurnOrder();
    }

    public void ClearSnapshot()
    {
        Debug.Log("clearing snapshot");
        indexSnapshot.Clear();
    }

    private IEnumerator MovePanelCo(Turn turn, bool wasChanged)
    {
        if(turn != null)
        {
            yield return wait02;

            float counter = 0f;
            float duration = 0.2f;
            RectTransform panelRect = turn.TurnPanel.GetComponent<RectTransform>();

            if (wasChanged || turn.TurnPanel.IsNew)
            {
                yield return wait02;
                panelRect.anchoredPosition = slotLocations[turn.QueueIndex].localPosition;
            }
            else
            {
                Vector2 start = panelRect.anchoredPosition;
                Vector2 end = slotLocations[turn.QueueIndex].localPosition;
                while (panelRect != null && counter < duration)
                {
                    panelRect.anchoredPosition = Vector3.Lerp(start, end, (counter / duration));
                    counter += Time.deltaTime;
                    yield return null;
                }
                if (panelRect != null)
                {
                    panelRect.anchoredPosition = end;
                }
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
        //add to snapshot (if being used)
        if (indexSnapshot != null)
        {
            indexSnapshot.Add(newTurn, indexSnapshot.Count - 1);
        }
        if (actor != null && !combatantTurns.ContainsKey(actor))
        {
            combatantTurns.Add(actor, new List<Turn>());
        }
        combatantTurns[actor].Add(newTurn);
        //create turn panel
        CreateTurnPanel(TurnQueue.Count - 1, newTurn);
        return newTurn;
    }

    public void CreateTurnPanel(int index, Turn turn)
    {
        //create panel
        GameObject turnPanelObject = Instantiate(turnPanelPrefab, new Vector3(0, 0), Quaternion.identity);
        turnPanelObject.transform.SetParent(panelsParent.transform, false);
        TurnPanel turnPanel = turnPanelObject.GetComponent<TurnPanel>();
        //display actor
        bool isIntervention = false;
        if (turn.TurnType == TurnType.Intervention)
        {
            isIntervention = true;
        }
        turnPanel.SetActor(turn.Actor, isIntervention);
        //add to panel dictionary
        turn.SetTurnPanel(turnPanel);
        DisplayTurnOrder();
    }

    public void ChangeCurrentCombatant(Combatant actor)
    {
        //create new turn
        Turn newTurn = new Turn(TurnType.Standard, actor, 1f);
        newTurn.SetAsCurrentTurn();
        CurrentTurn = newTurn;
        TurnQueue.Add(newTurn);
        //add to queue
        if (actor != null && !combatantTurns.ContainsKey(actor))
        {
            combatantTurns.Add(actor, new List<Turn>());
        }
        combatantTurns[actor].Add(newTurn);
        //create turn panel
        CreateTurnPanel(TurnQueue.Count - 1, newTurn);
    }


    public void RemoveTurn(Turn turnToRemove, bool isDead)
    {
        //remove from combatant's dict entry
        combatantTurns[turnToRemove.Actor].Remove(turnToRemove);
        //remove from queue
        TurnQueue.Remove(turnToRemove);
        //remove from snapshot
        if (indexSnapshot != null)
        {
            indexSnapshot.Remove(turnToRemove);
        }
        //refund intervention points
        if (turnToRemove.TurnType == TurnType.Intervention && turnToRemove != CurrentTurn)
        {
            UpdateInterventionPoints(25);
        }
        if(isDead)
        {
            StartCoroutine(TurnPanelKill(turnToRemove.TurnPanel));
        }
        else
        {
            StartCoroutine(TurnPanelExit(turnToRemove.TurnPanel));
        }
        DisplayTurnOrder();
    }

    public void RemoveCombatant(Combatant combatant, bool isDead)
    {
        for (int i = combatantTurns[combatant].Count - 1; i >= 0; i--)
        {
            RemoveTurn(combatantTurns[combatant][i], isDead);
        }
    }

    #region Casts

    public Turn AddCastToQueue(Combatant actor, Action action, List<Combatant> targets)
    {
        Turn castTurn = AddTurn(TurnType.Cast, actor, baseCastMultiplier);
        castTurn.SetAction(action);
        castTurn.SetTargets(targets);
        RemoveInterventions(actor);

        return castTurn;
    }

    #endregion

    #region Chains

    public void UpdateChainBonus(float newValue, CombatantType combatantType)
    {
        ChainBonus = newValue;
        chainType = combatantType;
        chainBonusDisplay.UpdateDisplay(newValue, combatantType);
    }

    #endregion

    #region Interventions

    public void AddInterventionToQueue(Combatant actor)
    {
        if (interventionPoints.Value >= 25)
        {
            if (actor != null && !actor.IsKOed)
            {
                AddTurn(TurnType.Intervention, actor);
                UpdateInterventionPoints(-25);
            }
            Debug.Log("invalid actor for intervention");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }

    public void UpdateInterventionPoints(int change)
    {
        interventionPoints.UpdateValue(change);
    }

    public void RemoveInterventions(Combatant actor)
    {
        List<Turn> interventions = new List<Turn>();
        for (int i = 1; i < TurnQueue.Count; i++)
        {
            if (TurnQueue[i].TurnType == TurnType.Intervention && TurnQueue[i].Actor == actor)
            {
                interventions.Add(TurnQueue[i]);
            }
            else if (TurnQueue[i].TurnType != TurnType.Intervention)
            {
                break;
            }
        }
        foreach (Turn turn in interventions)
        {
            RemoveTurn(turn, false);
        }
    }

    public void RemoveLastIntervention(Combatant actor)
    {
        Turn lastIntervention = null;
        for (int i = 1; i < TurnQueue.Count; i++)
        {
            if (TurnQueue[i].TurnType == TurnType.Intervention && TurnQueue[i].Actor == actor)
            {
                lastIntervention = TurnQueue[i];
            }
            else if (TurnQueue[i].TurnType != TurnType.Intervention)
            {
                break;
            }
        }
        if(lastIntervention != null)
        {
            RemoveTurn(lastIntervention, false);
        }
    }

    #endregion

    public void DisplayActionPreview(Turn turn, Action action, List<Combatant> targets, bool targetUnknown)
    {
        StartCoroutine(turn.TurnPanel.DisplayActionPreviewCo(action, targets, targetUnknown));
    }

    public void HideCurrentTurnPreview()
    {
        CurrentTurn.TurnPanel.HideActionPreview();
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

    public void ApplyTurnModifier(Combatant target, float modifier, bool isTemp, bool ignoreCasts, int actionIndex)
    {
        foreach (Turn turn in combatantTurns[target])
        {
            if (turn.TurnType != TurnType.Intervention 
                && turn != CurrentTurn 
                && !(turn.TurnType == TurnType.Cast && ignoreCasts)
                && TurnQueue.IndexOf(turn) > actionIndex)
            {
                turn.ApplyModifier(modifier, isTemp);
            }
        }
        DisplayTurnOrder();
    }

    public void RemoveTempTurnModifier(Combatant target)
    {
        foreach (Turn turn in combatantTurns[target])
        {
            turn.RemoveTempModifier();
            turn.TurnPanel.ClearTurnPosChange();
        }
        DisplayTurnOrder();
    }
}
