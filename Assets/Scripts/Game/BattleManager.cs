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
    public float defaultActionCost = 40f;
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
        //is character is slowed, round down
        if(speedMultiplier < 1)
        {
            return Mathf.Floor((float)combatant.GetStatValue(StatType.Agility) * speedMultiplier);
        }
        //if sped up, round up
        else if(speedMultiplier > 1)
        {
            return Mathf.Ceil((float)combatant.GetStatValue(StatType.Agility) * speedMultiplier);
        }
        //otherwise, round normally
        else
        {
            return Mathf.Round((float)combatant.GetStatValue(StatType.Agility) * speedMultiplier);
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
    [Header("Game Data Scriptable Object")]
    [SerializeField] private GameData gameData;
    
    [Header("Parties")]
    public BattleParty allyParty;
    public EnemyParty enemyParty;
    
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
        //remove from queue and set as current turn
        // turnForecast.Remove(currentTurnSlot);
        battleTimeline.ChangeCurrentTurn(currentTurnSlot);
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
        turnData = null;
        AdvanceTimeline();
    }

    public void OnTileSelect(GameObject tileObject)
    {
        Tile tile = tileObject.GetComponent<Tile>();
        if(tile.moveCost > -1)
        {
            currentTurnSlot.SetMoveCost(tile.moveCost * 10);
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
            int accuracy = battleCalculations.GetHitChance(turnData.action.accuracy, turnData.combatant.GetStatValue(StatType.Skill), target.GetStatValue(StatType.Agility));
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
