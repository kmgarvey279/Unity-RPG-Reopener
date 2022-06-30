using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using StateMachineNamespace;
using BattleCalculationsNamespace;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    private BattleCalculations battleCalculations;
    public GridManager gridManager;
    [Header("Game Data Scriptable Object")]
    // [SerializeField] private BattleData battleData;
    
    [Header("Parties")]
    [SerializeField] private PartyData partyData;
    [SerializeField] private EnemyPartyData enemyPartyData;
    private Dictionary<string, int> enemyInstances = new Dictionary<string, int>(); 
    [SerializeField] private List<Combatant> combatants = new List<Combatant>();
    public List<Combatant> Combatants
    {
        get {return combatants;}
        private set {combatants = value;}
    }
    private List<Combatant> koCombatants = new List<Combatant>();
    // private List<Combatant> playableCombatants = new List<Combatant>();
    // public List<Combatant> PlayableCombatants
    // {
    //     get {return playableCombatants;}
    //     private set {playableCombatants = value;}
    // }
    // private List<Combatant> koPlayableCombatants = new List<Combatant>();
    // private List<Combatant> enemyCombatants = new List<Combatant>();
    // public List<Combatant> EnemyCombatants
    // {
    //     get {return enemyCombatants;}
    //     private set {enemyCombatants = value;}
    // }
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private PlayableCharacterSpawner playableCharacterSpawner;
    [SerializeField] private EnemySpawner enemySpawner;
    
    [Header("Turn Order Display")]
    [SerializeField] private BattleTimeline battleTimeline;
    // // private TurnSlot currentTurnSlot;
    // public List<TurnSlot> turnForecast = new List<TurnSlot>();
    
    [Header("States")]
    public StateMachine stateMachine;
    
    [Header("Current Turn Data")]
    public TurnData turnData;

    public bool battleIsLoaded = false;

    public void Update() 
    {
        // if(Input.GetButtonDown("Debug1"))
        // {
        //     TimelineTest();
        // }
    }

    public IEnumerator SpawnCombatants()
    {
        int partySize = 0;
        foreach(PartyMember partyMember in partyData.partyMembers)
        {
            if(partySize < 3 && partyMember.inParty)
            {
                partySize++;
                Combatant combatant = playableCharacterSpawner.SpawnPlayableCharacter(partyMember.playableCharacterInfo, partySize);
                if(combatant != null)
                {
                    yield return new WaitUntil(() => combatant.isLoaded);
                    // playableCombatants.Add(combatant);
                    AddCombatant(combatant);
                
                    battlePartyHUD.CreatePartyPanel((PlayableCombatant)combatant);
                
                    // TurnSlot newSlot = new TurnSlot(combatant);
                    // TurnForecastAdd(newSlot);
                }
            }
        }
        int enemyPartySize = 0;
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        foreach(GameObject enemyPrefab in enemyPartyData.enemyPrefabs)
        {
            enemyPartySize++;
            Combatant combatant = enemySpawner.SpawnEnemy(enemyPrefab, enemyPartySize);
            if(combatant != null)
            {
                yield return new WaitUntil(() => combatant.isLoaded);
                AddCombatant(combatant);
            }
        }
        battleIsLoaded = true;
    }

    public void AdvanceTimeline()
    {
        Combatant nextCombatant = combatants[0];
        while(nextCombatant.turnCounter.GetValue() > 0)
        {
            foreach(Combatant combatant in combatants)
            {
                combatant.turnCounter.Tick();
            }
        }
        // while(turnForecast[0].GetCounterValue() > 0)
        // {
        //     foreach(TurnSlot turnSlot in turnForecast)
        //     {
        //         turnSlot.Tick();
        //     }
        //     // battleTimeline.UpdateTurnPanels(turnForecast);
        // }
        // currentTurnSlot = turnForecast[0];
        battleTimeline.ChangeCurrentTurn(nextCombatant);
    
        nextCombatant.turnCounter.ApplyTurnCost();
        UpdateTurnOrder();
        

        turnData = new TurnData(nextCombatant);
    }

    public void AddCombatant(Combatant combatant)
    {
        combatants.Add(combatant);
        if(combatant is EnemyCombatant)
        {
            if(!enemyInstances.ContainsKey(combatant.characterName))
            {
                enemyInstances.Add(combatant.characterName, 1);
            }
            else 
            {
                enemyInstances[combatant.characterName] += 1;
            }
            Debug.Log(combatant.characterName);
            combatant.characterName = combatant.characterName + " " + (char)(enemyInstances[combatant.characterName] + 64);
        }
        battleTimeline.CreateTurnPanel(combatants.Count - 1, combatant);
        UpdateTurnOrder();
    }

    public void RemoveCombatant(Combatant combatant)
    {
        combatants.Remove(combatant);
        battleTimeline.DestroyTurnPanel(combatant);
        UpdateTurnOrder();
    }

    public List<Combatant> GetCombatants(CombatantType combatantType)
    {
        List<Combatant> filteredCombatants = new List<Combatant>();
        foreach(Combatant combatant in combatants)
        {
            if(combatant.combatantType == combatantType)
            {
                filteredCombatants.Add(combatant);
            }
        }
        return filteredCombatants;
    }

    public void DisplayTimelineChanges(List<Combatant> targets)
    {
        combatants = combatants.OrderBy(combatant => combatant.turnCounter.GetValue()).ToList();
        StartCoroutine(battleTimeline.SwapCombatants(targets, combatants));
    }

    public void DisplayTimelinePreview(List<Combatant> targets)
    {
        List<Combatant> tempList = combatants;
        tempList = tempList.OrderBy(combatant => combatant.turnCounter.GetValue()).ToList();
        StartCoroutine(battleTimeline.SwapCombatants(targets, tempList));
    }

    public void ClearTimelinePreview()
    {
        combatants = combatants.OrderBy(combatant => combatant.turnCounter.GetValue()).ToList();
        battleTimeline.CancelAnimations(combatants);
    }

    // public void TimelineTest()
    // {
    //     Combatant nextCombatant = combatants[0];
    //     battleTimeline.ChangeCurrentTurn(nextCombatant);
    //     while(nextCombatant.turnCounter.GetValue() > 0)
    //     {
    //         foreach(Combatant combatant in combatants)
    //         {
    //             combatant.turnCounter.Tick();
    //         }
    //     }
    
    //     nextCombatant.turnCounter.Reset();
    //     combatants = combatants.OrderBy(combatant => combatant.turnCounter.GetValue()).ToList();

    //     combatants.Add(nextCombatant);
    //     StartCoroutine(battleTimeline.AddCombatants(new List<Combatant>(){nextCombatant}, combatants));
    // }

    public void UpdateTurnOrder()
    {
        // turnForecast = turnForecast.OrderBy(o=>o.GetCounterValue()).ToList();
        // battleTimeline.UpdateTurnPanels(turnForecast);
        combatants = combatants.OrderBy(combatant => combatant.turnCounter.GetValue()).ToList();
        battleTimeline.UpdateTurnPanels(combatants);
    }
    //set action to be executed in execution phase
    public void SetAction(Action action)
    {
        turnData.actionEvent = new ActionEvent(action, turnData.combatant);
        // turnData.combatant.ShowMPPreview(action.mpCost);
    }
    //set tile and combatants to be targeted in execution phase
    public void SetTargets(Combatant primaryTarget, List<Combatant> targets)
    {
        turnData.actionEvent.primaryTarget = primaryTarget;
        turnData.actionEvent.targets = targets;
    }
    //cancel selected action
    public void CancelAction()
    {
        turnData.actionEvent = null;
    }

    public void ApplyAction()
    {
        if(turnData.combatant is PlayableCombatant)
        {
            PlayableCombatant playableCombatant = (PlayableCombatant)turnData.combatant;
            playableCombatant.ChangeMana(turnData.actionEvent.action.mpCost);
        }
        UpdateTurnOrder();
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

    public void KOCombatant(Combatant combatant)
    {
        StartCoroutine(combatant.KOCo());
        // TurnSlot selectedTurnSlot = turnForecast.FirstOrDefault(turnSlot => turnSlot.combatant == combatant);
        // TurnForecastRemove(selectedTurnSlot);
        RemoveCombatant(combatant);
        if(combatant is PlayableCombatant)
        {
            koCombatants.Add(combatant);
        } 
    }

    public void ToggleTargetSelect(bool display)
    {
        foreach(Combatant combatant in combatants)
        {
            combatant.ChangeSelectState(display);
        }
        // GameObject selectedTarget = combatants[0].targetSelect.gameObject;
        // if(turnData.combatant != null)
        // {
        //     selectedTarget = turnData.combatant.targetSelect.gameObject;
        // }
        // EventSystem.current.SetSelectedGameObject(null);
        // EventSystem.current.SetSelectedGameObject(selectedTarget);
    }

    public void DisplayTargetPreview(List<Combatant> targets)
    {
        battleTimeline.HighlightTargets(targets);
    }
}
