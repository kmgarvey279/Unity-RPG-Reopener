using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    [SerializeField] public List<string> AcquiredItems { get; private set; } = new List<string>();

    public void OnAcquireItem(Guid itemInstanceId)
    {
        if (!AcquiredItems.Contains(itemInstanceId.ToString()))
        {
            AcquiredItems.Add(itemInstanceId.ToString());
        } 
    }
}
