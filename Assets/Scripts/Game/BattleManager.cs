using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using StateMachineNamespace;
using BattleCalculationsNamespace;

//stores information related to combatant's place in turn queue
[System.Serializable]
public class TurnSlot
{
    public Combatant combatant;
    [Header("Number of ticks remaining until next turn")]
    public int turnCounter;
    [Header("Cost of action this turn")]
    public float actionCost;
    public float defaultActionCost = 4f;
    [Header("Cost of movement this turn")]
    public float moveCost;
    public float defaultMoveCost = 0f;

    private float speedMultiplier = 1f;

    public TurnSlot(Combatant _combatant)
    {
        combatant = _combatant;
        SetTurnCounterToDefault();
    }

    public void SetTurnCounterToDefault()
    {
        actionCost = defaultActionCost;
        moveCost = defaultMoveCost;
        UpdateTurnCounter();
    }

    public void UpdateTurnCounter()
    {
        turnCounter = Mathf.FloorToInt((actionCost + moveCost) * 100 / GetSpeed());
    }

    public void SetActionCost(float actionCost)
    {
        this.actionCost = actionCost;
        UpdateTurnCounter();
    }

    public void SetMoveCost(float moveCost)
    {
        this.moveCost = moveCost;
        UpdateTurnCounter();
    }

    public void SetSpeedMultiplier(float newModifier)
    {
        speedMultiplier = newModifier;
        UpdateTurnCounter();
    }

    public float GetSpeed()
    {
        float speedTemp = (float)combatant.battleStatDict[BattleStatType.Speed].GetValue() * speedMultiplier;
        //is character is slowed, round down
        if(speedMultiplier < 1)
        {
            return Mathf.Floor(speedTemp);
        }
        //if speed up, round up
        else if(speedMultiplier > 1)
        {
            return Mathf.Ceil(speedTemp);
        }
        //otherwise, round normally
        else
        {
            return Mathf.Round(speedTemp);
        }
    }

    public void Tick()
    {
        turnCounter = turnCounter - 1;
    }
}

//stores information related to the current turn
[System.Serializable]
public class TurnData
{
    public Combatant combatant;
    public Action action;
    [Header("Targets")]
    public List<Combatant> targets = new List<Combatant>();
    public Tile targetedTile;
    [Header("Starting Location")]
    public Vector3 startingPosition;
    public Vector2 startingDirection;

    public TurnData(Combatant combatant)
    {
        this.combatant = combatant;
        this.startingPosition = combatant.transform.position;
        this.startingDirection = combatant.GetDirection();
    }
}

public class BattleManager : MonoBehaviour
{
    private BattleCalculations battleCalculations;
    [SerializeField] private GridManager gridManager;
    [Header("Game Data Scriptable Object")]
    [SerializeField] private GameData gameData;
    
    [Header("Parties")]
    public BattleParty allyParty;
    public EnemyParty enemyParty;
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    
    [Header("Turn Order Display")]
    [SerializeField] private BattleTimeline battleTimeline;
    private TurnSlot currentTurnSlot;
    public List<TurnSlot> turnForecast = new List<TurnSlot>();
    
    [Header("States")]
    public StateMachine stateMachine;
    
    [Header("Current Turn Data")]
    public TurnData turnData;

    public void Start()
    {
        battleCalculations = new BattleCalculations();
        //generate turn forecast & display it
        foreach(Combatant combatant in allyParty.combatants)
        {
            TurnSlot newSlot = new TurnSlot(combatant);
            turnForecast.Add(newSlot);
            //create playable character status panel
            battlePartyHUD.CreatePartyPanel((AllyCombatant)combatant);
        }
        foreach(Combatant combatant in enemyParty.combatants)
        {
            TurnSlot newSlot = new TurnSlot(combatant);
            turnForecast.Add(newSlot);
        }
        turnForecast = turnForecast.OrderBy(o=>o.turnCounter).ToList();
        battleTimeline.CreateTurnPanels(turnForecast);

        AdvanceTimeline();
    }

    private void AdvanceTimeline()
    {
        while(turnForecast[0].turnCounter > 0)
        {
            foreach(TurnSlot turnSlot in turnForecast)
            {
                turnSlot.Tick();
            }
            battleTimeline.UpdateTurnPanels(turnForecast);
        }
        StartTurn();
    }

    public void EndBattle()
    {
        Debug.Log("End Battle");
    }

    public void StartTurn()
    {   
        currentTurnSlot = turnForecast[0];
        battleTimeline.ChangeCurrentTurn(currentTurnSlot);
        battleTimeline.ToggleNextTurnIndicator(currentTurnSlot, true);
        currentTurnSlot.SetTurnCounterToDefault();
        UpdateTurnOrder();

        //create temp turn data
        turnData = new TurnData(currentTurnSlot.combatant);

        //get next state
        if(currentTurnSlot.combatant is AllyCombatant)
        {
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else
        {
            stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
        }
    }

    public void TurnForecastAdd(TurnSlot turnSlot)
    {
        turnForecast.Add(turnSlot);
        UpdateTurnOrder();
    }

    public void TurnForecastRemove(TurnSlot turnSlot)
    {
        turnForecast.Remove(turnSlot);
        UpdateTurnOrder();
    }

    public void UpdateTurnOrder()
    {
        turnForecast = turnForecast.OrderBy(o=>o.turnCounter).ToList();
        battleTimeline.UpdateTurnPanels(turnForecast);
    }
    public void SetMoveCost(int moveCost)
    {
        currentTurnSlot.SetMoveCost(moveCost);
        UpdateTurnOrder();
    }
    //set action to be executed in execution phase
    public void SetAction(Action action)
    {
        turnData.action = action;
        
        currentTurnSlot.SetActionCost(action.timeCost);
        UpdateTurnOrder();
    }
    //set tile and combatants to be targeted in execution phase
    public void SetTargets(Tile selectedTile, List<Combatant> selectedTargets)
    {
        turnData.targets = selectedTargets;
        turnData.targetedTile = selectedTile;
    }
    //cancel selected action and movement
    public void CancelAction()
    {
        turnData.action = null;
        
        currentTurnSlot.SetTurnCounterToDefault();
        UpdateTurnOrder();
    }

    public void EndTurn()
    {   
        Debug.Log("End Turn");
        battleTimeline.ToggleNextTurnIndicator(currentTurnSlot, false);
        turnData = null;
        AdvanceTimeline();
    }

    public void OnTileSelect(GameObject tileObject)
    {
        Tile tile = tileObject.GetComponent<Tile>();
        if(tile.moveCost > -1)
        {
            currentTurnSlot.SetMoveCost(tile.moveCost);
            UpdateTurnOrder();
        }
    }

    public void OnTargetSelect(GameObject gameObject)
    {
        //find target in turn forecast
        Combatant target = gameObject.GetComponent<Combatant>();
        TurnSlot selectedTurnSlot = turnForecast.FirstOrDefault(turnSlot => turnSlot.combatant == target);
        //get accuracy
        if(selectedTurnSlot != null)
        {
            int accuracy = battleCalculations.GetHitChance(turnData.action, turnData.combatant, target, gridManager.GetMoveCost(turnData.combatant.tile, target.tile));
            battleTimeline.DisplayAccuracyPreview(selectedTurnSlot, accuracy);
        }
    }

    public void OnTargetDeselect(GameObject gameObject)
    {
        Debug.Log("Deselect");
        Combatant target = gameObject.GetComponent<Combatant>();
        TurnSlot selectedTurnSlot = turnForecast.FirstOrDefault(turnSlot => turnSlot.combatant == target);
        if(selectedTurnSlot != null)
        { 
            battleTimeline.ClearAccuracyPreview(selectedTurnSlot);
        }
    }
}
