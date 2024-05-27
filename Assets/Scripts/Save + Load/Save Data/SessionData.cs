using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    [field: Header("Scene State")]
    [field: SerializeField] public int NextSceneEntryPoint { get; private set; } = 0;
    [field: SerializeField] public List<string> DefeatedEnemyIDs { get; private set; } = new List<string>();
    //[field: Header("Current Battle")]
    //[field: SerializeField] public string EngagedEnemyID { get; private set; }
    //[field: SerializeField] public EnemyPartyData EnemyPartyData { get; private set; }

    public void SetNextSceneEntryPoint(int nextSceneEntryPoint)
    {
        NextSceneEntryPoint = nextSceneEntryPoint;
    }

    //public void OnTriggerBattle(string enemyID, EnemyPartyData enemyPartyData)
    //{
    //    EngagedEnemyID = enemyID;
    //    EnemyPartyData = enemyPartyData;
    //}

    //public void OnExitBattle()
    //{
    //    EngagedEnemyID = "";
    //    EnemyPartyData = null;
    //}

    public void AddDefeatedEnemyID(string enemyID)
    {
        if (!DefeatedEnemyIDs.Contains(enemyID))
            DefeatedEnemyIDs.Add(enemyID);
    }

    public void ResetDefeatedEnemyIDs()
    {
        DefeatedEnemyIDs.Clear();
    }
}
