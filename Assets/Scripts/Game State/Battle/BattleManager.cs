using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public enum CombatantType
{
    Player,
    Enemy,
    None,
    All
}

public class ExpData
{
    [SerializeField] public Sprite Icon { get; private set; }
    [SerializeField] public int LevelStart { get; private set; }
    [SerializeField] public int LevelEnd { get; private set; }
    [SerializeField] public int CurrentEXP { get; private set; }
    [SerializeField] public int NextLevelRequirement { get; private set; }

    public ExpData(Sprite icon, int levelStart, int levelEnd, int currentEXP, int nextLevelRequirement)
    {
        Icon = icon;
        LevelStart = levelStart;
        LevelEnd = levelEnd;
        CurrentEXP = currentEXP;
        NextLevelRequirement = nextLevelRequirement;
    }
}

public class BattleManager : MonoBehaviour
{
    [Header("Parent GameObjects")]
    [SerializeField] private GameObject playerParent;
    [SerializeField] private GameObject reservePlayerParent;
    [SerializeField] private GameObject enemyParent;

    [field: Header("Battlefield")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas battleCanvas;
    [SerializeField] private Battlefield battlefield;
    [SerializeField] private GridManager gridManager;

    [Header("HUD")]
    [SerializeField] private BattleHUD battleHUD;
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private BattleTimeline battleTimeline;

    [Header("Input")]
    private bool canQueueIntervention = false;

    [Header("Battle Event Queue")]
    [SerializeField] private BattleEventQueue battleEventQueue;

    [Header("States")]
    [SerializeField] private StateMachine stateMachine;

    [Header("Signals")]
    [SerializeField] private SignalSender onFadeIn;
    [SerializeField] private SignalSender onFadeOut;
    [SerializeField] private SignalSenderBool onExitBattle;

    [field: Header("Data")]
    //battle data
    [field: SerializeField] public BattleData BattleData { get; private set; } = new BattleData();

    [field: SerializeField] private EnemyPartyData enemyPartyData;
    //active combatants
    [field: SerializeField] public List<PlayableCombatant> PlayableCombatants { get; private set; } = new List<PlayableCombatant>();
    [field: SerializeField] public List<PlayableCombatant> ReservePlayableCombatants { get; private set; } = new List<PlayableCombatant>();
    [field: SerializeField] public List<EnemyCombatant> EnemyCombatants { get; private set; } = new List<EnemyCombatant>();

    //cache wait for seconds
    private WaitForSeconds wait025 = new WaitForSeconds(0.25f);
    private WaitForSeconds wait05 = new WaitForSeconds(0.5f);
    private WaitForSeconds wait1 = new WaitForSeconds(1f);

    private bool canExitVictoryScreen;

    private void OnEnable()
    {
        InputManager.Instance.OnPressIntervention1.AddListener(PressIntervention1);
        InputManager.Instance.OnPressIntervention2.AddListener(PressIntervention2);
        InputManager.Instance.OnPressIntervention3.AddListener(PressIntervention3);

        InputManager.Instance.OnPressSubmit.AddListener(PressSubmit);

        mainCamera = FindFirstObjectByType<Camera>();
        //battleCanvas.
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPressIntervention1.RemoveListener(PressIntervention1);
        InputManager.Instance.OnPressIntervention2.RemoveListener(PressIntervention2);
        InputManager.Instance.OnPressIntervention3.RemoveListener(PressIntervention3);

        InputManager.Instance.OnPressSubmit.RemoveListener(PressSubmit);
    }

    #region Input
    private void PressIntervention1(bool isPressed)
    {
        Debug.Log("On Press Intervention 1");

        OnPressIntervention(0);
    }

    private void PressIntervention2(bool isPressed)
    {
        Debug.Log("On Press Intervention 2");
        
        OnPressIntervention(1);
    }

    private void PressIntervention3(bool isPressed)
    {
        Debug.Log("On Press Intervention 3");

        OnPressIntervention(2);
    }

    private void PressSubmit(bool isPressed)
    {
        if (canExitVictoryScreen)
        {
            ExitVictoryScreen();
        }
    }
    #endregion

    #region Start/End

    public IEnumerator EnterBattleCo(EnemyPartyData _enemyPartyData)
    {
        enemyPartyData = _enemyPartyData;
        yield return StartCoroutine(SetupBattleCo());

        onFadeIn.Raise();
        yield return new WaitForSeconds(1f);

        StartBattle();
    }

    public void StartBattle()
    {
        InputManager.Instance.ChangeActionMap(ActionMapType.Battle);
        InputManager.Instance.UnlockInput();

        stateMachine.ChangeState((int)BattleStateType.BattleStart);
    }

    public void OnVictory()
    {
        List<ExpData> activeEXPData = new List<ExpData>();
        List<ExpData> reserveEXPData = new List<ExpData>();

        List<PlayableCharacterID> activeIDs = PlayableCombatants.Select(combatant => combatant.PlayableCharacterID).ToList();
        foreach (PlayableCharacterID id in activeIDs)
        {
            PlayableCombatantRuntimeData charData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(id);
            int levelStart = charData.Level;

            charData.GainEXP(enemyPartyData.EXP);

            int levelEnd = charData.Level;
            int currentEXP = charData.CurrentEXP;
            int nextLevelRequirement = charData.GetNextLevelRequirement();

            activeEXPData.Add(new ExpData(charData.StaticInfo.TurnIcon, levelStart, levelEnd, currentEXP, nextLevelRequirement));
        }

        List<PlayableCharacterID> reserveIDs = ReservePlayableCombatants.Select(combatant => combatant.PlayableCharacterID).ToList();
        foreach (PlayableCharacterID id in reserveIDs)
        {
            PlayableCombatantRuntimeData charData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(id);
            int levelStart = charData.Level;

            charData.GainEXP(enemyPartyData.EXP);

            int levelEnd = charData.Level;
            int currentEXP = charData.CurrentEXP;
            int nextLevelRequirement = charData.GetNextLevelRequirement();

            reserveEXPData.Add(new ExpData(charData.StaticInfo.TurnIcon, levelStart, levelEnd, currentEXP, nextLevelRequirement));
        }
        battleHUD.DisplayVictoryScreen(activeEXPData, reserveEXPData, enemyPartyData.EXP);
        StartCoroutine(VictoryScreenDurationCo());
    }

    private IEnumerator VictoryScreenDurationCo()
    {
        yield return wait1;

        canExitVictoryScreen = true;
    }

    private void ExitVictoryScreen()
    {
        ExitBattle(false);
    }

    public void ExitBattle(bool didRun)
    {
        //clear
        onExitBattle.Raise(didRun);
    }
    #endregion

    #region Setup

    public IEnumerator SetupBattleCo()
    {
        ToggleCanQueueInterventions(false);
        
        battlefield.LoadEnvironment(enemyPartyData.EnvironmentPrefab);
        MusicManager.Instance.PlayClip(enemyPartyData.BGM, 1);

        yield return StartCoroutine(SpawnCombatants());
    }


    public IEnumerator SpawnCombatants()
    {
        PartyData partyData = SaveManager.Instance.LoadedData.PlayerData.PartyData;
        List<PlayableCharacterID> spawnedPlayableCharacterIDs = new List<PlayableCharacterID>();

        for (int i = 0; i < partyData.PartyMembers.Count; i++)
        {
            PlayableCharacterID characterID = partyData.PartyMembers[i];
            if (!spawnedPlayableCharacterIDs.Contains(characterID))
            {                
                //active party members
                if (partyData.ActivePartyMembers.Contains(characterID))
                {
                    Tile tile = gridManager.PlayerTiles[i];
                    if (tile != null)
                    {
                        PlayableCombatant combatant = SpawnPlayableCharacter(characterID, tile, true);
                        if (combatant != null)
                        {
                            //add to lists
                            spawnedPlayableCharacterIDs.Add(characterID);
                            //active only
                            AddCombatant(combatant, i);
                            battlePartyHUD.CreatePartyPanel(combatant, i);

                        }
                    }
                }
                //reserve party members
                else
                {
                    Tile tile = gridManager.SwapTiles[0];
                    if (tile != null)
                    {
                        PlayableCombatant combatant = SpawnPlayableCharacter(characterID, tile, false);
                        if (combatant != null)
                        {
                            //add to lists
                            spawnedPlayableCharacterIDs.Add(characterID);
                            AddReservePlayableCombatant(combatant);
                        }
                    }
                }
            }
        }
        int enemyCount = 0;
        if (enemyPartyData)
        {
            for (int i = 0; i < 9; i++)
            {
                if (enemyPartyData.Enemies[i] != null)
                {
                    //spawn gameobject
                    EnemyCombatant enemyCombatant = SpawnEnemy(enemyPartyData.Enemies[i], i);
                    //add to list
                    AddCombatant(enemyCombatant, enemyCount);
                    if (enemyCombatant.IsBoss)
                    {
                        BattleData.ModifyBool(BattleDataBool.CannotEscape, true);
                    }
                    //add to log
                    SaveManager.Instance.LoadedData.PlayerData.EnemyLog.AddEnemy(enemyPartyData.Enemies[i]);
                    enemyCount++;
                }
            }
            if (EnemyCombatants.Count < 1)
            {
                Debug.LogError("No enemies have been added to the associated party data.");
            }
        }
        else
        {
            Debug.LogError("Enemy party data has not been set.");
        }
        battleTimeline.PopulateToMax();
        yield return null;
    }
    #endregion

    #region Get Combatants
    public List<Combatant> GetCombatants(CombatantType combatantType)
    {
        List<Combatant> filteredCombatants = new List<Combatant>();
        if (combatantType == CombatantType.Player || combatantType == CombatantType.All)
        {
            foreach (Combatant combatant in PlayableCombatants)
            {
                if (combatant.CombatantState != CombatantState.KO)
                {
                    filteredCombatants.Add(combatant);
                }
            }
        }
        if (combatantType == CombatantType.Enemy || combatantType == CombatantType.All)
        {
            foreach (Combatant combatant in EnemyCombatants)
            {
                filteredCombatants.Add(combatant);
            }
        }
        return filteredCombatants;
    }

    public List<Combatant> GetKOedCombatants()
    {
        List<Combatant> filteredCombatants = new List<Combatant>();
        foreach (Combatant combatant in PlayableCombatants)
        {
            if (combatant.CombatantState == CombatantState.KO)
            {
                filteredCombatants.Add(combatant);
            }
        }
        return filteredCombatants;
    }

    public List<Combatant> GetAltTargets(Combatant actor, TargetingType targetingType, bool pickRandomTarget)
    {
        List<Combatant> targets = new List<Combatant>();
        switch (targetingType)
        {
            case TargetingType.TargetSelf:
                targets.Add(actor);
                break;
            case TargetingType.TargetAllies:
                targets.AddRange(GetCombatants(actor.CombatantType));
                targets.Remove(actor);
                break;
            case TargetingType.TargetKOAllies:
                if (actor.CombatantType == CombatantType.Player)
                {
                    targets.AddRange(GetKOedCombatants());
                }
                break;
            case TargetingType.TargetFriendly:
                targets.AddRange(GetCombatants(actor.CombatantType));
                break;
            case TargetingType.TargetHostile:
                if (actor.CombatantType == CombatantType.Player)
                {
                    targets.AddRange(GetCombatants(CombatantType.Enemy));
                }
                else
                {
                    targets.AddRange(GetCombatants(CombatantType.Player));
                }
                break;
            default:
                break;
        }
        if (targets.Count > 1 && pickRandomTarget)
        {
            int roll = Random.Range(0, targets.Count);
            targets = new List<Combatant>() { targets[roll] };
        }
        return targets;
    }
    #endregion

    #region Add/Remove Combatants 

    public void AddCombatant(Combatant combatant, int listIndex)
    {
        if (combatant is EnemyCombatant)
        {
            char letter = (char)(listIndex + 65);
            combatant.SetName(combatant.CharacterName, letter.ToString());
            EnemyCombatants.Add((EnemyCombatant)combatant);
        }
        else if (combatant is PlayableCombatant)
        {
            PlayableCombatants.Insert(listIndex, (PlayableCombatant)combatant);
        }
        battleTimeline.AddCombatant(combatant);
    }

    public void AddReservePlayableCombatant(PlayableCombatant playableCombatant)
    {
        ReservePlayableCombatants.Add(playableCombatant);
    }

    public IEnumerator SwapPlayableCombatants(PlayableCombatant combatantToRemove, PlayableCombatant combatantToAdd)
    {
        if (combatantToRemove == null || combatantToAdd == null)
        {
            Debug.Log("combatant to switch not found");
            yield return null;
        }

        //new character will be assigned to the following
        int partyIndex = PlayableCombatants.IndexOf(combatantToRemove);
        int reserveIndex = ReservePlayableCombatants.IndexOf(combatantToAdd);
        Tile tile = combatantToRemove.Tile;
        BattlePartyPanel panel = combatantToRemove.BattlePartyPanel;

        //remove original character from battlefield/active party
        yield return combatantToRemove.Move(gridManager.SwapTiles[0].transform, "Move");
        combatantToRemove.ReturnToDefaultAnimation();
        //add to reserve list
        ReservePlayableCombatants[reserveIndex] = combatantToRemove;
        combatantToRemove.transform.parent = reservePlayerParent.transform;

        //add new character to battlefield
        combatantToAdd.gameObject.SetActive(true);
        //party list
        PlayableCombatants[partyIndex] = combatantToAdd;
        combatantToAdd.transform.parent = playerParent.transform;
        //tile
        tile.AssignOccupier(combatantToAdd);
        combatantToAdd.SetTile(tile);
        //panel
        panel.AssignCombatant(combatantToAdd);
        combatantToAdd.AssignBattlePartyPanel(panel);
        //move offscreen and deactivate
        yield return combatantToAdd.Move(tile.transform, "Move");
        combatantToAdd.ReturnToDefaultAnimation();
        combatantToRemove.gameObject.SetActive(false);

        //lock swap until next turn
        battleTimeline.CurrentTurn.OnSwapIn();
    }

    public PlayableCombatant SpawnPlayableCharacter(PlayableCharacterID playableCharacterID, Tile tile, bool isInActiveParty)
    {
        PlayableCombatantRuntimeData data = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
        if (data == null)
        {
            Debug.LogError("Unable to find data for " + playableCharacterID.ToString());
            return null;
        }

        GameObject playableCharacterObject = Instantiate(data.StaticInfo.Prefab, tile.transform.position, Quaternion.identity);
        PlayableCombatant playableCombatant = playableCharacterObject.GetComponent<PlayableCombatant>();
        if (playableCombatant)
        {
            if (isInActiveParty)
            {
                tile.AssignOccupier(playableCombatant);
                playableCombatant.SetTile(tile);

                playableCombatant.transform.parent = playerParent.transform;
            }
            else
            {
                playableCombatant.gameObject.SetActive(false);

                playableCombatant.transform.parent = reservePlayerParent.transform;
            }

            //set data
            playableCombatant.SetCharacterData(data);

            //set references 
            playableCombatant.SetExternalReferences(this, battleTimeline);
            playableCombatant.SetUICamera(mainCamera);

            return playableCombatant;
        }
        Debug.LogError("Unable to spawn " + playableCharacterID.ToString());
        return null;
    }

    public EnemyCombatant SpawnEnemy(EnemyInfo enemyInfo, int positionNum)
    {
        if (positionNum < 9)
        {
            Tile tile = gridManager.EnemyTiles[positionNum];
            GameObject enemyObject = Instantiate(enemyInfo.Prefab, tile.transform.position, Quaternion.identity);
            enemyObject.transform.parent = enemyParent.transform;
            EnemyCombatant enemyCombatant = enemyObject.GetComponent<EnemyCombatant>();
            if (enemyCombatant)
            {
                //set tile
                tile.AssignOccupier(enemyCombatant);
                enemyCombatant.SetTile(tile);
                enemyCombatant.SetExternalReferences(this, battleTimeline);

                //set data
                enemyCombatant.SetCharacterData(enemyInfo);

                //set camera for ui display
                enemyCombatant.SetUICamera(mainCamera);
                return enemyCombatant;
            }
            Debug.LogError("Unable to spawn " + enemyInfo.CharacterName);
            return null;
        }
        Debug.LogError("Invalid enemy position #");
        return null;
    }
    #endregion

    #region Interventions
    private void OnPressIntervention(int index)
    {
        if (index < PlayableCombatants.Count)
        {
            PlayableCombatant actor = (PlayableCombatant)PlayableCombatants[index];
            if (actor.InterventionQueued)
            {
                UnqueueIntervention(actor);
            }
            else
            {
                QueueIntervention(actor);
            }
        }
    }

    private void QueueIntervention(PlayableCombatant actor)
    { 
        if (!canQueueIntervention)
        {
            Debug.Log("intervention locked by battle manager");
            return;
        }

        if (BattleData.CheckBool(BattleDataBool.CannotTriggerIntervention))
        {
            Debug.Log("intervention locked by battle data bool");
            return;
        }

        if (actor.InterventionCheck())
        {
            actor.QueueIntervention();
            battleTimeline.AddInterventionToQueue(actor);
        }
    }

    public void UnqueueIntervention(PlayableCombatant actor)
    {
        if (!canQueueIntervention)
        {
            return;
        }

        battleTimeline.RemoveLastIntervention(actor);
        actor.UnqueueIntervention();
    }

    public void ToggleCanQueueInterventions(bool canQueue)
    {
        canQueueIntervention = canQueue;
        //interventionPointsDisplay.ToggleCanQueueIcon(canQueue);
    }
    #endregion

    #region Misc. Battle Events
    public IEnumerator ResolveBarChanges()
    {
        foreach (Combatant combatant in GetCombatants(CombatantType.All))
        {
            combatant.ResolveBarChanges();
        }
        yield return wait025;
    }

    public IEnumerator ResolveKOs()
    {
        List<Combatant> allCombatants = GetCombatants(CombatantType.All);
        bool waitForKO = false;

        Debug.Log("checking for ko'd combatants");
        //kill marked combatants
        for (int i = allCombatants.Count - 1; i >= 0; i--)
        {
            if (allCombatants[i].CombatantState == CombatantState.PreKO)
            {
                Debug.Log("found ko'd combatant");

                Combatant combatant = allCombatants[i];
                waitForKO = true;
                combatant.OnKO();

                if (combatant.CombatantType == CombatantType.Player)
                {
                    battleTimeline.ToggleKOState(combatant, true);
                }
                else
                {
                    if (EnemyCombatants.Contains(combatant))
                    {
                        EnemyCombatants.Remove((EnemyCombatant)combatant);
                    }
                    battleTimeline.RemoveCombatant(combatant, true);
                }
                //battleTimeline.UpdateCasts();
            }
        }
        //battleTimeline.DisplayTurnOrder();

        if (waitForKO)
        {
            yield return wait05;
        }
        yield return null;
    }

    public void ReviveCombatant(Combatant combatant)
    {
        battleTimeline.ToggleKOState(combatant, false);
    }

    public void ResetCombatantPositions()
    {
        foreach (Combatant combatant in GetCombatants(CombatantType.All))
        {
            StartCoroutine(combatant.ReturnToDefaultPosition());
        }
    }

    public void ResetCombatantAnimations()
    {
        foreach (Combatant combatant in GetCombatants(CombatantType.All))
        {
            combatant.ReturnToDefaultAnimation();
        }
    }

    public void HideHealthBars()
    {
        foreach (Combatant combatant in GetCombatants(CombatantType.All))
        {
            combatant.HideHealthInfo();
        }

        foreach (Combatant combatant in GetKOedCombatants())
        {
            combatant.HideHealthInfo();
        }
    }
    #endregion

    #region Write Player Data
    #endregion
}
