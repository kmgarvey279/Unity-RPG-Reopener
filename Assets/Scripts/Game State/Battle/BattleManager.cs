using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using StateMachineNamespace;

public class BattleManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private PartyData partyData;
    [SerializeField] private EnemyPartyData enemyPartyData;

    [Header("Combatants and Grid")]
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private GridManager gridManager;
    private Dictionary<string, int> enemyInstances = new Dictionary<string, int>();
    [SerializeField] private PlayableCharacterSpawner playableCharacterSpawner;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Intervention Display")]
    [SerializeField] private InterventionPointsDisplay interventionPointsDisplay;

    [Header("Turn Order Display")]
    [SerializeField] private BattleTimeline battleTimeline;
    
    [Header("Battle Event Queue")]
    [SerializeField] private BattleEventQueue battleEventQueue;
    
    [Header("States")]
    [SerializeField] private StateMachine stateMachine;

    [Header("Signals")]
    [SerializeField] private SignalSender onScreenFadeIn;
    [SerializeField] private SignalSender onScreenFadeOut;
    [SerializeField] private SignalSender onInterventionEnd;

    //cache wait for seconds
    private WaitForSeconds waitZeroPointTwoFive = new WaitForSeconds(0.25f);
    private WaitForSeconds waitZeroPointFive = new WaitForSeconds(0.5f);
    private WaitForSeconds waitOne = new WaitForSeconds(1f);

    //[field: SerializeField] public List<Turn> TurnQueue { get; private set; } = new List<Turn>();
    public int InterventionPoints { get; private set; } = 0;
    public bool InterventionInQueue { get; private set; } = false;
    public List<Combatant> Combatants { get; private set; } = new List<Combatant>();
    public List<Combatant> KOPlayableCombatants { get; private set; } = new List<Combatant>();
    public bool BattleIsLoaded { get; private set; } = false;

    #region Setup
    public IEnumerator StartBattleCo()
    {
        yield return null;
        UpdateInterventionPoints(55);
        yield return StartCoroutine(SpawnCombatants());
        Debug.Log("Combatants spawned.");
        onScreenFadeIn.Raise();
        yield return waitOne;
    }

    public IEnumerator SpawnCombatants()
    {
        List<PlayableCombatant> spawnedPlayableCombatants = new List<PlayableCombatant>();
        for (int i = 0; i < 3; i++)
        {
            PlayableCharacterID playableCharacterID = partyData.PartySlots[i];
            PlayableCharacterID linkedCharacterID = partyData.PartySlots[i + 3];

            if (playableCharacterID != PlayableCharacterID.None)
            {
                PlayableCharacterInfo playableCharacterInfo = partyData.GetSlotInfo(i);
                Vector2Int startPos = partyData.SpawnPositions[spawnedPlayableCombatants.Count];
                Tile startTile = gridManager.GetTileArray(CombatantType.Player)[startPos.x, startPos.y];
                PlayableCombatant playableCombatant = playableCharacterSpawner.SpawnPlayableCharacter(playableCharacterInfo, startTile, linkedCharacterID);
                if (playableCombatant != null)
                {
                    spawnedPlayableCombatants.Add(playableCombatant);
                    AddCombatant(playableCombatant);
                    battlePartyHUD.CreatePartyPanel(playableCombatant);
                }
            }
        }
        for (int i = 0; i < enemyPartyData.Enemies.Count; i++)
        {
            if (enemyPartyData.Enemies[i])
            {
                EnemyCombatant enemyCombatant = enemySpawner.SpawnEnemy(enemyPartyData.Enemies[i], i);
                AddCombatant(enemyCombatant);
            }
        }
        yield return null;
    }
    #endregion

    #region Turns/Timeline

    //public IEnumerator AdvanceTimelineCo()
    //{
    //    //check if all combatants are dead 
    //    if (battleTimeline.TurnQueue.Count < 0)
    //    {
    //        stateMachine.ChangeState((int)BattleStateType.BattleEnd);
    //        yield break;
    //    }

    //    yield return StartCoroutine(battleTimeline.AdvanceCo());

    //    //get next turn
    //    Turn currentTurn = battleTimeline.TurnQueue[0].Turn;
    //    currentTurn.SetAsCurrentTurn();

    //    //if next turn is an intervention:
    //    if (CurrentTurn.TurnType == TurnType.Intervention)
    //    {
    //        StartIntervention();
    //    }
    //    //if next turn was "paused" due to an intervention, use saved turn data
    //    else if (CurrentTurn.TurnType == TurnType.Paused)
    //    {
    //        stateMachine.ChangeState((int)BattleStateType.Menu);
    //        //ResumeTurn();
    //    }
    //    //if next turn is a queued cast:
    //    else if (CurrentTurn.TurnType == TurnType.Cast)
    //    {
    //        StartQueuedCast();
    //    }
    //    else
    //    {
    //        stateMachine.ChangeState((int)BattleStateType.TurnStart);
    //    }
    //}

    //public void SetCurrentTurn(Turn turn)
    //{
    //    turn = CurrentTurn;
    //}

    //private void ResumeTurn()
    //{
    //    stateMachine.ChangeState((int)BattleStateType.Menu);
    //    //UpdateTurnOrder();
    //}

    //public void UpdateTurnOrder()
    //{
    //    TurnQueue = TurnQueue.OrderBy(c => c.Counter).ToList();
    //    battleTimeline.UpdateTurnPanels(TurnQueue);
    //}
    #endregion

    #region Casts
    //public Turn AddCastToQueue(ActionEvent actionEvent)
    //{
    //    Turn castTurn = new Turn(TurnType.Cast, actionEvent.Actor);
    //    castTurn.SetActionEvent(actionEvent);
    //    castTurn.ApplyTurnCost(0.4f);
    //    TurnQueue.Add(castTurn);
    //    battleTimeline.CreateTurnPanel(TurnQueue.Count - 1, castTurn);
    //    UpdateTurnOrder();
    //    return castTurn;
    //}

    //refresh cast targets when a targets dies or two playable characters swap
    //public void UpdateCasts()
    //{
    //    //get all casts in timeline
    //    List<Turn> casts = new List<Turn>();
    //    foreach (Turn turn in TurnQueue)
    //    {
    //        if (turn.TurnType == TurnType.Cast)
    //        {
    //            casts.Add(turn);
    //        }
    //    }
    //    //update casts
    //    for (int i = casts.Count - 1; i >= 0; i--)
    //    {
    //        Turn castTurn = casts[i];

    //        List<Combatant> originalTargets = casts[i].ActionEvent.Targets;
    //        List<Combatant> availableTargets = GetCombatants(casts[0].ActionEvent.TargetedCombatantType);

    //        if (availableTargets.Count == 0)
    //        {
    //            return;
    //        }

    //        //update primary target if no longer avalable 
    //        if (!availableTargets.Contains(originalTargets[0]))
    //        {
    //            if (castTurn.Actor.CombatantType == CombatantType.Enemy)
    //            {
    //                int roll = Mathf.FloorToInt(UnityEngine.Random.Range(0, availableTargets.Count));
    //                castTurn.ActionEvent.SetTargets(new List<Combatant>() { availableTargets[roll] });
    //            }
    //            else
    //            {
    //                castTurn.SetReselectTargets(true);
    //            }
    //            battleTimeline.UpdateTargets(castTurn);
    //        }
    //    }
    //}

    //private void StartQueuedCast()
    //{
    //    //move on to next turn if the cast can't occur
    //    if(CurrentTurn.ActionEvent == null)
    //    {
    //        Debug.Log("error: no action event assigned to cast");
    //        battleTimeline.RemoveCurrentTurn();
    //        StartCoroutine(AdvanceTimelineCo());
    //        return;
    //    }

    //    if (CurrentTurn.Actor.CombatantType == CombatantType.Enemy)
    //    {
    //        //recheck avalable targets
    //        if (CurrentTurn.ActionEvent.Action.AOEType == AOEType.All)
    //        {
    //            CurrentTurn.ActionEvent.SetTargets(GetCombatants(CurrentTurn.ActionEvent.TargetedCombatantType));
    //            stateMachine.ChangeState((int)BattleStateType.Execute);
    //        }
    //    }
    //    else
    //    {
    //        //select a new target if the orginal target is dead
    //        if (CurrentTurn.ReselectTargets) 
    //        {
    //            CurrentTurn.ActionEvent.ClearTargets();
    //            stateMachine.ChangeState((int)BattleStateType.TargetSelect);
    //        }
    //        //otherwise, recheck targets in aoe range
    //        else
    //        {
    //            CurrentTurn.ActionEvent.SetTargets(gridManager.GetTargets(CurrentTurn.ActionEvent.Targets[0].Tile, CurrentTurn.ActionEvent.Action.AOEType, CurrentTurn.ActionEvent.Action.IsMelee, CurrentTurn.ActionEvent.TargetedCombatantType));
    //            stateMachine.ChangeState((int)BattleStateType.Execute);
    //        }
    //    }
    //    if(CurrentTurn.ActionEvent.Targets.Count == 0)
    //    {
    //        Debug.Log("error: no targets for cast");
    //        battleTimeline.RemoveCurrentTurn();
    //        StartCoroutine(AdvanceTimelineCo());
    //        return;
    //    }
    //}
    #endregion

    #region Intervention
    //public void AddInterventionToQueue()
    //{
    //    if(!InterventionInQueue)
    //    {
    //        InterventionInQueue = true;
    //        Turn interventionTurn = new Turn(TurnType.Intervention);
    //        TurnQueue.Add(interventionTurn);
    //        battleTimeline.CreateTurnPanel(TurnQueue.Count - 1, interventionTurn);
    //        UpdateTurnOrder();
    //    }
    //}

    //public void TriggerIntervention()
    //{
    //    //store data on current turn
    //    CurrentTurn.PauseTurn();

    //    battleTimeline.AddInterventionToQueue(CurrentTurn.Actor);
    //    StartCoroutine(AdvanceTimelineCo());
    //}

    //private void StartIntervention()
    //{
    //    InterventionInQueue = false;
    //    stateMachine.ChangeState((int)BattleStateType.InterventionStart);
    //}

    public void UpdateInterventionPoints(int change)
    {
        InterventionPoints = Mathf.Clamp(InterventionPoints += change, 0, 100);
        if(change > 0)
        {
            StartCoroutine(interventionPointsDisplay.GainPoints(InterventionPoints));
        }
        else
        {
            StartCoroutine(interventionPointsDisplay.SpendPoints(InterventionPoints));
        }
    }
#endregion

    #region Combatant List
    public void SwapPlayableCombatants(PlayableCombatant combatantToSwap)
    {
        PlayableCharacterID oldID = combatantToSwap.PlayableCharacterID;
        PlayableCharacterID newID = combatantToSwap.LinkedCharacterID;

        if (newID == PlayableCharacterID.None)
        {
            return;
        }

        Tile tile = combatantToSwap.Tile;
        BattlePartyPanel panel = combatantToSwap.BattlePartyPanel;
        RemoveCombatant(combatantToSwap, false);

        PlayableCharacterInfo playableCharacterInfo = partyData.PlayableCharacterInfoDict[newID];
        PlayableCombatant combatantToAdd = playableCharacterSpawner.SpawnPlayableCharacter(playableCharacterInfo, tile, oldID);
        
        AddCombatant(combatantToAdd);
        panel.AssignCombatant(combatantToAdd);
        combatantToAdd.AssignBattlePartyPanel(panel);

        battleTimeline.UpdateCasts();
    }

    public void AddCombatant(Combatant combatant)
    {
        Combatants.Add(combatant);

        if (combatant is EnemyCombatant)
        {
            if(!enemyInstances.ContainsKey(combatant.CharacterName))
            {
                enemyInstances.Add(combatant.CharacterName, 1);
            }
            else 
            {
                enemyInstances[combatant.CharacterName] += 1;
            }
            char letter = (char)(enemyInstances[combatant.CharacterName] + 64);
            combatant.SetName(combatant.CharacterName, letter.ToString());
        }
        battleTimeline.AddCombatant(combatant);
    }

    public void RemoveCombatant(Combatant combatant, bool isDead)
    {
        Combatants.Remove(combatant);
        battleTimeline.RemoveCombatant(combatant, isDead);
    }

    public void KOCombatant(Combatant combatant)
    {
        combatant.OnKO();
        if (combatant is PlayableCombatant)
        {
            KOPlayableCombatants.Add(combatant);
        }
        RemoveCombatant(combatant, true);
    }

    public List<Combatant> GetCombatants(CombatantType combatantType)
    {
        List<Combatant> filteredCombatants = new List<Combatant>();
        foreach (Combatant combatant in Combatants)
        {
            if (combatant.CombatantType == combatantType)
            {
                filteredCombatants.Add(combatant);
            }
        }
        return filteredCombatants;
    }
    #endregion

    #region Turn Modifiers

    //public void ApplyTurnModifier(Combatant combatant, float modifier, bool isTemp)
    //{
    //    battleTimeline.ApplyTurnModifier(combatant, modifier, isTemp);
    //}
    //public void ApplyTempTurnModifier(Combatant combatant, float modifier)
    //{
    //    foreach (Turn turn in TurnQueue)
    //    {
    //        if (turn.Actor== combatant)
    //        {
    //            turn.ApplyModifier(modifier, true);
    //        }
    //    }
    //    DisplayTurnModifier(combatant, modifier, false);
    //}

    //public void RemoveTempTurnModifier(Combatant combatant)
    //{
    //    foreach (Turn turn in TurnQueue)
    //    {
    //        if (turn.Actor == combatant)
    //        {
    //            turn.RemoveTempModifier();
    //        }
    //    }
    //    UpdateTurnOrder();
    //}

    //public void DisplayTurnModifier(Combatant combatant, float turnModifier, bool publish)
    //{
    //    List<Turn> tempQueue = new List<Turn>(TurnQueue);
    //    for (int i = TurnQueue.Count - 1; i >= 0; i--)
    //    {
    //        if (TurnQueue[i].Actor == combatant)
    //        {
    //            Turn turnToRemove = TurnQueue[i];
    //            tempQueue.Remove(turnToRemove);
    //            //if speed down, lose any "ties"
    //            if (turnModifier > 0)
    //            {
    //                tempQueue.Add(turnToRemove);
    //            }
    //            //if speed up buff, win any "ties"
    //            else
    //            {
    //                tempQueue.Insert(0, turnToRemove);
    //            }
    //        }
    //    }
    //    tempQueue = tempQueue.OrderBy(c => c.Counter).ToList();
    //    if (publish)
    //    {
    //        TurnQueue = tempQueue;
    //    }
    //    battleTimeline.UpdateTurnPanels(tempQueue);
    //}
    #endregion 

    #region Action Events
    //set action to be executed in execution phase
    //public void SetAction(Action action)
    //{
    //    CurrentTurn.SetAction(action);
    //}
    //cancel selected action
    //public void CancelAction()
    //{
    //    CurrentTurn.CancelActionEvent();
    //}

    //public void ApplyAction()
    //{
    //    //apply hp/mp cost
    //    CurrentTurn.Actor.ApplyActionCost(CurrentTurn.ActionEvent.Action.ActionCostType, CurrentTurn.ActionEvent.Action.Cost);
    //    //consume item and destroy runtime only scriptable object
    //    if (CurrentTurn.ActionEvent.Action is ActionUseItem)
    //    { 
    //        ActionUseItem actionUseItem = (ActionUseItem)CurrentTurn.ActionEvent.Action;
    //        Debug.Log(actionUseItem.UsableItem.ItemName);
    //        inventory.RemoveItem(actionUseItem.UsableItem);
    //        Destroy(actionUseItem);
    //    }
    //}
    #endregion


    #region Targeting

    //public void DisableTargetSelect()
    //{
    //    foreach(Combatant combatant in Combatants)
    //    {
    //        combatant.ChangeSelectState(CombatantTargetState.Default);
    //    }
    //}

    //public void DisplayTimelineTargetPreview(List<Combatant> targets)
    //{
    //    battleTimeline.DisplayTargetPreview(CurrentTurn.ActionEvent.Action, targets);
    //}

    //public void ClearTimelineTargetPreview()
    //{
    //    battleTimeline.ClearTargetPreview();
    //}
    #endregion
}
