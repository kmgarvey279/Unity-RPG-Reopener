using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class WorldData
{
    public Dictionary<string, SceneData> SceneEntries = new Dictionary<string, SceneData>();

    //public void SetData(WorldData worldData)
    //{
    //    SceneEntries = worldData.SceneEntries;
    //}

    public void UpdateSceneList(string sceneName)
    {
        if (!SceneEntries.ContainsKey(sceneName))
        {
            SceneEntries.Add(sceneName, new SceneData());
        }
    }

    public void UpdateAcquiredItemList(string sceneName, Guid itemInstanceId)
    {
        if (!SceneEntries.ContainsKey(sceneName))
        {
            UpdateSceneList(sceneName);
        }
        SceneEntries[sceneName].OnAcquireItem(itemInstanceId);
    }

    //public SerializableWorldData()
    //{
    //    sceneEntries = new Dictionary<string, SceneData>();
    //}
}
