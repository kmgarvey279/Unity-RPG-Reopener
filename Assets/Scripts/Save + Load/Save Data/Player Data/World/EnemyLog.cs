using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyLogEntry
{
    [field: SerializeField] public string EnemyID { get; private set; }
    [field: SerializeField] public string EnemyName { get; private set; }
    [field: SerializeField] public List<ElementalProperty> RevealedElements { get; private set; } = new List<ElementalProperty>();
    [field: SerializeField] public int KillCount { get; private set; } = 0;


    [Newtonsoft.Json.JsonIgnore]
    public EnemyInfo EnemyInfo
    {
        get
        {
            return DatabaseDirectory.Instance.EnemyDatabase.LookupDictionary[EnemyID];
        }
        set
        {
            EnemyID = value.EnemyID;
        }
    }

    public EnemyLogEntry(EnemyInfo enemyInfo)
    {
        EnemyInfo = enemyInfo;
        EnemyName = enemyInfo.CharacterName;
    }

    public void AddRevealedElement(ElementalProperty elementalProperty)
    {
        if (!RevealedElements.Contains(elementalProperty))
        {
            RevealedElements.Add(elementalProperty);
        }
    }
}


[System.Serializable]
public class EnemyLog
{
    [field: SerializeField] public Dictionary<string, EnemyLogEntry> EnemyEntries { get; private set; } = new Dictionary<string, EnemyLogEntry>();

    public void AddEnemy(EnemyInfo newEnemyInfo)
    {
        if (EnemyEntries.ContainsKey(newEnemyInfo.EnemyID))
        {
            return;
        }

        EnemyLogEntry enemyLogEntry = new EnemyLogEntry(newEnemyInfo);
        EnemyEntries.Add(enemyLogEntry.EnemyID, enemyLogEntry);
    }

    public void AddRevealedElement(EnemyInfo enemyInfo, ElementalProperty elementalProperty)
    {
        if (!EnemyEntries.ContainsKey(enemyInfo.EnemyID))
        {
            return;
        }

        EnemyEntries[enemyInfo.EnemyID].AddRevealedElement(elementalProperty);
    }
}
