using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using StateMachineNamespace;

public enum CombatantType
{
    Player,
    Enemy,
    None,
    All
}

public class BattleManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private PartyData partyData;
    [SerializeField] private EnemyPartyData enemyPartyData;

    [Header("Combatant Prefabs")]
    [SerializeField] private GameObject claire;
    [SerializeField] private GameObject mutiny;
    [SerializeField] private GameObject shad;
    [SerializeField] private GameObject blaine;
    [SerializeField] private GameObject lucy;
    [SerializeField] private GameObject oshi;
    private Dictionary<PlayableCharacterID, GameObject> playableCombatantPrefabs = new Dictionary<PlayableCharacterID, GameObject>();
    private Dictionary<PlayableCharacterID, PlayableCombatant> spawnedPlayableCharacters = new Dictionary<PlayableCharacterID, PlayableCombatant>();
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject playerParent;
    [SerializeField] private GameObject enemyParent;

    [Header("Battlefield")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private GridManager gridManager;

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
    //active combatants
    [field: SerializeField] public List<Combatant> PlayableCombatants { get; private set; } = new List<Combatant>();
    [field: SerializeField] public List<Combatant> EnemyCombatants { get; private set; } = new List<Combatant>();
    public bool BattleIsLoaded { get; private set; } = false;

    #region Setup

    public void Awake()
    {
        playableCombatantPrefabs.Add(PlayableCharacterID.Claire, claire);
        playableCombatantPrefabs.Add(PlayableCharacterID.Mutiny, mutiny);
        playableCombatantPrefabs.Add(PlayableCharacterID.Shad, shad);
        playableCombatantPrefabs.Add(PlayableCharacterID.Blaine, blaine);
        playableCombatantPrefabs.Add(PlayableCharacterID.Lucy, lucy);
        playableCombatantPrefabs.Add(PlayableCharacterID.Oshi, oshi);
    }
    public IEnumerator StartBattleCo()
    {
        yield return StartCoroutine(SpawnCombatants());
        Debug.Log("Combatants spawned.");
        onScreenFadeIn.Raise();
        yield return waitOne;
    }

    public IEnumerator SpawnCombatants()
    {
        int battleIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            PlayableCharacterID activeCharacterID = partyData.PartySlots[i];
            PlayableCharacterID linkedCharacterID = partyData.PartySlots[i + 3];
            if (activeCharacterID != PlayableCharacterID.None && !spawnedPlayableCharacters.ContainsKey(activeCharacterID))
            {
                PlayableCharacterInfo activeCharacterInfo = partyData.PlayableCharacterInfoDict[activeCharacterID];
                Tile tile = gridManager.GetTileArray(CombatantType.Player)[0, i];
                PlayableCombatant activeCombatant = SpawnPlayableCharacter(activeCharacterInfo, tile, linkedCharacterID);
                if (activeCombatant != null)
                {
                    spawnedPlayableCharacters.Add(activeCharacterID, activeCombatant);
                    AddCombatant(activeCombatant, battleIndex);
                    battlePartyHUD.CreatePartyPanel(activeCombatant, battleIndex);
                    battleIndex++;
                }
            }
            if (linkedCharacterID != PlayableCharacterID.None && !spawnedPlayableCharacters.ContainsKey(linkedCharacterID))
            {
                PlayableCharacterInfo linkedCharacterInfo = partyData.PlayableCharacterInfoDict[linkedCharacterID];
                Tile tile = gridManager.linkTiles[i];
                PlayableCombatant linkedCombatant = SpawnPlayableCharacter(linkedCharacterInfo, tile, activeCharacterID);
                if (linkedCombatant != null)
                {
                    spawnedPlayableCharacters.Add(linkedCharacterID, linkedCombatant);
                    linkedCombatant.gameObject.SetActive(false);
                }
            }
        }
        battleIndex = 0;
        for (int i = 0; i < 9; i++)
        {
            if (enemyPartyData.Enemies[i] != null)
            {
                EnemyCombatant enemyCombatant = SpawnEnemy(enemyPartyData.Enemies[i], i);
                AddCombatant(enemyCombatant, battleIndex);
                battleIndex++;
            }
        }
        yield return null;
    }
    #endregion

    #region Combatant List
    public void SwapPlayableCombatants(PlayableCombatant combatantToRemove)
    {
        if (combatantToRemove == null)
        {
            Debug.Log("combatant to switch not found");
            return;
        }

        if (combatantToRemove.LinkedCharacterID == PlayableCharacterID.None)
        {
            Debug.Log("no linked combatant");
            return;
        }

        int index = PlayableCombatants.IndexOf(combatantToRemove);
        Tile tile = combatantToRemove.Tile;
        BattlePartyPanel panel = combatantToRemove.BattlePartyPanel;

        battleTimeline.RemoveCombatant(combatantToRemove, false);
        combatantToRemove.transform.position = gridManager.linkTiles[index].transform.position;
        combatantToRemove.gameObject.SetActive(false);

        PlayableCombatant combatantToAdd = spawnedPlayableCharacters[combatantToRemove.LinkedCharacterID];
        tile.AssignOccupier(combatantToAdd);
        combatantToAdd.transform.position = tile.transform.position;
        combatantToAdd.gameObject.SetActive(true);
        
        AddCombatant(combatantToAdd, index);
        battleTimeline.ChangeCurrentCombatant(combatantToAdd);
        panel.AssignCombatant(combatantToAdd);
        combatantToAdd.AssignBattlePartyPanel(panel);

        battleTimeline.UpdateCasts();
    }

    public void AddCombatant(Combatant combatant, int listIndex)
    {        
        if (combatant is EnemyCombatant)
        {
            char letter = (char)(listIndex + 65);
            combatant.SetName(combatant.CharacterName, letter.ToString());
            EnemyCombatants.Add(combatant);
        }
        else if (combatant is PlayableCombatant)
        {
            PlayableCombatants.Insert(listIndex, combatant);
        }
        battleTimeline.AddCombatant(combatant);
    }

    public void KOCombatant(Combatant combatant)
    {
        combatant.OnKO();

        battleTimeline.RemoveCombatant(combatant, true);
        battleTimeline.UpdateCasts();
    }

    public void ReviveCombatant(Combatant combatant)
    {
        battleTimeline.AddCombatant(combatant);
    }

    public List<Combatant> GetCombatants(CombatantType combatantType, bool getKOed = false)
    {
        List<Combatant> filteredCombatants = new List<Combatant>();
        if (combatantType == CombatantType.Player || combatantType == CombatantType.All)
        {
            foreach (Combatant combatant in PlayableCombatants)
            {
                if (combatant.IsKOed == getKOed)
                {
                    filteredCombatants.Add(combatant);
                }
            }
        }
        if (combatantType == CombatantType.Enemy || combatantType == CombatantType.All)
        {
            foreach (Combatant combatant in EnemyCombatants)
            {
                if (combatant.IsKOed == getKOed)
                {
                    filteredCombatants.Add(combatant);
                }
            }
        }
        return filteredCombatants;
    }
    #endregion

    public PlayableCombatant SpawnPlayableCharacter(PlayableCharacterInfo playableCharacterInfo, Tile tile, PlayableCharacterID linkedCharacter)
    {
        GameObject playableCharacterObject = Instantiate(playableCombatantPrefabs[playableCharacterInfo.PlayableCharacterID], tile.transform.position, Quaternion.identity);
        //set transform
        playableCharacterObject.transform.parent = playerParent.transform;
        PlayableCombatant playableCombatant = playableCharacterObject.GetComponent<PlayableCombatant>();
        if (playableCombatant)
        {
            //set data
            playableCombatant.SetCharacterData(playableCharacterInfo, linkedCharacter);
            //set tile
            tile.AssignOccupier(playableCombatant);
            playableCombatant.SetTile(tile);
            //set camera for ui display
            playableCombatant.SetBattleManager(this);
            playableCombatant.SetUICamera(mainCamera);
            return playableCombatant;
        }
        return null;
    }

    public EnemyCombatant SpawnEnemy(EnemyInfo enemyInfo, int positionNum)
    {
        if (positionNum < 9)
        {
            Tile tile = gridManager.enemyTiles[positionNum];
            GameObject enemyObject = Instantiate(enemyPrefab, tile.transform.position, Quaternion.identity);
            enemyObject.transform.parent = enemyParent.transform;
            EnemyCombatant enemyCombatant = enemyObject.GetComponent<EnemyCombatant>();
            if (enemyCombatant)
            {
                //set data
                enemyCombatant.SetCharacterData(enemyInfo);
                //set tile
                tile.AssignOccupier(enemyCombatant);
                enemyCombatant.SetTile(tile);
                enemyCombatant.SetBattleManager(this);
                //set camera for ui display
                enemyCombatant.SetUICamera(mainCamera);
                return enemyCombatant;
            }
            return null;
        }
        return null;
    }

    public bool InterventionCheck(int index)
    {
        if (index < PlayableCombatants.Count)
        {
            Combatant actor = PlayableCombatants[index];
            if (actor.IsKOed || actor.IsCasting)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void LockInterventionTriggerIcons(bool isLocked)
    {
        foreach (Combatant combatant in GetCombatants(CombatantType.Player))
        {
            PlayableCombatant playableCombatant = (PlayableCombatant)combatant;
            if(!playableCombatant.IsCasting)
            {
                playableCombatant.BattlePartyPanel.LockInterventionTriggerIcon(isLocked);
            }
        }
    }
}
