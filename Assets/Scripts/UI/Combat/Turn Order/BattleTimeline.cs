using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTimeline : MonoBehaviour
{
    private const float panelHeight = 28f;
    private const int displayedPanelCount = 8;

    public BattleManager battleManager;
    [field: Header("Queue"), SerializeField] public List<Turn> TurnQueue { get; private set; } = new List<Turn>();
    public Turn CurrentTurn { get; private set; }
    private Dictionary<Combatant, List<Turn>> combatantTurns = new Dictionary<Combatant, List<Turn>>();
    private Dictionary<Turn, int> indexSnapshot = new Dictionary<Turn, int>();
    private List<Combatant> highlightedTargets = new List<Combatant>();

    [Header("Timeline Display")]
    [SerializeField] private GameObject panelsContainer;
    private float scrollY = 0;

    [Header("Panels")]
    [SerializeField] private GameObject panelsParent;
    [SerializeField] private GameObject turnPanelPrefab;
    
    [Header("Slots")]
    [SerializeField] private GameObject slotLocationsParent;
    [SerializeField] private GameObject slotLocationPrefab;
    [SerializeField] private List<RectTransform> slotLocations = new List<RectTransform>();

    private WaitForSeconds wait02 = new WaitForSeconds(0.2f);
    private WaitForSeconds wait05 = new WaitForSeconds(0.5f);

    #region Main
    public IEnumerator AdvanceCo()
    {
        while (TurnQueue[0].Counter > 0)
        {
            foreach (Turn turn in TurnQueue)
            {
                turn.Tick();
            }
            yield return null;
        }

        if (!TurnQueue[0].IsIntervention)
        {
            //add a new copy of combatant's turn counter to queue
            AddTurn(TurnQueue[0].Actor, 5f, false);
        }

        PopulateToMax();

        CurrentTurn = TurnQueue[0];
        CurrentTurn.SetAsCurrentTurn();

        if (CurrentTurn.IsIntervention)
        {
            PlayableCombatant playableCombatant = (PlayableCombatant)CurrentTurn.Actor;
            playableCombatant.SpendInterventionNode();
        }

        DisplayTurnOrder();
    }

    public void DisplayTurnOrder()
    {
        //sort by speed, then by turn counter
        TurnQueue = TurnQueue.OrderByDescending(turn => turn.Actor.Stats[IntStatType.Agility]).ToList();
        TurnQueue = TurnQueue.OrderBy(turn => turn.Counter).ToList();

        //go through turn queue in order and check if their panel need to change position
        for (int i = 0; i < TurnQueue.Count; i++)
        {
            bool wasChanged = false;

            if (TurnQueue[i].WasChanged)
            {
                TurnQueue[i].RemoveChangedState();
                wasChanged = true;
                TurnQueue[i].TurnPanel.TriggerActorAnimation("Exit");
                
                if (TurnQueue[i].TempModifier != 0)
                {
                    //indexSnapshot
                    TurnQueue[i].TurnPanel.DisplayTurnModifier(TurnQueue[i].TempModifier);
                }
            }
            TurnQueue[i].SetQueueIndex(i);
            //debug
            TurnQueue[i].TurnPanel.UpdateCounter(i, TurnQueue[i].QueueIndex, TurnQueue[i].Counter);
            //
            StartCoroutine(MovePanelCo(TurnQueue[i], wasChanged));
        }
    }

    private IEnumerator MovePanelCo(Turn turn, bool wasChanged)
    {
        if (turn != null)
        {
            yield return wait02;

            //hide cursor if offscreen
            bool isOnscreen = false;
            if (turn.QueueIndex < slotLocations.Count - 1)
            {
                isOnscreen = true;
            }
            turn.TurnPanel.CursorCheck(isOnscreen);

            //trigger out animation if turn modifie
            if (wasChanged)
            {
                turn.TurnPanel.TriggerActorAnimation("Out");
            }

            float counter = 0f;
            float duration = 0.2f;
            RectTransform panelRect = turn.TurnPanel.GetComponent<RectTransform>();

            //move offscreen + immediatly move to new positon
            if (wasChanged || turn.TurnPanel.IsNew)
            {
                yield return wait02;

                //yield return new WaitForSeconds(0.35f);
                if (turn.QueueIndex < slotLocations.Count)
                {
                    panelRect.anchoredPosition = slotLocations[turn.QueueIndex].localPosition;
                }
                else
                {
                    panelRect.anchoredPosition = slotLocations[slotLocations.Count - 1].localPosition;
                }
            }
            //shift up/down
            else
            {
                Vector2 start = panelRect.anchoredPosition;
                Vector2 end = slotLocations[slotLocations.Count - 1].localPosition;
                if (turn.QueueIndex < slotLocations.Count)
                {
                    end = slotLocations[turn.QueueIndex].localPosition;
                }

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

    public void PopulateToMax()
    {
        //ensure visible queue is always full
        int multiplier = 5;
        List<Combatant> actors = combatantTurns.Keys.ToList();
        while (TurnQueue.Count < displayedPanelCount && multiplier < displayedPanelCount)
        {
            //add additional turn for each actor
            foreach (Combatant actor in actors)
            {
                AddTurn(actor, multiplier, false);
            }
            multiplier++;
        }
        DisplayTurnOrder();
    }
    #endregion

    #region Snapshots
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
        //DisplayTurnOrder();
    }

    public void ClearSnapshot()
    {
        Debug.Log("clearing snapshot");
        indexSnapshot.Clear();
    }
    #endregion

    #region Add/Remove Panel
    public void CreateTurnPanel(Turn turn)
    {
        //create panel
        GameObject turnPanelObject = Instantiate(turnPanelPrefab, slotLocations[slotLocations.Count - 1].localPosition, Quaternion.identity);
        turnPanelObject.transform.SetParent(panelsParent.transform, false);
        TurnPanel turnPanel = turnPanelObject.GetComponent<TurnPanel>();
        if (turnPanel != null)
        {
            //toggle intervention border
            bool isIntervention = turn.IsIntervention;

            //link via references
            turnPanel.SetActor(turn.Actor, isIntervention);
            turn.SetTurnPanel(turnPanel);

            DisplayTurnOrder();
        }
    }
    #endregion

    #region Add/Remove Turn
    public Turn AddTurn(Combatant actor, float turnMultiplier, bool isIntervention)
    {
        //create new turn
        Turn newTurn = new Turn(actor, turnMultiplier, isIntervention);
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
        combatantTurns[actor] = combatantTurns[actor].OrderBy(turn => turn.Counter).ToList();
        //create turn panel
        CreateTurnPanel(newTurn);

        //reset highlight animations so they're synched
        ResetHighlightAnimations();

        return newTurn;
    }

    public void RemoveTurn(Turn turnToRemove, bool isDead)
    {
        //remove from queue
        TurnQueue.Remove(turnToRemove);
        //remove from combatant's dict entry
        combatantTurns[turnToRemove.Actor].Remove(turnToRemove);

        if (isDead)
        {
            StartCoroutine(TurnPanelKill(turnToRemove.TurnPanel));
        }
        else
        {
            StartCoroutine(TurnPanelExit(turnToRemove.TurnPanel));
        }
        //DisplayTurnOrder();
    }
    #endregion

    #region Add/Remove Combatant
    public void AddCombatant(Combatant combatant)
    {
        for (float i = 0; i < 5; i++)
        {
            AddTurn(combatant, i, false);
        }
    }

    public void RemoveCombatant(Combatant combatant, bool isDead)
    {
        Debug.Log("removing " + combatant.CharacterName + " " + combatant.CharacterLetter);
        if (!combatantTurns.ContainsKey(combatant))
        {
            return;
        }

        for (int i = combatantTurns[combatant].Count - 1; i >= 0; i--)
        {
            RemoveTurn(combatantTurns[combatant][i], isDead);
        }
        combatantTurns.Remove(combatant);
    }
    #endregion

    #region Swaps
    public void DisplaySwapPreview(Combatant combatant1, Combatant combatant2)
    {
        if (!combatantTurns.ContainsKey(combatant1))
        {
            return;
        }

        //store turn order of original timeline
        TakeSnapshot();

        //remove first combatant from timeline (but save entries in dict)
        for (int i = combatantTurns[combatant1].Count - 1; i >= 0; i--)
        {
            //remove from queue/timeline display
            TurnQueue.Remove(combatantTurns[combatant1][i]);
            combatantTurns[combatant1][i].OnSwapOut();
            StartCoroutine(TurnPanelSwap(combatantTurns[combatant1][i].TurnPanel));
        }

        //add second combatant to timeline
        AddCombatant(combatant2);
        Turn newCurrentTurn = combatantTurns[combatant2][0];
        newCurrentTurn.SetAsCurrentTurn();
        CurrentTurn = newCurrentTurn;

        DisplayTurnOrder();
    }

    public void CancelSwapPreview(Combatant combatant1, Combatant combatant2)
    {
        if (!combatantTurns.ContainsKey(combatant1) || !combatantTurns.ContainsKey(combatant2))
        {
            return;
        }

        //remove temp combatant
        RemoveCombatant(combatant2, false);

        //add original back to queue
        for (int i = combatantTurns[combatant1].Count - 1; i >= 0; i--)
        {
            //add to queue/timeline display
            TurnQueue.Add(combatantTurns[combatant1][i]);
        }
        CurrentTurn = combatantTurns[combatant1][0];

        //restore timeline
        LoadSnapshot();
    }

    public void PublishSwapPreview(Combatant combatant1, Combatant combatant2)
    {
        RemoveCombatant(combatant1, false);
        ClearSnapshot();
    }
    #endregion

    #region Interventions

    public void AddInterventionToQueue(Combatant actor)
    {
        Debug.Log("Adding intervention" + "(" + actor.CharacterName + ")");
        AddTurn(actor, 1f, true);

        PlayableCombatant playableCombatant = (PlayableCombatant)actor;
        if (playableCombatant)
        {
            playableCombatant.QueueIntervention();
        }
    }

    public void RemoveAllInterventions(Combatant actor)
    {
        if (actor is EnemyCombatant)
        {
            return;
        }

        List<Turn> interventions = new List<Turn>();
        for (int i = 1; i < TurnQueue.Count; i++)
        {
            if (TurnQueue[i].IsIntervention && TurnQueue[i].Actor == actor)
            {
                interventions.Add(TurnQueue[i]);
            }
            else if (!TurnQueue[i].IsIntervention)
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
        Debug.Log("removing intervention" + "(" + actor.CharacterName +")");
        
        Turn lastIntervention = null;
        for (int i = 1; i < TurnQueue.Count; i++)
        {
            if (TurnQueue[i].IsIntervention && TurnQueue[i].Actor == actor)
            {
                lastIntervention = TurnQueue[i];
            }
            else if (!TurnQueue[i].IsIntervention)
            {
                break;
            }
        }
        if (lastIntervention != null)
        {
            RemoveTurn(lastIntervention, false);
        }

        DisplayTurnOrder();
    }

    #endregion

    #region Panel Animations
    private IEnumerator TurnPanelSwap(TurnPanel turnPanel)
    {
        turnPanel.TriggerActorAnimation("Exit");
        yield return wait05;

        RectTransform panelRect = turnPanel.GetComponent<RectTransform>();
        panelRect.anchoredPosition = slotLocations[slotLocations.Count - 1].localPosition;
    }

    private IEnumerator TurnPanelExit(TurnPanel turnPanel)
    {
        turnPanel.TriggerActorAnimation("Exit");
        yield return wait05;
        DestroyImmediate(turnPanel.gameObject);
    }

    private IEnumerator TurnPanelKill(TurnPanel turnPanel)
    {
        turnPanel.TriggerActorAnimation("Kill");
        yield return wait05;
        Destroy(turnPanel.gameObject);
    }
    #endregion

    #region Panel State
    private IEnumerator ScrollTimeline()
    {
        float counter = 0f;
        float duration = 0.2f;

        RectTransform timelineRect = panelsContainer.GetComponent<RectTransform>();
        Vector2 start = timelineRect.anchoredPosition;
        Vector2 end = new Vector2(timelineRect.anchoredPosition.x, scrollY);
        
        while (timelineRect != null && counter < duration)
        {
            timelineRect.anchoredPosition = Vector3.Lerp(start, end, (counter / duration));
            counter += Time.deltaTime;
            yield return null;
        }
        timelineRect.anchoredPosition = end;
    }

    public void SelectTarget(Combatant target)
    {
        if (!combatantTurns.ContainsKey(target))
        {
            return;
        }

        int turnIndex = 0;
        for (int i = 0; i < combatantTurns[target].Count; i++)
        {
            turnIndex = TurnQueue.IndexOf(combatantTurns[target][i]);

            //shift timeline if first turn instance of selected combatant is currently offscreen
            //if (i == 0)
            //{
            //    if (turnIndex > displayedPanelCount - 1)
            //    {
            //        scrollY = (turnIndex - (displayedPanelCount - 1)) * panelHeight;
            //    }
                //StartCoroutine(ScrollTimeline());
            //}

            //combatantTurns[target][i].TurnPanel.SetSelected(true);

            //set cursor
            if (turnIndex < slotLocations.Count - 1)
            {
                combatantTurns[target][i].TurnPanel.DisplayCursor();
            //    combatantTurns[target][i].TurnPanel.SetSelected(true);
            }
        }
    }

    public void UnselectTarget(Combatant target)
    {
        if (!combatantTurns.ContainsKey(target))
        {
            return;
        }

        foreach (Turn turn in combatantTurns[target])
        {
            turn.TurnPanel.HideCursor();
        }

        if (scrollY != 0)
        {
            scrollY = 0;
            //StartCoroutine(ScrollTimeline());
        }
    }

    public void HighlightTarget(Combatant target)
    {
        if (!combatantTurns.ContainsKey(target))
        {
            return;
        }

        //highlight
        foreach (Turn turn in combatantTurns[target])
        {
            turn.TurnPanel.Highlight();
        }

        if (!highlightedTargets.Contains(target))
        {
            highlightedTargets.Add(target);
        }
    }

    public void UnhighlightTarget(Combatant target)
    {
        if (!combatantTurns.ContainsKey(target))
        {
            return;
        }

        foreach (Turn turn in combatantTurns[target])
        {
            turn.TurnPanel.Unhighlight();
        }

        if (highlightedTargets.Contains(target))
        {
            highlightedTargets.Remove(target);
        }
    }

    private void ResetHighlightAnimations()
    {
        foreach (Turn turn in TurnQueue)
        {
            if (highlightedTargets.Contains(turn.Actor))
            {
                turn.TurnPanel.Highlight();
            }
        }
    }

    public void ToggleStunState(Combatant combatant, bool isStunned)
    {
        Debug.Log("Target is stunned!");
        if (!combatantTurns.ContainsKey(combatant) || combatantTurns[combatant].Count == 0)
        {
            return;
        }

        for (int i = 0; i < combatantTurns[combatant].Count; i++)
        {
            if (combatantTurns[combatant][i] != CurrentTurn)
            {
                combatantTurns[combatant][0].TurnPanel.ToggleStunFilter(isStunned);
                break;
            }
        }
    }

    public void ToggleKOState(Combatant combatant, bool isKOed)
    {
        if (!combatantTurns.ContainsKey(combatant))
        {
            return;
        }

        for (int i = combatantTurns[combatant].Count - 1; i >= 0; i--)
        {
            Turn turn = combatantTurns[combatant][i];

            if (turn.IsIntervention)
            {
                RemoveTurn(turn, true);
            }
            else
            {
                turn.TurnPanel.ToggleKOFilter(isKOed);
            }
        }
    }
    #endregion

    #region Turn Modifiers
    public void ApplyTurnModifier(Combatant target, float modifier, bool isTemp, bool applyToNextTurnOnly)
    {
        foreach (Turn turn in combatantTurns[target])
        {
            //ignore interventions
            if (turn.IsIntervention)
                continue;
            //ignore current turn
            if (turn == CurrentTurn)
                continue;
            //if modifier won't be applied until after this turn:
            //if (TurnQueue.IndexOf(turn) < timelineOccurrence)
            //    continue;

            turn.ApplyModifier(modifier, isTemp, applyToNextTurnOnly);

            if (applyToNextTurnOnly)
            {
                break;
            }
        }
        //DisplayTurnOrder();
    }

    public void RemoveOneTurnModifiers(Combatant target)
    {
        if (!combatantTurns.ContainsKey(target))
        {
            return;
        }

        foreach (Turn turn in combatantTurns[target])
        {
            turn.RemoveOneTurnModifiers();
        }
        DisplayTurnOrder();
    }

    public void RemoveTempTurnModifier(Combatant target)
    {
        if (!combatantTurns.ContainsKey(target))
        {
            return;
        }

        foreach (Turn turn in combatantTurns[target])
        {
            turn.RemoveTempModifier();
            turn.TurnPanel.ClearTurnPosChange();
        }
        DisplayTurnOrder();
    }
    #endregion

    public int GetNextTurnIndex(Combatant combatant, bool includeInterventions)
    {
        int index = 0;

        foreach (Turn turn in TurnQueue)
        {
            if (includeInterventions || !includeInterventions && !turn.IsIntervention)
            {
                index++;

                if (turn.Actor == combatant)
                {
                    return index;
                }
            }
        }
        return -1;
    }
}
