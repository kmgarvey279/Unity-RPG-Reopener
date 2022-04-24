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
    private int turnCounter = 100;
    public int finalTurnCounter;

    public TurnSlot(Combatant combatant)
    {
        this.combatant = combatant;
    }

    public void SetTurnCounterToDefault()
    {
        turnCounter = 100;
    }

    public int GetCounterValue()
    {
        int speedBonus = Mathf.FloorToInt((float)combatant.battleStatDict[BattleStatType.Speed].GetValue() / 2f);
        return Mathf.Clamp((turnCounter - speedBonus), 0, 100);
    }

    public void Tick()
    {
        if(turnCounter > 0)
        {
            turnCounter = turnCounter - 1;
        }
    }
}

//stores information related to the current turn
[System.Serializable]
public class TurnData
{
    public Combatant combatant;
    public Action action;
    public int actionPoints = 0;
    [Header("Targets")]
    public List<Combatant> targets = new List<Combatant>();
    public Tile targetedTile;
    public TargetType targetType;

    public TurnData(Combatant combatant)
    {
        this.combatant = combatant;;
    }
}

public class BattleManager : MonoBehaviour
{
    private BattleCalculations battleCalculations;
    public GridManager gridManager;
    [Header("Game Data Scriptable Object")]
    // [SerializeField] private BattleData battleData;
    
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

    [Header("Action Points")]
    [SerializeField] private ActionPointDisplay actionPointDisplay;
    
    [Header("States")]
    public StateMachine stateMachine;
    
    [Header("Current Turn Data")]
    public TurnData turnData;

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
        currentTurnSlot = turnForecast[0];
        battleTimeline.ChangeCurrentTurn(currentTurnSlot);
        StartCoroutine(StartBattleCo());
    }

    private IEnumerator StartBattleCo()
    {
        yield return new WaitForSeconds(0.2f);
        AdvanceTimeline();
    }

    private void AdvanceTimeline()
    {
        while(turnForecast[0].GetCounterValue() > 0)
        {
            foreach(TurnSlot turnSlot in turnForecast)
            {
                turnSlot.Tick();
            }
            // battleTimeline.UpdateTurnPanels(turnForecast);
        }
        StartTurn();
    }

    public void StartTurn()
    {   
        //set combatant turn slot as current turn
        currentTurnSlot = turnForecast[0];
        battleTimeline.ChangeCurrentTurn(currentTurnSlot);
        currentTurnSlot.SetTurnCounterToDefault();

        //update timeline
        battleTimeline.ToggleNextTurnColor(currentTurnSlot, true);
        UpdateTurnOrder();

        //create temp turn data
        turnData = new TurnData(currentTurnSlot.combatant);

        //update action point display
        UpdateActionPoints(2);

        //get next state
        if(currentTurnSlot.combatant is PlayableCombatant)
        {
            Debug.Log("Player Turn Start");
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else
        {
            Debug.Log("Enemy Turn Start");
            stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
        }
    }

    private void UpdateActionPoints(int actionPointChange)
    {
        turnData.actionPoints += actionPointChange;
        actionPointDisplay.DisplayAP(turnData.actionPoints);
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
        battleTimeline.DestroyTurnPanel(turnSlot);
        UpdateTurnOrder();
    }

    public void UpdateTurnOrder()
    {
        turnForecast = turnForecast.OrderBy(o=>o.GetCounterValue()).ToList();
        battleTimeline.UpdateTurnPanels(turnForecast);
    }
    //set action to be executed in execution phase
    public void SetAction(Action action)
    {
        turnData.action = action;
        // turnData.combatant.ShowMPPreview(action.mpCost);
        actionPointDisplay.ShowPreview(action.apCost);
    }
    //set tile and combatants to be targeted in execution phase
    public void SetTargets(Tile selectedTile, List<Combatant> selectedTargets, TargetType targetType)
    {
        turnData.targets = selectedTargets;
        turnData.targetedTile = selectedTile;
        turnData.targetType = targetType;
    }
    public void UpdateActionCost(int actionCost)
    {
        turnData.actionPoints -= actionCost;
    }
    //cancel selected action and movement
    public void CancelAction()
    {
        turnData.action = null;
        actionPointDisplay.HidePreview();
    }
    public void EndAction()
    {
        UpdateActionPoints(-turnData.action.apCost);
        if(turnData.actionPoints > 0)
        {
            if(currentTurnSlot.combatant is PlayableCombatant)
            {
                stateMachine.ChangeState((int)BattleStateType.Menu);
            }
            else
            {
                stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
            }
        }
        else
        {
            StartCoroutine(EndTurnCo());
        }
    }

    public IEnumerator EndTurnCo()
    {   
        battleTimeline.ToggleNextTurnColor(currentTurnSlot, false);   
        bool allPartyMembersKO = true;
        foreach(PlayableCombatant playableCombatant in playableCombatants)
        {
            if(playableCombatant.ko == false)
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

    public void OnCombatantKO(GameObject gameObject)
    {
        Debug.Log("KO signal recieved!");
        Combatant combatant = gameObject.GetComponent<Combatant>();
        TurnSlot selectedTurnSlot = turnForecast.FirstOrDefault(turnSlot => turnSlot.combatant == combatant);
        TurnForecastRemove(selectedTurnSlot);
        if(combatant is EnemyCombatant)
        {
            enemyCombatants.Remove(combatant);
        } 
    }
}
