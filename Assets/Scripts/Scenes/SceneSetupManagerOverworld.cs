using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetupManagerOverworld : SceneSetupManager
{
    [SerializeField] private string locationName;

    [Header("Overworld")]
    [SerializeField] protected GameObject overworldScreen;
    
    [Header("Battle")]
    [SerializeField] protected GameObject battleScreenPrefab;
    [SerializeField] protected GameObject battleScreen;
    private EnemyContainer engagedEnemyContainer;

    [Header("Signals")]
    [SerializeField] private SignalSender onEnableEnemyInteraction;
    [SerializeField] private SignalSender onDisableEnemyInteraction;

    public void Update()
    {
        Debug.Log("Current Time Scale: " + Time.timeScale);
    }

    //public void Start()
    //{
    //    overworldScreen.SetActive(true);
    //    battleScreen.SetActive(false);
    //}

    public override IEnumerator SetupSceneCo()
    {
        #if UNITY_EDITOR
        if (SaveManager.Instance.LoadedData == null)
        {
            SaveManager.Instance.LoadDebugData();
        }
        #endif

        CheckPlayerData();

        PlayerSpawner playerSpawner = FindObjectOfType<PlayerSpawner>();
        if (playerSpawner != null)
        {
            playerSpawner.SpawnPlayer(overworldData.NextSceneEntryPoint);
        }

        //autosave
        SaveManager.Instance.SaveLoadedData(0);

        yield return null;
    }

    public override IEnumerator BreakdownSceneCo()
    {
        yield return null;
    }

    private void CheckPlayerData()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SaveManager.Instance.LoadedData.PlayerData.OnEnterScene(thisScene.name, locationName);

        ItemContainer[] itemContainers = FindObjectsOfType<ItemContainer>();
        foreach (ItemContainer itemContainer in itemContainers)
        {
            Guid thisItemInstanceID = itemContainer.ItemInstanceID.Guid;

            if (SaveManager.Instance.LoadedData.PlayerData.WorldData.SceneEntries.ContainsKey(thisScene.name))
            {
                if (SaveManager.Instance.LoadedData.PlayerData.WorldData.SceneEntries[thisScene.name].AcquiredItems.Contains(thisItemInstanceID.ToString()))
                {
                    itemContainer.DeactivateItem();
                }
            }
        }
    }

    public IEnumerator OnEnterBattle(EnemyContainer enemyContainer)
    {
        Debug.Log("on enter battle");
        
        engagedEnemyContainer = enemyContainer;
        onDisableEnemyInteraction.Raise();

        Time.timeScale = 0f;
        InputManager.Instance.LockInput();

        yield return new WaitForSecondsRealtime(0.5f);
        onFadeOut.Raise();

        yield return new WaitForSecondsRealtime(0.5f);

        overworldScreen.SetActive(false);
        battleScreen = Instantiate(battleScreenPrefab, new Vector2(0,0), Quaternion.identity);
        
        Time.timeScale = 1f;

        BattleManager battleManager = battleScreen.GetComponentInChildren<BattleManager>();
        StartCoroutine(battleManager.EnterBattleCo(engagedEnemyContainer.EnemyPartyData));
    }

    public void OnExitBattle(bool didRun)
    {
        StartCoroutine(OnExitBattleCo(didRun));
    }

    public IEnumerator OnExitBattleCo(bool didRun)
    {
        Debug.Log("on exit battle");

        Time.timeScale = 0f;
        InputManager.Instance.LockInput();

        onFadeOut.Raise();
        yield return new WaitForSecondsRealtime(0.5f);

        Debug.Log("reached start of corroutine");
        //if (didRun)
        //{
        //temporarily disable all enemy hitboxes and movement. 
        //engagedEnemyContainer.OverworldEnemy
        //}
        //else
        //{
        //deactivate enemy
        //}

        Destroy(battleScreen);
        battleScreen = null;

        overworldScreen.SetActive(true);

        //engagedEnemyContainer.ActivateEnemy(false);
        //engagedEnemyContainer = null;

        Time.timeScale = 1f;

        onFadeIn.Raise();
        yield return new WaitForSecondsRealtime(1f);

        InputManager.Instance.ChangeActionMap(startingActionMap);
        InputManager.Instance.UnlockInput();

        Debug.Log("reached middle of corroutine");

        yield return new WaitForSeconds(3f);
        onEnableEnemyInteraction.Raise();

        Debug.Log("reached end of corroutine");
    }
}
