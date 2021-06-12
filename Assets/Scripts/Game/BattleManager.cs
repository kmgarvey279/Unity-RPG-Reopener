using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using StateMachineNamespace;

[System.Serializable]
public class TurnSlot
{
    public Combatant combatant;
    private float turnCounterBase = 200;
    public float turnCounter;

    public float StartingCounterValue()
    {
        int agility = combatant.battleStats.statDict[StatType.Agility].GetValue();
        return turnCounterBase - agility;
    } 

    public TurnSlot(Combatant _combatant)
    {
        combatant = _combatant;
        turnCounter = StartingCounterValue();
    }
}

[System.Serializable]
public class TurnData
{
    public Combatant combatant;
    public Action action;
    public List<Combatant> targets = new List<Combatant>();
    public Tile targetedTile;
    public bool hasMoved;

    public TurnData(Combatant combatant)
    {
        this.combatant = combatant;
    }
}

public class BattleManager : MonoBehaviour
{
    [Header("Game Data Scriptable Object")]
    [SerializeField] private GameData gameData;
    [Header("Parties")]
    public BattleParty allyParty;
    public EnemyParty enemyParty;
    [Header("Turn Order Display")]
    [SerializeField] private BattleTimeline battleTimeline;
    [SerializeField] private List<TurnSlot> turnForecast = new List<TurnSlot>();
    [Header("States")]
    public StateMachine stateMachine;
    [Header("Current Turn Data")]
    public TurnData turnData;

    public SignalSenderGO changeCameraFocus;

    public void Start()
    {
        foreach (Combatant combatant in allyParty.combatants)
        {
            TurnSlot newSlot = new TurnSlot(combatant);
            turnForecast.Add(newSlot);
        }
        foreach (Combatant combatant in enemyParty.combatants)
        {
            TurnSlot newSlot = new TurnSlot(combatant);
            turnForecast.Add(newSlot);
        }
        turnForecast.Sort((x, y) => x.turnCounter.CompareTo(y.turnCounter));
        
        AdvanceTimeline();
    }

    private void AdvanceTimeline()
    {
        if(turnForecast[0].turnCounter > 0)
        {
            float timeToAdvance = turnForecast[0].turnCounter;
            foreach(TurnSlot turnSlot in turnForecast)
            {
                turnSlot.turnCounter =  turnSlot.turnCounter - timeToAdvance;
            }
        }
        StartTurn();
    }

    public void EndBattle()
    {
        Debug.Log("End Battle");
    }

    public void StartTurn()
    {   
        TurnSlot currentTurnSlot = turnForecast[0];
        //update turn forecast
        turnForecast.Remove(currentTurnSlot);
        currentTurnSlot.turnCounter = currentTurnSlot.StartingCounterValue();
        TurnForecastAdd(currentTurnSlot);

        //create temp turn data
        turnData = new TurnData(currentTurnSlot.combatant);

        //get next state
        if(currentTurnSlot.combatant is AllyCombatant)
        {
            stateMachine.ChangeState("BattleMenu");
        }
        else
        {
            stateMachine.ChangeState("EnemyTurn");
        }
    }

    private void TurnForecastAdd(TurnSlot turnSlot)
    {
        turnForecast.Add(turnSlot);
        turnForecast.Sort((x, y) => x.turnCounter.CompareTo(y.turnCounter));
        battleTimeline.UpdateTurnList(turnForecast[0], turnForecast);
    }

    public void EndTurn()
    {   
        turnData = null;
        AdvanceTimeline();
    }
}
