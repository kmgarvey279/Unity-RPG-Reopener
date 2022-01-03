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

    public void SetActionCost(float costModifier)
    {
        this.actionCost = defaultActionCost + costModifier;
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
    public Tile startingTile;
    public Vector2 startingDirection;

    public TurnData(Combatant combatant)
    {
        this.combatant = combatant;
        this.startingTile = combatant.tile;
        this.startingDirection = combatant.GetDirection();
    }
}

public class BattleManager : MonoBehaviour
{
    private BattleCalculations battleCalculations;
    public GridManager gridManager;
    [Header("Game Data Scriptable Object")]
    [SerializeField] private GameData gameData;
    
    [Header("Parties")]
    [SerializeField] private PartyData partyData;
    [SerializeField] private EnemyPartyData enemyPartyData;
    private List<Combatant> playableCombatants = new List<Combatant>();
    private List<Combatant> enemyCombatants = new List<Combatant>();
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private PlayableCharacterSpawner playableCharacterSpawner;
    [SerializeField] private EnemySpawner enemySpawner;
    
    [Header("Turn Order Display")]
    [SerializeField] private BattleTimeline battleTimeline;
    private TurnSlot currentTurnSlot;
    public List<TurnSlot> turnForecast = new List<TurnSlot>();
    
    [Header("States")]
    public StateMachine stateMachine;
    
    [Header("Current Turn Data")]
    public TurnData turnData;

    public void AddPlayableCombatant(Combatant combatant)
    {
        playableCombatants.Add(combatant);
    }

    public void AddEnemyCombatant(Combatant combatant)
    {
        enemyCombatants.Add(combatant);
    }

    public void Start()
    {
        int partySize = 0;
        foreach(PartyMember partyMember in partyData.partyMembers)
        {
            if(partySize < 3 && partyMember.inParty)
            {
                partySize++;
                Combatant combatant = playableCharacterSpawner.SpawnPlayableCharacter(partyMember.playableCharacterID, partySize);
                if(combatant != null)
                {
                    playableCombatants.Add(combatant);
                
                    battlePartyHUD.CreatePartyPanel((PlayableCombatant)combatant);
                
                    TurnSlot newSlot = new TurnSlot(combatant);
                    TurnForecastAdd(newSlot);
                }
            }
        }
        int enemyPartySize = 0;
        foreach(GameObject enemyPrefab in enemyPartyData.enemyPrefabs)
        {
            enemyPartySize++;
            Combatant combatant = enemySpawner.SpawnEnemy(enemyPrefab, enemyPartySize);
            if(combatant != null)
            {
                enemyCombatants.Add(combatant);
                
                TurnSlot newSlot = new TurnSlot(combatant);
                TurnForecastAdd(newSlot);

                EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
                enemyCombatant.CreateAggroList(playableCombatants);
            }
        }
        StartCoroutine(BattleStartCo());
    }

    private IEnumerator BattleStartCo()
    {
        yield return new WaitForSeconds(0.4f);
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

    public void StartTurn()
    {   
        currentTurnSlot = turnForecast[0];
        battleTimeline.ChangeCurrentTurn(currentTurnSlot);
        currentTurnSlot.SetTurnCounterToDefault();
        UpdateTurnOrder();

        //create temp turn data
        turnData = new TurnData(currentTurnSlot.combatant);

        //get next state
        if(currentTurnSlot.combatant is PlayableCombatant)
        {
            Debug.Log("Player Turn Start");
            stateMachine.ChangeState((int)BattleStateType.Move);
        }
        else
        {
            Debug.Log("Enemy Turn Start");
            stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
        }
    }

    public void TurnForecastAdd(TurnSlot turnSlot)
    {
        turnForecast.Add(turnSlot);
        battleTimeline.CreateTurnPanel(turnForecast.Count - 1, turnSlot);
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
        
        currentTurnSlot.SetActionCost(action.timeModifier);
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

    public IEnumerator EndTurnCo()
    {   
        bool allPartyMembersKO = true;
        foreach (Combatant combatant in playableCombatants)
        {
            if(combatant.ko == false)
            {
                allPartyMembersKO = false;
            }
        }
        yield return new WaitForSeconds(1f);
        if(enemyCombatants.Count <= 0)
        {
            WinBattle();
        }
        else if(allPartyMembersKO == true)
        {
            LoseBattle();
        }
        else
        {
            turnData = null;
            AdvanceTimeline();
        }
    }

    private void WinBattle()
    {
        Debug.Log("You Win!");
    }

    private void LoseBattle()
    {
        Debug.Log("Game Over");
    }

    private void EscapeBattle()
    {
        Debug.Log("You escaped!");
    }

    public void OnPlayableCombatantHeal(Combatant healer, int amount)
    {
        if(enemyCombatants.Count > 0)
        {
            foreach(Combatant combatant in enemyCombatants)
            {
                EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
                enemyCombatant.UpdateAggro(healer, Mathf.RoundToInt(amount / 2f));
            }
        }
    }

    public void OnCombatantKO(Combatant combatant)
    {
        TurnSlot selectedTurnSlot = turnForecast.FirstOrDefault(turnSlot => turnSlot.combatant == combatant);
        TurnForecastRemove(selectedTurnSlot);
        if(combatant is EnemyCombatant)
        {
            enemyCombatants.Remove(combatant);
        } 
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
    }

    public void OnTargetDeselect(GameObject gameObject)
    {
        Combatant target = gameObject.GetComponent<Combatant>();
        TurnSlot selectedTurnSlot = turnForecast.FirstOrDefault(turnSlot => turnSlot.combatant == target);
    }
}
