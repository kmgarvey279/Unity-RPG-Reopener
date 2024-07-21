using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Scene Info")]
    [SerializeField] private string locationName;
    [SerializeField] private ActionMapType startingActionMap;
    [SerializeField] private AudioClip bgm;

    [Header("Overworld")]
    [SerializeField] private GameObject overworldScreen;

    [Header("Battle")]
    [SerializeField] private GameObject battleScreenPrefab;
    private GameObject battleScreen;
    private EnemyContainer engagedEnemyContainer;

    [Header("Signals")]
    [SerializeField] private SignalSender onEnableEnemyInteraction;
    [SerializeField] private SignalSender onDisableEnemyInteraction;
    [SerializeField] protected SignalSender onFadeIn;
    [SerializeField] protected SignalSender onFadeOut;

    //protected void OnEnable()
    //{
    //    SceneSwapManager.onLoadComplete.AddListener(OnLoadComplete);
    //}

    //protected void OnDisable()
    //{
    //    SceneSwapManager.onLoadComplete.RemoveListener(OnLoadComplete);
    //}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OnEnable()
    {
        StartCoroutine(EnterSceneCo());
    }

    protected void OnLoadComplete()
    {
        Debug.Log("Load complete!");
        StartCoroutine(EnterSceneCo());
    }

    public IEnumerator EnterSceneCo()
    {
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(SetupSceneCo());

        MusicManager.Instance.PlayClip(bgm, 1);

        onFadeIn.Raise();
        yield return new WaitForSeconds(1f);

        StartScene();
    }

    public IEnumerator OnExitSceneCo(string sceneName, int entryPointIndex = 0)
    {
        SaveManager.Instance.SessionData.SetNextSceneEntryPoint(entryPointIndex);

        Debug.Log("exiting current scene");
        InputManager.Instance.LockInput();

        onFadeOut.Raise();
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(BreakdownSceneCo());

        StartCoroutine(SceneSwapManager.Instance.ChangeSceneCo(sceneName));
    }

    public IEnumerator OnReturnToTitle()
    {
        Debug.Log("exiting to title menu");
        InputManager.Instance.LockInput();

        onFadeOut.Raise();
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SceneSwapManager.Instance.ChangeSceneCo("TitleScreen"));
    }

    public IEnumerator SetupSceneCo()
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
            playerSpawner.SpawnPlayer(SaveManager.Instance.SessionData.NextSceneEntryPoint);
        }

        //autosave
        SaveManager.Instance.SaveLoadedData(0);

        yield return null;
    }

    public void StartScene()
    {
        InputManager.Instance.ChangeActionMap(startingActionMap);
        InputManager.Instance.UnlockInput();
    }

    public virtual IEnumerator BreakdownSceneCo()
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
        battleScreen = Instantiate(battleScreenPrefab, new Vector2(0, 0), Quaternion.identity);

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

        Destroy(battleScreen);
        battleScreen = null;

        overworldScreen.SetActive(true);

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
