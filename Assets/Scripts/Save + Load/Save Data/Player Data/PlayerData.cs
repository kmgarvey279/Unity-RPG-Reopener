using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Current Scene")]
    public string CurrentSceneName = "";
    public string CurrentLocationName = "New";

    [Header("Data Classes")]
    [SerializeReference] public SystemData SystemData = new SystemData();
    [SerializeReference] public PartyData PartyData = new PartyData();
    [SerializeReference] public InventoryData InventoryData = new InventoryData();
    [SerializeReference] public WorldData WorldData = new WorldData();
    [SerializeReference] public EnemyLog EnemyLog = new EnemyLog();

    public void OnEnterScene(string sceneName, string locationName)
    {
        //WorldData.UpdateSceneList(sceneName);
        CurrentSceneName = sceneName;
        CurrentLocationName = locationName;

        WorldData.UpdateSceneList(sceneName);
    }

    public void OnPickupItem(string sceneName, Guid itemInstanceId, Item item)
    {
        //WorldData.UpdateAcquiredItemList(sceneName, itemInstanceId);
        WorldData.UpdateAcquiredItemList(sceneName, itemInstanceId);

        InventoryData.AddItem(item);
    }
}