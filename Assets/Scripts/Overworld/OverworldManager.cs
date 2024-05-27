using StateMachineNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private OverworldData overworldData;
    //[Header("Events")]
    //[SerializeField] private SignalSender onScreenFadeIn;
    //[SerializeField] private SignalSender onScreenFadeOut;
    [SerializeField] private SignalSender onPauseStart;
    [SerializeField] private SignalSender onPauseEnd;

    //[Header("Player")]
    //[SerializeField] private GameObject playerPrefab;
    //private Player player;

    //[Header("Spawn Points")]
    //[SerializeField] private Transform defaultPlayerSpawnPoint;
    [SerializeField] private PlayerSpawner playerSpawner;

    private WaitForSeconds wait1 = new WaitForSeconds(1f);

    //public IEnumerator LoadAreaCo()
    //{
    //    yield return StartCoroutine(SpawnPlayerCo());
    //    onScreenFadeIn.Raise();
    //    yield return wait1;
    //}

    //private void OnEnable()
    //{
    //    CheckPlayerData();
    //    StartCoroutine(SpawnPlayerCo());
    //}

    //private void CheckPlayerData()
    //{
    //    Scene thisScene = SceneManager.GetActiveScene();
    //    SaveManager.Instance.LoadedData.PlayerData.OnEnterScene(thisScene.name);

    //    ItemContainer[] itemContainers = FindObjectsOfType<ItemContainer>();
    //    foreach (ItemContainer itemContainer in itemContainers)
    //    {
    //        Guid thisItemInstanceID = itemContainer.ItemInstanceID.Guid;

    //        if (SaveManager.Instance.LoadedData.PlayerData.WorldData.SceneEntries.ContainsKey(thisScene.name))
    //        {
    //            if (SaveManager.Instance.LoadedData.PlayerData.WorldData.SceneEntries[thisScene.name].AcquiredItems.Contains(thisItemInstanceID.ToString()))
    //            {
    //                itemContainer.DeactivateItem();
    //            }
    //        }
    //    }
    //}

    //private IEnumerator SpawnPlayerCo()
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (playerSpawner != null)
    //    {
    //        playerSpawner.SpawnPlayer(overworldData.NextSceneEntryPoint);
    //    }
    //}

    //private IEnumerator ResolveChangeRoomCo(Vector2 spawnPosition)
    //{
    //    //freeze scene + fade out
    //    stateMachine.ChangeState((int)OverworldStateType.Cutscene);
    //    onScreenFadeOut.Raise();
    //    yield return wait1;

    //    player.transform.position = spawnPosition;
    //    onScreenFadeIn.Raise();
    //    yield return wait1;

    //    stateMachine.ChangeState((int)OverworldStateType.FreeMove);
    //}

    //public void OnEnterSceneConnector(GameObject gameObject)
    //{
    //    if (player = null)
    //    {
    //        return;
    //    }

    //    SceneConnector sceneConnector = gameObject.GetComponent<SceneConnector>();
    //    if (sceneConnector != null && sceneConnector.LinkedScene.SceneName != null)
    //    {
    //        StartCoroutine(ResolveChangeSceneCo(sceneConnector.LinkedScene.SceneName));
    //    }
    //}

    //private IEnumerator ResolveChangeSceneCo(string newSceneName)
    //{
    //    Scene newScene = SceneManager.GetSceneByName(newSceneName);
    //    if (newScene != null)
    //    {
    //        stateMachine.ChangeState((int)OverworldStateType.Cutscene);
    //        onScreenFadeOut.Raise();
    //        yield return wait1;

    //        //store name of previous scene
    //        Scene thisScene = SceneManager.GetActiveScene();
    //        overworldData.SetPreviousSceneName(thisScene.name);

    //        //load new scene
    //        SceneManager.LoadScene(newSceneName);
    //        yield return wait1;

    //        //switch to new scene
    //        SceneManager.SetActiveScene(newScene);
    //    }
    //    else
    //    {
    //        Debug.Log("Error: invalid name, scene not found!");
    //        yield return null;
    //    }
    //}

    public void PauseStart()
    {
        onPauseStart.Raise();
    }

    public void PauseEnd()
    {
        onPauseEnd.Raise();
    }
}
